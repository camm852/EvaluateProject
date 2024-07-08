using AutoMapper;
using EvaluationProjects.Interfaces;
using EvaluationProjects.Models.Constans;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Entities;
using EvaluationProjects.Models.Enum;
using EvaluationProjects.Persistence.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;

namespace EvaluationProjects.Controllers
{

    [SwaggerTag("Assignment",
                Description = "All endpoints required role program")]
    [ApiController]
    [Authorize(Roles ="program")]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {

        private readonly IGenericRepository<Assignment> _assignmentRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Evaluation> _evaluationRepository;
        private readonly IMapper _mapper;

        public AssignmentController(IGenericRepository<Assignment> assignmentRepository, IMapper mapper, IGenericRepository<Evaluation> evaluationRepository, IGenericRepository<User> userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _evaluationRepository = evaluationRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AssingTeacher([FromBody] AssignmentRequestDto assignmentRequestDto)
        {


            var assignmentStored = await _assignmentRepository.GetAsync(
                assignment => assignment.ProjectId.Equals(assignmentRequestDto.ProjectId)
            );

            if (assignmentStored is null) return NotFound("Assignment not exist");

            var evaluationStored = await _evaluationRepository.GetAsync(x => x.AssignmentId.Equals(assignmentStored.Id));

            if (evaluationStored.StatusId.Equals((int)StatusEnum.Reviewed)) return BadRequest("Project already has reviewed");


            var userStored = await _userRepository.GetAsync(x => x.Id.Equals(assignmentRequestDto.TeacherId));

            if (assignmentRequestDto.TeacherId != null && userStored is null) return BadRequest("Teacher not exists");

            if (assignmentRequestDto.TeacherId != null && !userStored.Role.ToLower().Equals(ROLES.TEACHER.ToLower()))
            {
                return BadRequest("User is not teacher");
            }

            //if (assignmentStored!.TeacherId.Equals(assignmentRequestDto.TeacherId))
            //{
            //    return BadRequest("Assigment Exists");
            //}

            if (assignmentRequestDto.TeacherId != null && !userStored.Role.ToLower().Equals(ROLES.TEACHER.ToLower())) return BadRequest("This user is not Teacher");

            assignmentStored.AssignmentDate = assignmentRequestDto.TeacherId == null ? null : DateOnly.FromDateTime(DateTime.Now);
            assignmentStored.TeacherId = assignmentRequestDto.TeacherId;

            _assignmentRepository.Update(assignmentStored); //Se crea una asignacion


            evaluationStored!.StatusId = assignmentRequestDto.TeacherId == null ? (int)StatusEnum.Waiting : (int)StatusEnum.Revision; // Se actualiza el estado

            _evaluationRepository.Update(evaluationStored);

            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssingnmet() {
            return StatusCode((int)HttpStatusCode.OK, await _assignmentRepository.GetAllAsync());
        }
    }
}

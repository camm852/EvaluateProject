using EvaluationProjects.Interfaces;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Entities;
using EvaluationProjects.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EvaluationProjects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly IGenericRepository<Evaluation> _evaluationRepository;

        public EvaluationController(IGenericRepository<Evaluation> evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }



        /// <summary>
        /// Required role program
        /// </summary>
        [HttpGet]
        [Authorize(Roles ="program")]
        public async Task<IActionResult> GetAllEvaluations()
        {
            return StatusCode((int)HttpStatusCode.OK, await _evaluationRepository.GetAllAsync());
        }

        /// <summary>
        /// Required role program or teacher
        /// </summary>
        [HttpGet("{evaluationId}")]
        [Authorize(Roles = "program,teacher")]
        public async Task<IActionResult> GetByProjectId(int evaluationId)
        {
                return StatusCode((int)HttpStatusCode.OK, _evaluationRepository
                    .Get(x => x.Id.Equals(evaluationId)));
        }

        /// <summary>
        /// Required role program or teacher
        /// </summary>
        [HttpPost("{projectId}")]
        [Authorize(Roles = "program,teacher")]
        public async Task<IActionResult> EvaluateProject(int projectId, [FromBody] EvaluateProjectRequestDto evaluateDto)
        {
            if (projectId == null) return BadRequest("Enter valid project");

            var evaluationStored = await _evaluationRepository.GetAsync(x => x.Assignment.ProjectId.Equals(projectId));
            if(evaluationStored == null) return StatusCode((int)HttpStatusCode.NotFound, "Project not found");
            evaluationStored.StatusId = (int)StatusEnum.Reviewed;
            evaluationStored.Approved = evaluateDto.Approved;
            evaluationStored.Feedback = evaluateDto.FeedBack is not null ? evaluateDto.FeedBack : "";
            evaluationStored.EvaluationDate = DateOnly.FromDateTime(DateTime.Now);

            _evaluationRepository.Update(evaluationStored);

            return StatusCode((int)HttpStatusCode.OK, "Evaluated project succesfully");
        }
    }
}

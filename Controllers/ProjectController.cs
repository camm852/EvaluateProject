using EvaluationProjects.Interfaces;
using EvaluationProjects.Models.Constans;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Entities;
using EvaluationProjects.Models.Enum;
using EvaluationProjects.Persistence;
using EvaluationProjects.Persistence.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EvaluationProjects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {

        private readonly ProjectRepository _projectRepository;
        private readonly IGenericRepository<Assignment> _assignmentRepository;
        private readonly IGenericRepository<Evaluation> _evaluationRepository;
        private readonly IFileManagement _fileManagement;
        private readonly DatabaseContext _context;

        public ProjectController(ProjectRepository projectRepository, IFileManagement fileManagement, DatabaseContext context, IGenericRepository<Assignment> assignmentRepository, IGenericRepository<Evaluation> evaluationRepository)
        {
            _projectRepository = projectRepository;
            _fileManagement = fileManagement;
            _context = context;
            _assignmentRepository = assignmentRepository;
            _evaluationRepository = evaluationRepository;
        }

        /// <summary>
        /// Required role program or teacher
        /// </summary>
        [HttpGet] //api/projects?assigned=true'
        [Authorize(Roles ="program")]
        public async Task<IActionResult> GetProjects([FromQuery] bool? assigned)
        {
            IQueryable<dynamic> query;
            

            if (assigned.HasValue)
            {
                if (assigned.Value)
                {
                    // Traer proyectos asignados
                    query = from project in _context.Projects
                    join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                    join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                    where assignment.TeacherId != null
                    select new
                    {
                        Student = new
                        {
                            StudentName = project.Student.Name,
                            StudentEmail = project.Student.Email,
                        },
                        Project = new
                        {
                            ProjectId = project.Id,
                            ProjectTitle = project.Title,
                            ProjectDescription = project.Description,
                            ProjectFile = project.File,
                            ProjectDeliveryDate = project.DeliveryDate,
                            ProjectStudentId = project.StudentId,
                        },
                        Assignment = assignment == null ? null : new
                        {
                            AssignmentId = assignment.Id,
                            AssignmentProjectId = assignment.ProjectId,
                            AssignmentTeacherId = assignment.TeacherId,
                            AssignmentDate = assignment.AssignmentDate,
                        },
                        Evaluation = evaluation == null ? null : new
                        {
                            evaluation.Id,
                            evaluation.AssignmentId,
                            evaluation.StatusId,
                            evaluation.Approved,
                            evaluation.Feedback,
                            evaluation.EvaluationDate,
                            StatusDescription = evaluation.Status.Description
                        },
                        Teacher = assignment == null ? null : new
                        {
                            assignment.TeacherId,
                            TeacherName = assignment.Teacher.Name,
                            TeacherEmail = assignment.Teacher.Email,
                        },
                        DaysRemaning = assignment.AssignmentDate == null ? null : new { days = Math.Max(0, 10 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - ((DateOnly)(assignment.AssignmentDate)).DayNumber)) }

                    };
                }
                else
                {
                 
                    query = from project in _context.Projects
                    join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                    join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                    where assignment.TeacherId == null
                    select new
                    {
                        Student = new
                        {
                            StudentName = project.Student.Name,
                            StudentEmail = project.Student.Email,
                        },
                        Project = new
                        {
                            ProjectId = project.Id,
                            ProjectTitle = project.Title,
                            ProjectDescription = project.Description,
                            ProjectFile = project.File,
                            ProjectDeliveryDate = project.DeliveryDate,
                            ProjectStudentId = project.StudentId,
                        },
                        Assignment = assignment == null ? null : new
                        {
                            AssignmentId = assignment.Id,
                            AssignmentProjectId = assignment.ProjectId,
                            AssignmentTeacherId = assignment.TeacherId,
                            AssignmentDate = assignment.AssignmentDate,
                        },
                        Evaluation = evaluation == null ? null : new
                        {
                            evaluation.Id,
                            evaluation.AssignmentId,
                            evaluation.StatusId,
                            evaluation.Approved,
                            evaluation.Feedback,
                            evaluation.EvaluationDate,
                            StatusDescription = evaluation.Status.Description
                        },
                        Teacher = assignment == null ? null : new
                        {
                            assignment.TeacherId,
                            TeacherName = assignment.Teacher.Name,
                            TeacherEmail = assignment.Teacher.Email,
                        }
                    };
                }
                var projects = await query.ToListAsync();

                return Ok(projects);
            }
            else
            {
                var projects = await (from project in _context.Projects
                join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                select new
                {
                    Student = new
                    {
                        StudentName = project.Student.Name,
                        StudentEmail = project.Student.Email,
                    },
                    Project = new
                    {
                        ProjectId = project.Id,
                        ProjectTitle = project.Title,
                        ProjectDescription = project.Description,
                        ProjectFile = project.File,
                        ProjectDeliveryDate = project.DeliveryDate,
                        ProjectStudentId = project.StudentId,
                    },
                    Assignment = assignment == null ? null : new
                    {
                        AssignmentId = assignment.Id,
                        AssignmentProjectId = assignment.ProjectId,
                        AssignmentTeacherId = assignment.TeacherId,
                        AssignmentDate = assignment.AssignmentDate,
                    },
                    Evaluation = evaluation == null ? null : new
                    {
                        evaluation.Id,
                        evaluation.AssignmentId,
                        evaluation.StatusId,
                        evaluation.Approved,
                        evaluation.Feedback,
                        evaluation.EvaluationDate,
                        StatusDescription = evaluation.Status.Description
                    },
                    Teacher = assignment == null ? null : new
                    {
                        assignment.TeacherId,
                        TeacherName = assignment.Teacher.Name,
                        TeacherEmail = assignment.Teacher.Email,
                    },
                    DaysRemaning = assignment.AssignmentDate == null ? null : new { days = Math.Max(0, 10 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - ((DateOnly)(assignment.AssignmentDate)).DayNumber)) }
                }).ToListAsync();
                return StatusCode((int)HttpStatusCode.OK, projects);
            }


        }

        /// <summary>
        /// Required authentication
        /// </summary>
        [HttpGet("{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            if (projectId == null) return BadRequest("Enter valid project id");


            var projects = await (from project in _context.Projects
                join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                where project.Id.Equals(projectId)
                select new
                {
                    Student = new
                    {
                        StudentName = project.Student.Name,
                        StudentEmail = project.Student.Email,
                    },
                    Project = new
                    {
                        ProjectId = project.Id,
                        ProjectTitle = project.Title,
                        ProjectDescription = project.Description,
                        ProjectFile = project.File,
                        ProjectDeliveryDate = project.DeliveryDate,
                        ProjectStudentId = project.StudentId,
                    },
                    Assignment = assignment == null ? null : new
                    {
                        AssignmentId = assignment.Id,
                        AssignmentProjectId = assignment.ProjectId,
                        AssignmentTeacherId = assignment.TeacherId,
                        AssignmentDate = assignment.AssignmentDate,
                    },
                    Evaluation = evaluation == null ? null : new
                    {
                        evaluation.Id,
                        evaluation.AssignmentId,
                        evaluation.StatusId,
                        evaluation.Approved,
                        evaluation.Feedback,
                        evaluation.EvaluationDate,
                        StatusDescription = evaluation.Status.Description
                    },
                    Teacher = assignment == null ? null : new
                    {
                        assignment.TeacherId,
                        TeacherName = assignment.Teacher.Name,
                        TeacherEmail = assignment.Teacher.Email,
                    },
                    DaysRemaning = assignment.AssignmentDate == null ? null : new { days = Math.Max(0, 10 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - ((DateOnly)(assignment.AssignmentDate)).DayNumber))}

                }).FirstOrDefaultAsync();

            return StatusCode((int)HttpStatusCode.OK, projects);
        }

        /// <summary>
        /// Required autentication
        /// </summary>
        [HttpGet("student/{studentId}")] //api/projects/1?assigned=true'
        [Authorize]
        public async Task<IActionResult> GetProjectByStudentId(int? studentId, [FromQuery] bool? assigned)
        {
            if (studentId == null) return BadRequest("Enter valid student id");
            if (!assigned.HasValue)
            {
                var projects = await (from project in _context.Projects
                    join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                    join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                    where project.StudentId.Equals(studentId)
                    select new
                    {
                        Student = new
                        {
                            StudentName = project.Student.Name,
                            StudentEmail = project.Student.Email,
                        },
                        Project = new
                        {
                            ProjectId = project.Id,
                            ProjectTitle = project.Title,
                            ProjectDescription = project.Description,
                            ProjectFile = project.File,
                            ProjectDeliveryDate = project.DeliveryDate,
                            ProjectStudentId = project.StudentId,
                        },
                        Assignment = assignment == null ? null : new
                        {
                            AssignmentId = assignment.Id,
                            AssignmentProjectId = assignment.ProjectId,
                            AssignmentTeacherId = assignment.TeacherId,
                            AssignmentDate = assignment.AssignmentDate,
                        },
                        Evaluation = evaluation == null ? null : new
                        {
                            evaluation.Id,
                            evaluation.AssignmentId,
                            evaluation.StatusId,
                            evaluation.Approved,
                            evaluation.Feedback,
                            evaluation.EvaluationDate,
                            StatusDescription = evaluation.Status.Description
                        },
                        Teacher = assignment == null ? null : new
                        {
                            assignment.TeacherId,
                            TeacherName = assignment.Teacher.Name,
                            TeacherEmail = assignment.Teacher.Email,
                        },
                        DaysRemaning = assignment.AssignmentDate == null ? null : new { days = Math.Max(0, 10 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - ((DateOnly)(assignment.AssignmentDate)).DayNumber)) }
                    }).ToListAsync();
                return StatusCode((int)HttpStatusCode.OK, projects);
            }
            else
            {
                IQueryable<dynamic> query;
                if (assigned.Value)
                {
                    // Traer proyectos asignados
                    query = from project in _context.Projects
                        join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                        join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                        where assignment.TeacherId != null && project.StudentId.Equals(studentId)
                        select new
                        {
                            Student = new
                            {
                                StudentName = project.Student.Name,
                                StudentEmail = project.Student.Email,
                            },
                            Project = new
                            {
                                ProjectId = project.Id,
                                ProjectTitle = project.Title,
                                ProjectDescription = project.Description,
                                ProjectFile = project.File,
                                ProjectDeliveryDate = project.DeliveryDate,
                                ProjectStudentId = project.StudentId,
                            },
                            Assignment = assignment == null ? null : new
                            {
                                AssignmentId = assignment.Id,
                                AssignmentProjectId = assignment.ProjectId,
                                AssignmentTeacherId = assignment.TeacherId,
                                AssignmentDate = assignment.AssignmentDate,
                            },
                            Evaluation = evaluation == null ? null : new
                            {
                                evaluation.Id,
                                evaluation.AssignmentId,
                                evaluation.StatusId,
                                evaluation.Approved,
                                evaluation.Feedback,
                                evaluation.EvaluationDate,
                                StatusDescription = evaluation.Status.Description
                            },
                            Teacher = assignment == null ? null : new
                            {
                                assignment.TeacherId,
                                TeacherName = assignment.Teacher.Name,
                                TeacherEmail = assignment.Teacher.Email,
                            },
                            DaysRemaning = assignment.AssignmentDate == null ? null : new { days = Math.Max(0, 10 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - ((DateOnly)(assignment.AssignmentDate)).DayNumber)) }
                        };
                }
                else
                {
                    // Traer proyectos no asignados
                    query = from project in _context.Projects
                        join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                        join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                        where assignment.TeacherId != null && project.StudentId.Equals(studentId)
                        select new
                        {
                            Project = new
                            {
                                ProjectId = project.Id,
                                ProjectTitle = project.Title,
                                ProjectDescription = project.Description,
                                ProjectFile = project.File,
                                ProjectDeliveryDate = project.DeliveryDate,
                                ProjectStudentId = project.StudentId,
                            },
                            Assignment = assignment == null ? null : new
                            {
                                AssignmentId = assignment.Id,
                                AssignmentProjectId = assignment.ProjectId,
                                AssignmentTeacherId = assignment.TeacherId,
                                AssignmentDate = assignment.AssignmentDate,
                            },
                            Evaluation = evaluation == null ? null : new
                            {
                                evaluation.Id,
                                evaluation.AssignmentId,
                                evaluation.StatusId,
                                evaluation.Approved,
                                evaluation.Feedback,
                                evaluation.EvaluationDate,
                                StatusDescription = evaluation.Status.Description
                            },
                            Teacher = assignment == null ? null : new
                            {
                                assignment.TeacherId,
                                TeacherName = assignment.Teacher.Name,
                                TeacherEmail = assignment.Teacher.Email,
                            }
                        };
}
                var projects = await query.ToListAsync();
                return Ok(projects);
            }
        }

        /// <summary>
        /// Required autentication
        /// </summary>
        [HttpGet("teacher/{teacherId}")] //api/projects/1?assigned=true'
        [Authorize]
        public async Task<IActionResult> GetProjectByTeacherId(int? teacherId, [FromQuery] bool? assigned)
        {
            if (teacherId == null) return BadRequest("Enter valid teacher id");
                var projects = await (from project in _context.Projects
                    join assignment in _context.Assignments on project.Assignment.Id equals assignment.Id
                    join evaluation in _context.Evaluations on assignment.Id equals evaluation.AssignmentId
                    where assignment.TeacherId.Equals(teacherId)
                    select new
                    {
                        Student = new
                        {
                            StudentName = project.Student.Name,
                            StudentEmail = project.Student.Email,
                        },
                        Project = new
                        {
                            ProjectId = project.Id,
                            ProjectTitle = project.Title,
                            ProjectDescription = project.Description,
                            ProjectFile = project.File,
                            ProjectDeliveryDate = project.DeliveryDate,
                            ProjectStudentId = project.StudentId,
                        },
                        Assignment = assignment == null ? null : new
                        {
                            AssignmentId = assignment.Id,
                            AssignmentProjectId = assignment.ProjectId,
                            AssignmentTeacherId = assignment.TeacherId,
                            AssignmentDate = assignment.AssignmentDate,
                        },
                        Evaluation = evaluation == null ? null : new
                        {
                            evaluation.Id,
                            evaluation.AssignmentId,
                            evaluation.StatusId,
                            evaluation.Approved,
                            evaluation.Feedback,
                            evaluation.EvaluationDate,
                            StatusDescription = evaluation.Status.Description
                        },
                        Teacher = assignment == null ? null : new
                        {
                            assignment.TeacherId,
                            TeacherName = assignment.Teacher.Name,
                            TeacherEmail = assignment.Teacher.Email,
                        },
                        DaysRemaning = assignment.AssignmentDate == null ? null : new { days = Math.Max(0, 10 - (DateOnly.FromDateTime(DateTime.Now).DayNumber - ((DateOnly)(assignment.AssignmentDate)).DayNumber)) }
                    }).ToListAsync();
            return StatusCode((int)HttpStatusCode.OK, projects);
            
        }

        [HttpGet("resource/{projectId}")]
        public async Task<IActionResult> DownloadPdf(int projectId)
        {
            var projectStored = await _projectRepository.GetAsync(x => x.Id.Equals(projectId));

            if(projectStored is null){
                return NotFound("Project not found");
            }

            byte[] fileBytes = await _fileManagement.GetStreamFile(projectStored.File);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                return null;
            }
            return new FileStreamResult(new MemoryStream(fileBytes), "application/octet-stream")
            {
                FileDownloadName = projectStored.File
            };
        }

        /// <summary>
        /// Required role student
        /// </summary>
        [HttpPost]
        //[Authorize(Roles = "student")]
        public async Task<IActionResult> NewProject([FromForm] ProjectRequestDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("File is not selected or empty.");
            }

            var user = await _context.Users.Where(x => x.Id.Equals(dto.StudentId)).FirstOrDefaultAsync();
            if (user is null) return NotFound("Student not found");
            if (!user.Role.ToLower().Equals(ROLES.STUDENT.ToLower())) return BadRequest("Must be a student");

            string uploadResult = await _fileManagement.UploadFile(dto.File.FileName, dto.File.OpenReadStream());
            if (uploadResult is null)
            {
                return StatusCode(500, "Error uploading file");
            }

            var fileName = uploadResult;

            Project project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                File = fileName!,
                StudentId = dto.StudentId,
                DeliveryDate = DateOnly.FromDateTime(DateTime.Now)
            };


            var projectSaved = _projectRepository.Add(project);

            var assigmentSaved = _assignmentRepository.Add(new Assignment() // Se crea una asignacion por defecto
            {
                ProjectId = projectSaved.Id,
                TeacherId = null,
                AssignmentDate = null
            });

            _evaluationRepository.Add(new Evaluation() //Se crea una evaluacion por defecto
            {
                AssignmentId = assigmentSaved.Id,
                StatusId = (int)StatusEnum.Waiting,
                Approved = null,
                EvaluationDate = null
            });


            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}

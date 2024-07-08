using AutoMapper;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Entities;

namespace EvaluationProjects.Helpers.Mappers
{
    public class ProjectMapper : Profile
    {
        public ProjectMapper() {

            CreateMap<Project, ProjectRequestDto>().ReverseMap();
            
        }
    }
}

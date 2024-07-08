using AutoMapper;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Entities;

namespace EvaluationProjects.Helpers.Mappers
{
    public class AssigmentMapper : Profile
    {

        public AssigmentMapper() {
            CreateMap<Assignment, AssignmentRequestDto>().ReverseMap();

        }


    }
}

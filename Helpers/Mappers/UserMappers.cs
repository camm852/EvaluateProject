using AutoMapper;
using EvaluationProjects.Models.Dtos.Request;
using EvaluationProjects.Models.Dtos.Response;
using EvaluationProjects.Models.Entities;

namespace EvaluationProjects.Helpers.Mappers
{
    public class UserMappers : Profile
    {
        public UserMappers() { 
        
            CreateMap<User, UserRequestDto>().ReverseMap();
            CreateMap<User, LoginResponseDto>().ReverseMap();
        }
    }
}

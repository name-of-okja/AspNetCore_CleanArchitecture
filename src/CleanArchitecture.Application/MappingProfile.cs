using AutoMapper;
using CleanArchitecture.Application.Features.Video.Dtos;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application;
public class MappingProfile : Profile
{
    public MappingProfile()
    {   
        this.CreateMap<Video, VideoDto>();
    }
}

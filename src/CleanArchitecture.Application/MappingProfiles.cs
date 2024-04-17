using AutoMapper;
using CleanArchitecture.Application.Features.Streamers.Dtos;
using CleanArchitecture.Application.Features.Videos.Dtos;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {   
        this.CreateMap<Video, VideoDto>();
        this.CreateMap<CreateStreamerDto, Streamer>();
    }
}

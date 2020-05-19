using AutoMapper;
using DatingApp_Backend.DTOs.Request;
using DatingApp_Backend.DTOs.Response;
using DatingApp_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_Backend.Helpers
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<Users, UsersToListResponse>()
                .ForMember(dest => dest.PhotoURL, opt => opt
                          .MapFrom(src => src.Photos.FirstOrDefault(c => c.IsMain).URL))
                .ForMember(dest => dest.Age, opt => opt
                          .MapFrom(src => src.DateOfBirth.CalculateAge()));


            CreateMap<Users, UserDetailedResponse>()
                .ForMember(dest => dest.PhotoURL, opt => opt
                        .MapFrom(src => src.Photos.FirstOrDefault(c => c.IsMain).URL))
                .ForMember(dest => dest.Age, opt => opt
                         .MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoResponse>();
            CreateMap<UpdateUserDetailRequest, Users>();
        }

    }
}

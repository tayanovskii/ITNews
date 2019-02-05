using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO.UserDto;

namespace ITNews.Mapping
{
    public class MapperUserProfile : Profile
    {
        public MapperUserProfile()
        {

            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(dto => dto.Avatar, opt => opt.MapFrom(profile => profile.Avatar))
                .ForMember(dto => dto.BirthDay, opt => opt.MapFrom(profile => profile.BirthDay))
                .ForMember(dto => dto.City, opt => opt.MapFrom(profile => profile.City))
                .ForMember(dto => dto.Country, opt => opt.MapFrom(profile => profile.Country))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(profile => profile.FirstName))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(profile => profile.LastName))
                .ForMember(dto => dto.Specialization, opt => opt.MapFrom(profile => profile.Specialization))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(profile => profile.UserId))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(profile => profile.User.UserName))
                .ReverseMap();

            CreateMap<ApplicationUser, UserMiniCardDto>()
                .ForMember(dto => dto.Avatar, opt => opt.MapFrom(user => user.UserProfile.Avatar))
                .ForMember(dto => dto.CountLikes, opt => opt.MapFrom(user => user.CommentLikes.Count()))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(user => user.Id))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(user => user.UserName));

        }
    }
}

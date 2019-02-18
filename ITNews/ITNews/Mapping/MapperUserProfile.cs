using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO.AccountDto;
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
                .ForMember(dto => dto.Id, opt => opt.MapFrom(profile => profile.Id))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(profile => profile.User.UserName))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(profile => profile.User.CreatedAt));


            CreateMap<UserProfileDto, UserProfile>()
                .ForMember(profile => profile.Avatar, opt => opt.MapFrom(dto => dto.Avatar))
                .ForMember(profile => profile.BirthDay, opt => opt.MapFrom(dto => dto.BirthDay))
                .ForMember(profile => profile.City, opt => opt.MapFrom(dto => dto.City))
                .ForMember(profile => profile.Country, opt => opt.MapFrom(dto => dto.Country))
                .ForMember(profile => profile.FirstName, opt => opt.MapFrom(dto => dto.FirstName))
                .ForMember(profile => profile.LastName, opt => opt.MapFrom(dto => dto.LastName))
                .ForMember(profile => profile.Specialization, opt => opt.MapFrom(dto => dto.Specialization))
                .ForMember(profile => profile.Id, opt => opt.Ignore());


            CreateMap<ApplicationUser, UserMiniCardDto>()
                .ForMember(dto => dto.Avatar, opt => opt.MapFrom(user => user.UserProfile.Avatar))
                .ForMember(dto => dto.CountLikes, opt => opt.MapFrom(user => user.CommentLikes.Count()))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(user => user.Id))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(user => user.UserName));
               

            CreateMap<RegistrationDto, ApplicationUser>()
                .ForMember(user => user.UserName, opt => opt.MapFrom(dto => dto.UserName))
                .ForMember(user => user.Email, opt => opt.MapFrom(dto => dto.Email))
                .AfterMap((dto, user) =>
                {
                    user.CreatedAt = DateTime.Now;
                    user.ModifiedAt = user.CreatedAt;
                });

            CreateMap<ApplicationUser, ManageUserDto>()
                .ForMember(dto => dto.UserId, opt=>opt.MapFrom(user => user.Id))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(user => user.UserName))
                .ForMember(dto => dto.UserBlocked, opt => opt.MapFrom(user => user.UserBlocked))
                .ForMember(dto => dto.CountLikes, opt => opt.MapFrom(user => user.CommentLikes.Count()))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(user => user.CreatedAt))
                .ForAllOtherMembers(opt=>opt.Ignore());

        }
    }
}

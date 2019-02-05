using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO.Rating;

namespace ITNews.Mapping
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<CreateRatingDto, Rating>()
                .ForMember(rating => rating.NewsId, opt=>opt.MapFrom(dto => dto.NewsId))
                .ForMember(rating => rating.Value, opt => opt.MapFrom(dto => dto.Value))
                .ForMember(rating => rating.UserId, opt => opt.MapFrom(dto => dto.UserId))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Rating, RatingDto>()
                .ForMember(dto => dto.Value, opt => opt.MapFrom(rating => rating.Value))
                .ForMember(dto => dto.NewsId, opt => opt.MapFrom(rating => rating.NewsId))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(rating => rating.UserId))
                .ReverseMap();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO;

namespace ITNews.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Tag, TagDto>().ForMember(dto => dto.Name, opt => opt.MapFrom(tag => tag.Name)).ReverseMap();

            CreateMap<News, NewsCardDto>()
                .ForMember(dto => dto.Description, opt => opt.MapFrom(news => news.Description))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(news => news.CreatedAt))
                .ForMember(dto => dto.ModifiedAt, opt => opt.MapFrom(news => news.ModifiedAt))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(news => news.Title))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(news => news.Description))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(news => news.UserId))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(news => news.User.UserName))
                .ForMember(dto => dto.VisitorCount, opt => opt.MapFrom(news => news.VisitorCount));

            CreateMap<News, CreateNewsDto>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(news => news.Content))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(news => news.Description))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(news => news.Title))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(news => news.UserId))
                .ForMember(dto => dto.Tags, opt => opt.MapFrom(news => news.NewsTags))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(news => news.NewsTags))
                .ReverseMap();

            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(category => category.Name)).ReverseMap();


        }
    }
}

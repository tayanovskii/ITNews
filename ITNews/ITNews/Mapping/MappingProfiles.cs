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
                .ForMember(dto => dto.Description, opt => opt.MapFrom(news => news.Description));
            //todo mapping news

            //CreateMap<News, CreateNewsDto>()
            //    .ForMember(model => model.Id, opts => opts.MapFrom(news => news.Id))
            //    .ForMember(model => model.Content, opts => opts.MapFrom(news => news.Content))
            //    .ForMember(model => model.Description, opts => opts.MapFrom(news => news.Description))
            //    .ForMember(model => model.Title, opts => opts.MapFrom(news => news.Title))
            //    .ForMember(model => model.VisitorCount, opts => opts.MapFrom(news => news.VisitorCount))
            //    .ForMember(model => model.UserId, opts => opts.MapFrom(news => news.UserId));

            //CreateMap<Person, PersonDTO>()
            //    .ForMember(dest => dest.City,
            //        opts => opts.MapFrom(
            //            src => src.Address.City
            //        )).ReverseMap();
        }
    }
}

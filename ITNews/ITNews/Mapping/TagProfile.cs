using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.Tag;

namespace ITNews.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>().ForMember(dto => dto.Name, opt => opt.MapFrom(tag => tag.Name)).ReverseMap();
            CreateMap<Tag, TagsCloud>()
                .ForMember(cloud => cloud.Id, opt=>opt.MapFrom(tag => tag.Id))
                .ForMember(cloud => cloud.Name, opt=> opt.MapFrom(tag => tag.Name))
                .ForAllOtherMembers(opt=>opt.Ignore());
        }
    }
}

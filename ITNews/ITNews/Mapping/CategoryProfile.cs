using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO;

namespace ITNews.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(category => category.Name)).ReverseMap();
        }
    }
}

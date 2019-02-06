using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.CommentLikeDto;

namespace ITNews.Mapping
{
    public class CommentLikeProfile : Profile
    {
        public CommentLikeProfile()
        {
            CreateMap<CommentLike, CreateCommentLikeDto>()
                .ForMember(dto => dto.CommentId, opt => opt.MapFrom(like => like.CommentId))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(like => like.UserId))
                .ReverseMap()
                .ForAllOtherMembers(opt=>opt.Ignore());
        }
    }
}

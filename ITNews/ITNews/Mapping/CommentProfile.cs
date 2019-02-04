using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO.CommentDto;

namespace ITNews.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(comment => comment.Content))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(comment => comment.CreatedAt))
                .ForMember(dto => dto.ModifiedAt, opt => opt.MapFrom(comment => comment.ModifiedAt))
                .ForMember(dto => dto.ModifiedBy, opt => opt.MapFrom(comment => comment.ModifiedBy))
                .ForMember(dto => dto.NewsId, opt => opt.MapFrom(comment => comment.NewsId))
                .ForMember(dto => dto.UserCard, opt => opt.MapFrom(comment => comment.User))
                .ForMember(dto => dto.CountLikes, opt => opt.Ignore())
                .AfterMap((comment, dto) =>
                {
                    if (comment.Likes != null && comment.Likes.Any())
                    {
                        dto.CountLikes = comment.Likes.Count();
                    }
                });

            CreateMap<CreateCommentDto, Comment>()
                .ForMember(comment => comment.Content, opt => opt.MapFrom(dto => dto.Content))
                .ForMember(comment => comment.NewsId, opt => opt.MapFrom(dto => dto.NewsId))
                .ForMember(comment => comment.UserId, opt => opt.MapFrom(dto => dto.UserId));


            CreateMap<EditCommentDto, Comment>()
                .ForMember(comment => comment.Content, opt => opt.MapFrom(dto => dto.Content));
        }
    }
}

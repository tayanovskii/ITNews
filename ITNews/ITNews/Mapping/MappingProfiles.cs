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
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(news => news.UserId))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(news => news.User.UserName))
                .ForMember(dto => dto.VisitorCount, opt => opt.MapFrom(news => news.VisitorCount))
                .ForMember(dto => dto.CommentCount, opt => opt.MapFrom(news => news.Comments.Count()))
                .ForMember(dto => dto.Rating, opt => opt.Ignore())
                .AfterMap((news, dto) =>
                {
                    if (news.Ratings != null && news.Ratings.Any())
                    {
                        dto.Rating = news.Ratings.Average(rating => rating.Value);
                    }
                });

            CreateMap<News, FullNewsDto>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(news => news.Content))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(news => news.CreatedAt))
                .ForMember(dto => dto.ModifiedAt, opt => opt.MapFrom(news => news.ModifiedAt))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(news => news.Title))
                .ForMember(dto => dto.VisitorCount, opt => opt.MapFrom(news => news.VisitorCount))
                .ForMember(dto => dto.UserMiniCardDto, opt => opt.MapFrom(news => news.User))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(news => news.Comments))
                .ForMember(dto => dto.MarkDown, opt => opt.MapFrom(news => news.MarkDown))
                .ForMember(dto => dto.Categories, opt => opt.Ignore())
                .ForMember(dto => dto.Tags, opt => opt.Ignore())
                .ForMember(dto => dto.Rating, opt => opt.Ignore())
                .AfterMap((news, dto) =>
                {
                    dto.Tags = news.NewsTags.Select(tag => new TagDto(){Id = tag.TagId, Name = tag.Tag.Name});
                    dto.Categories = news.NewsCategories.Select(category => new CategoryDto(){Id = category.CategoryId, Name = category.Category.Name});
                    if (news.Ratings!=null && news.Ratings.Any())
                    {
                        dto.Rating = news.Ratings.Average(rating => rating.Value);
                    }
                });

      
            CreateMap<ApplicationUser, UserMiniCardDto>()
                .ForMember(dto => dto.Avatar, opt => opt.MapFrom(user => user.UserProfile.Avatar))
                .ForMember(dto => dto.CountLikes, opt => opt.MapFrom(user => user.CommentLikes.Count()))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(user => user.Id))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(user => user.UserName));

            CreateMap<Comment, CommentDto>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(comment => comment.Content))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(comment => comment.CreatedAt))
                .ForMember(dto => dto.ModifiedAt, opt => opt.MapFrom(comment => comment.ModifiedAt))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(comment => comment.UserId))
                .ForMember(dto => dto.ModifiedBy, opt => opt.MapFrom(comment => comment.ModifiedBy))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(comment => comment.User.UserName))
                .ForMember(dto => dto.CountLike, opt => opt.Ignore())
                .AfterMap((comment, dto) =>
                {
                    if (comment.Likes!=null && comment.Likes.Any())
                    {
                        dto.CountLike = comment.Likes.Count();
                    }
                })
                .ReverseMap();
                


            CreateMap<News, EditNewsDto>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(news => news.Content))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(news => news.Description))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(news => news.Title))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(news => news.UserId))
                .ForMember(dto => dto.MarkDown, opt => opt.MapFrom(news => news.MarkDown))
                .ForMember(dto => dto.Tags, opt => opt.Ignore())
                .ForMember(dto => dto.Categories, opt => opt.Ignore())
                .AfterMap((news, dto) =>
                    {
                        dto.Tags = news.NewsTags.Select(tag => new TagDto {Id = tag.TagId, Name = tag.Tag.Name});
                        dto.Categories = news.NewsCategories.Select(category => new CategoryDto
                            {Id = category.CategoryId, Name = category.Category.Name});
                    });
            
            CreateMap<EditNewsDto,News>()
                .ForMember(news => news.Content, opt=>opt.MapFrom(dto => dto.Content))
                .ForMember(news => news.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(news => news.MarkDown, opt => opt.MapFrom(dto => dto.MarkDown))
                .ForMember(news => news.Title, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(news => news.ModifiedBy, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(news => news.NewsTags, opt => opt.Ignore())
                .ForMember(news => news.NewsCategories, opt => opt.Ignore())
                .AfterMap((dto, news) =>
                {
                    var removedCategories = news.NewsCategories.Where(newsCategory => !dto.Categories.Contains(new CategoryDto(){Id = newsCategory.CategoryId}));
                    news.NewsCategories = news.NewsCategories.Except(removedCategories);

                    //todo added categories
                    dto.Categories.


                    var removedTags = news.NewsTags.Where(newsTag => !dto.Tags.Contains(new TagDto() { Id = newsTag.TagId }));
                    var newsListNewsTags = news.NewsTags.Except(removedTags);
                    news.NewsTags = newsListNewsTags;
                })
                .ForAllOtherMembers(expression => expression.Ignore());
                
                CreateMap<CreateNewsDto, News>()
                .ForMember(news => news.Content, opt => opt.MapFrom(dto => dto.Content))
                .ForMember(news => news.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(news => news.MarkDown, opt => opt.MapFrom(dto => dto.MarkDown))
                .ForMember(news => news.Title, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(news => news.UserId, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(news => news.NewsTags, opt => opt.Ignore())
                .ForMember(news => news.NewsCategories, opt => opt.Ignore())
                .ForMember(news => news.Ratings, opt => opt.Ignore())
                .ForMember(news => news.Comments, opt => opt.Ignore())
                .AfterMap((dto, news) =>
                    {
                        news.NewsCategories = dto.Categories.Select(newDtoCategory => new NewsCategory() {CategoryId = newDtoCategory.Id}).ToList();
                    });


            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(category => category.Name)).ReverseMap();

            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(dto => dto.Avatar, opt => opt.MapFrom(profile => profile.Avatar))
                .ForMember(dto => dto.BirthDay, opt => opt.MapFrom(profile => profile.BirthDay))
                .ForMember(dto => dto.City, opt => opt.MapFrom(profile => profile.City))
                .ForMember(dto => dto.Country, opt => opt.MapFrom(profile => profile.Country))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(profile => profile.FirstName))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(profile => profile.LastName))
                .ForMember(dto => dto.Specialization, opt => opt.MapFrom(profile => profile.Specialization))
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(profile => profile.UserId))
                .ReverseMap();


        }
    }
}

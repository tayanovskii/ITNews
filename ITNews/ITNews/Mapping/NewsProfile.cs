using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO;
using ITNews.DTO.NewsDto;

namespace ITNews.Mapping
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsCardDto>()
               .ForMember(dto => dto.Description, opt => opt.MapFrom(news => news.Description))
               .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(news => news.CreatedAt))
               .ForMember(dto => dto.ModifiedAt, opt => opt.MapFrom(news => news.ModifiedAt))
               .ForMember(dto => dto.Title, opt => opt.MapFrom(news => news.Title))
               .ForMember(dto => dto.User, opt => opt.MapFrom(news => news.User))
               .ForMember(dto => dto.NewsStatistic, opt => opt.MapFrom(news => news))
               .ForMember(dto => dto.Categories, opt => opt.Ignore())
               .ForMember(dto => dto.Tags, opt => opt.Ignore())
               .AfterMap((news, dto) =>
               {
                   dto.Categories = news.NewsCategories.Select(category => new CategoryDto()
                   { Id = category.CategoryId, Name = category.Category.Name });
                   dto.Tags = news.NewsTags.Select(tag => new TagDto() { Id = tag.TagId, Name = tag.Tag.Name });
               });

            CreateMap<News, NewsStatisticDto>()
                .ForMember(dto => dto.VisitorCount, opt => opt.MapFrom(news => news.VisitorCount))
                .ForMember(dto => dto.CommentCount, opt => opt.Ignore())
                .ForMember(dto => dto.Rating, opt => opt.Ignore())
                .ForMember(dto => dto.RatingCount, opt => opt.Ignore())
                .AfterMap((news, dto) =>
                {
                    if (news.Ratings != null && news.Ratings.Any())
                    {
                        dto.Rating = news.Ratings.Average(rating => rating.Value);
                        dto.RatingCount = news.Ratings.Count();
                    }

                    if (news.Comments != null && news.Comments.Any())
                    {
                        dto.CommentCount = news.Comments.Count();
                    }

                });

            CreateMap<News, FullNewsDto>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(news => news.Content))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(news => news.CreatedAt))
                .ForMember(dto => dto.ModifiedAt, opt => opt.MapFrom(news => news.ModifiedAt))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(news => news.Title))
                .ForMember(dto => dto.UserMiniCardDto, opt => opt.MapFrom(news => news.User))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(news => news.Comments))
                .ForMember(dto => dto.MarkDown, opt => opt.MapFrom(news => news.MarkDown))
                .ForMember(dto => dto.NewsStatistic, opt => opt.MapFrom(news => news))
                .ForMember(dto => dto.Categories, opt => opt.Ignore())
                .ForMember(dto => dto.Tags, opt => opt.Ignore())
                .AfterMap((news, dto) =>
                {
                    dto.Tags = news.NewsTags.Select(tag => new TagDto() { Id = tag.TagId, Name = tag.Tag.Name });
                    dto.Categories = news.NewsCategories.Select(category => new CategoryDto() { Id = category.CategoryId, Name = category.Category.Name });
                });

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
                    dto.Tags = news.NewsTags.Select(tag => new TagDto { Id = tag.TagId, Name = tag.Tag.Name });
                    dto.Categories = news.NewsCategories.Select(category => new CategoryDto
                    { Id = category.CategoryId, Name = category.Category.Name });
                });

            CreateMap<EditNewsDto, News>()
                .ForMember(news => news.Content, opt => opt.MapFrom(dto => dto.Content))
                .ForMember(news => news.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(news => news.MarkDown, opt => opt.MapFrom(dto => dto.MarkDown))
                .ForMember(news => news.Title, opt => opt.MapFrom(dto => dto.Title))
                .ForMember(news => news.ModifiedBy, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(news => news.NewsTags, opt => opt.Ignore())
                .ForMember(news => news.NewsCategories, opt => opt.Ignore())
                .AfterMap((dto, news) =>
                {
                    var removedCategories = news.NewsCategories.Where(newsCategory => !dto.Categories.Contains(new CategoryDto() { Id = newsCategory.CategoryId }));
                    news.NewsCategories = news.NewsCategories.Except(removedCategories);

                    var addedCategories = dto.Categories.Where(categoryDto => news.NewsCategories.All(category => category.CategoryId != categoryDto.Id)).Select(categoryDto => new NewsCategory() { CategoryId = categoryDto.Id });
                    var listNewsCategories = news.NewsCategories.ToList();
                    foreach (var addedCategory in addedCategories)
                    {
                        listNewsCategories.Add(addedCategory);
                    }

                    news.NewsCategories = listNewsCategories;

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
                news.NewsCategories = dto.Categories.Select(newDtoCategory => new NewsCategory() { CategoryId = newDtoCategory.Id }).ToList();
            });
        }
    }
}

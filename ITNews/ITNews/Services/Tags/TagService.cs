using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.DTO;
using Microsoft.EntityFrameworkCore;

namespace ITNews.Services.Tags
{

    public class TagService:ITagService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TagService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> AddTags(IEnumerable<TagDto> newTagDto)
        {
            var newTagsDto = new List<TagDto>();
            var resultTags = new List<Tag>();
            foreach (var tagDto in newTagDto)
            {
                var tagByName = GetTagByName(tagDto.Name).Result;
                if (tagByName != null)
                {
                    resultTags.Add(tagByName);
                    continue;
                }
                newTagsDto.Add(tagDto);
            }

            var newTags = mapper.Map<IEnumerable<TagDto>, IEnumerable<Tag>>(newTagsDto);
            foreach (var newTag in newTags)
            {
                newTag.CreatedAt = DateTime.Now;
                newTag.ModifiedAt = newTag.CreatedAt;
            }

            await context.Tags.AddRangeAsync(newTags);
            await context.SaveChangesAsync();
            resultTags.AddRange(newTags);
            var resultTagsDto = mapper.Map<IEnumerable<Tag>,IEnumerable<TagDto>>(resultTags);
            return resultTagsDto;
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await context.Tags.FirstOrDefaultAsync(tag => tag.Name == name);
        }
    }
}

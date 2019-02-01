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
            var newTagsDtoToSaveDataBase = new List<TagDto>();

            //input TagID = 0 denote that it`s a new tag and we need to add it to database
            foreach (var tagDto in newTagDto)
            {
                if (tagDto.Id == 0)
                {
                    var tagByName = GetTagByName(tagDto.Name).Result;
                    if (tagByName != null)
                    {
                        newTagsDto.Add(new TagDto(){Id = tagByName.Id, Name = tagByName.Name});
                        continue;
                    }
                    newTagsDtoToSaveDataBase.Add(tagDto);
                    continue;
                }
                newTagsDto.Add(tagDto);
            }

            var newTags = mapper.Map<IEnumerable<TagDto>, IEnumerable<Tag>>(newTagsDtoToSaveDataBase);
            foreach (var newTag in newTags)
            {
                newTag.CreatedAt = DateTime.Now;
                newTag.ModifiedAt = newTag.CreatedAt;
            }

            await context.Tags.AddRangeAsync(newTags);
            await context.SaveChangesAsync();
            var newTagsSavedInDataBase= mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(newTags);
            newTagsDto.AddRange(newTagsSavedInDataBase);
            return newTagsDto;
        }

        public async Task<Tag> GetTagByName(string name)
        {
            var findTag = await context.Tags.FirstOrDefaultAsync(tag => tag.Name == name);
            return findTag;

        }
    }
}

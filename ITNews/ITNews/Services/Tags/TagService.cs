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
                var tagByName = await GetTagByName(tagDto.Name);
                if (tagDto.Id == 0)
                {
                    var checkedTag = tagByName;
                    if (checkedTag != null)
                    {
                        newTagsDto.Add(new TagDto(){Id = checkedTag.Id, Name = checkedTag.Name});
                        continue;
                    }
                    newTagsDtoToSaveDataBase.Add(tagDto);
                    continue;
                }

                var tagByNameDto = mapper.Map<Tag,TagDto>(tagByName);
                newTagsDto.Add(tagByNameDto);
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
            var findTag = await context.Tags.SingleOrDefaultAsync(tag => tag.Name == name);
            return findTag;
        }
    }
}

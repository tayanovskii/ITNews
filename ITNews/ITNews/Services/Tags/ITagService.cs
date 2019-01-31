using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data.Entities;
using ITNews.DTO;

namespace ITNews.Services.Tags
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> AddTags(IEnumerable<TagDto> newTagDto);
        Task<Tag> GetTagByName(string name);
    }
}

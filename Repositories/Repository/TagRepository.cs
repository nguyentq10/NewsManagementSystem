using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Context;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class TagRepository : GenericRepository<Tag>
    {
        public TagRepository(NewsManagementDBContext context) : base(context)
        {
        }
        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {
            var normalizedName = tagName.Trim().ToLower();
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagName.ToLower() == normalizedName);

            if (existingTag != null)
            {
                return existingTag;
            }
            var newTag = new Tag { TagName = tagName, Note = "" }; 
            _context.Tags.Add(newTag);
           
            return newTag;
        }

    }
}

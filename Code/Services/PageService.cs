﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.Areas.Front.ViewModels;
using Bonsai.Data;
using Bonsai.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bonsai.Code.Services
{
    /// <summary>
    /// Page management service.
    /// </summary>
    public class PageService
    {
        public PageService(AppDbContext db, MarkdownService markdown)
        {
            _db = db;
            _markdown = markdown;
        }

        private readonly AppDbContext _db;
        private readonly MarkdownService _markdown;

        #region Public methods

        /// <summary>
        /// Returns the description VM for a page.
        /// </summary>
        public async Task<PageDescriptionVM> GetPageDescriptionAsync(string url)
        {
            var page = await _db.Pages
                                .AsNoTracking()
                                .Include(p => p.Relations)
                                .ThenInclude(r => r.Object)
                                .FirstOrDefaultAsync(x => x.Url == url);

            if(page == null)
                throw new KeyNotFoundException();

            // todo: main block
            return new PageDescriptionVM
            {
                Title = page.Title,
                Url = page.Url,
                Description = _markdown.Compile(page.Description)
            };
        }

        /// <summary>
        /// Returns the list of media files.
        /// </summary>
        public async Task<PageMediaVM> GetPageMediaAsync(string url)
        {
            var page = await _db.Pages
                                .AsNoTracking()
                                .Include(p => p.MediaTags)
                                .ThenInclude(t => t.Media)
                                .FirstOrDefaultAsync(x => x.Url == url);

            if (page == null)
                throw new KeyNotFoundException();

            var list = new List<MediaThumbnailVM>();
            foreach (var tag in page.MediaTags)
            {
                list.Add(new MediaThumbnailVM
                {
                    Type = tag.Media.Type,
                    MediaKey = tag.Media.Key,
                    // todo: year
                });
            }

            return new PageMediaVM
            {
                Title = page.Title,
                Url = page.Url,
                Media = list
            };
        }

        /// <summary>
        /// Returns the list of fact groups.
        /// </summary>
        public async Task<PageFactsVM> GetPageFactsAsync(string url)
        {
            var page = await _db.Pages
                                .AsNoTracking()
                                .Include(p => p.Relations)
                                .ThenInclude(t => t.Object)
                                .FirstOrDefaultAsync(x => x.Url == url);

            if (page == null)
                throw new KeyNotFoundException();

            var groups = GetPersonalFacts(page)
                .Concat(GetRelationFacts(page))
                .ToList();

            return new PageFactsVM
            {
                Title = page.Title,
                Url = page.Url,
                FactGroups = groups
            };
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Returns the list of fact groups based converted from relations.
        /// </summary>
        private IEnumerable<FactGroupVM> GetRelationFacts(Page page)
        {
            // todo
            yield break;
        }

        /// <summary>
        /// Returns the list of personal facts for a page.
        /// </summary>
        private IEnumerable<FactGroupVM> GetPersonalFacts(Page page)
        {
            // todo
            yield break;
        }

        #endregion
    }
}

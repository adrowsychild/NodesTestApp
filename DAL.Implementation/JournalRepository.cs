using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dto;

namespace DAL.Implementation
{
    public class JournalRepository : IJournalRepository
    {
        private readonly DataDbContext context;

        public JournalRepository(DataDbContext context)
        {
            this.context = context;
        }

        public async Task AddJournalItemAsync(JournalItem item)
        {
            context.JournalItems.Add(item);
            await context.SaveChangesAsync();
        }

        public Task<JournalItem> GetJournalItemAsync(int id)
        {
            return context.JournalItems.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<JournalItem>> GetJournalItemsAsync()
        {
            return await context.JournalItems.ToListAsync();
        }

        public async Task<ICollection<JournalItem>> GetJournalItemsAsync(JournalFilter filter)
        {
            var items = await context.JournalItems.ToListAsync();
            if (!items.Any())
            {
                return items;
            }

            IEnumerable<JournalItem> tmp = items;

            if (filter.Skip != null && filter.Skip > 0)
            {
                tmp = tmp.Skip(filter.Skip.Value);
            }

            if (filter.Take != null && filter.Take > 0)
            {
                tmp = tmp.Take(filter.Take.Value);
            }

            if (filter.BodyFilter != null)
            {
                if (filter.BodyFilter.From != null)
                {
                    tmp = tmp.Where(x => x.Timestamp >= filter.BodyFilter.From);
                }

                if (filter.BodyFilter.To != null)
                {
                    tmp = tmp.Where(x => x.Timestamp <= filter.BodyFilter.To);
                }

                if (!string.IsNullOrEmpty(filter.BodyFilter.Search))
                {
                    tmp = tmp.Where(x => ContainsSubstring(x, filter.BodyFilter.Search));
                }
            }

            return tmp.ToList();
        }

        private bool ContainsSubstring(JournalItem item, string searchString)
        {
            if (string.IsNullOrEmpty(item.ErrorMessage))
            {
                return false;
            }

            var result = item.ErrorMessage.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
            if (!result)
            {
                if (string.IsNullOrEmpty(item.StackTrace))
                {
                    return false;
                }

                result = item.StackTrace.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!result)
            {
                if (string.IsNullOrEmpty(item.QueryParameters))
                {
                    return false;
                }

                result = item.QueryParameters.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!result)
            {
                if (string.IsNullOrEmpty(item.BodyParameters))
                {
                    return false;
                }

                result = item.BodyParameters.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }
    }
}

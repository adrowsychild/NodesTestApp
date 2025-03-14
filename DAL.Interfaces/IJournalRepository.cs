using Models;
using Models.Dto;

namespace DAL.Interfaces
{
    public interface IJournalRepository
    {
        Task<ICollection<JournalItem>> GetJournalItemsAsync();

        Task<ICollection<JournalItem>> GetJournalItemsAsync(JournalFilter filter);

        Task AddJournalItemAsync(JournalItem item);

        Task<JournalItem> GetJournalItemAsync(int id);
    }
}

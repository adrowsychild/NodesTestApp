using Models;
using Models.Dto;

namespace Services.Interfaces
{
    public interface IJournalService
    {
        Task<ICollection<JournalItem>> GetJournalItemsAsync();
        Task<JournalItem> GetJournalItemAsync(int id);
        Task<ICollection<JournalItem>> GetJournalItemsAsync(JournalFilter filter);
    }
}

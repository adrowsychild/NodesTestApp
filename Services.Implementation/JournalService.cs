using DAL.Interfaces;
using Models;
using Models.Dto;
using Services.Interfaces;

namespace Services.Implementation
{
    public class JournalService : IJournalService
    {
        protected readonly IJournalRepository journalRepository;

        public JournalService(IJournalRepository journalRepository)
        {
            this.journalRepository = journalRepository;
        }

        public Task<ICollection<JournalItem>> GetJournalItemsAsync()
        {
            return journalRepository.GetJournalItemsAsync();
        }

        public Task<JournalItem> GetJournalItemAsync(int id)
        {
            return journalRepository.GetJournalItemAsync(id);
        }

        public Task<ICollection<JournalItem>> GetJournalItemsAsync(JournalFilter filter)
        {
            return journalRepository.GetJournalItemsAsync(filter);
        }
    }
}

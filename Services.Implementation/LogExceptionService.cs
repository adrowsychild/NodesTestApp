using DAL.Interfaces;
using Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class LogExceptionService : ILogExceptionService
    {
        protected readonly IJournalRepository journalRepository;

        public LogExceptionService(IJournalRepository repository)
        {
            this.journalRepository = repository;
        }

        public Task AddJournalItemAsync(JournalItem item)
        {
            return journalRepository.AddJournalItemAsync(item);
        }
    }
}

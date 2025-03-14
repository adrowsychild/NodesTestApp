using Models;

namespace Services.Interfaces
{
    public interface ILogExceptionService
    {
        Task AddJournalItemAsync(JournalItem item);
    }
}

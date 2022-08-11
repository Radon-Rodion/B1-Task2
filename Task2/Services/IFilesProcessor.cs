using Task2.Data;
using Task2.Models;

namespace Task2.Services
{
    public interface IFilesProcessor
    {
        public Task LoadFileToDbAsync(string fileName, ApplicationDbContext context);
        public Task<LoadedList[]> GetLoadedFilesAsync(ApplicationDbContext context);
        public Task<TotalInfo> GetTotalFileInfoAsync(int id, ApplicationDbContext context);
        public Task<string[][]> GetFileAsTableAsync(int id, ApplicationDbContext context);
        public Task SaveDbToFileAsync(int loadedListId, string FileName, ApplicationDbContext context);
    }
}

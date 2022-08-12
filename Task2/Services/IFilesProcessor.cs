using Task2.Data;
using Task2.Models;

namespace Task2.Services
{
    public interface IFilesProcessor
    {
        /// <summary>
        /// Reads excel file and saves info from it into database
        /// </summary>
        public Task LoadFileToDbAsync(string fileName, ApplicationDbContext context);

        /// <summary>
        /// Returns liat of loaded excel files
        /// </summary>
        public Task<LoadedList[]> GetLoadedFilesAsync(ApplicationDbContext context);

        /// <summary>
        /// Returns info from single file loaded into database
        /// as a single balance object aggregating all other balances
        /// </summary>
        public Task<TotalInfo> GetTotalFileInfoAsync(int id, ApplicationDbContext context);

        /// <summary>
        /// Returns info from single file loaded into database
        /// as a spreadsheet (similar to one used in excel)
        /// </summary>
        public Task<string[][]> GetFileAsTableAsync(int id, ApplicationDbContext context);

        /// <summary>
        /// Runs info from database and saves it into excel file
        /// </summary>
        public Task SaveDbToFileAsync(int loadedListId, string FileName, ApplicationDbContext context);
    }
}

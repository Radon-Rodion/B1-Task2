using ExcelLibrary;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Task2.Data;
using Task2.Models;
using static System.Convert;

namespace Task2.Services
{
    public class ExelFileProcessor : IFilesProcessor
    {
        public async Task<LoadedList[]> GetLoadedFilesAsync(ApplicationDbContext context)
        {
            return await context.LoadedLists.ToArrayAsync();
        }

        private List<AccountInfo> GetAccountInfos(int loadedListId, int accountGroupId, ApplicationDbContext context)
        {
            List<AccountInfo> result = new List<AccountInfo>();

            foreach(var accountBalanceInfo in context.AccountsBalances.Where(bal => bal.AccountGroupId == accountGroupId && bal.LoadedListId == loadedListId))
            {
                var accountInfo = new AccountInfo()
                {
                    FullId = accountBalanceInfo.AccountId,
                    IncomingActive = accountBalanceInfo.IncomingActive,
                    IncomingPassive = accountBalanceInfo.IncomingPassive,
                    Debit = accountBalanceInfo.Debit,
                    Credit = accountBalanceInfo.Credit,
                    OutgoingActive = accountBalanceInfo.OutgoingActive,
                    OutgoingPassive = accountBalanceInfo.OutgoingPassive
                };
                result.Add(accountInfo);
            }
            return result;
        }

        private List<AccountGroupInfo> GetAccountGroupsInfos(int loadedListId, int operationClassId, ApplicationDbContext context)
        {
            List<AccountGroupInfo> result = new List<AccountGroupInfo>();

            foreach(var accountGroupInfo in context.AccountGroups.Where(group => group.OperationsClassId == operationClassId).Join(
                context.AccountGroupsBalances.Where(balance => balance.LoadedListId == loadedListId), 
                gr => gr.Id, bal => bal.AccountGroupId,
                (gr, bal) => new { 
                    Id = gr.Id, 
                    OperationClass = gr.OperationsClassId,
                    LoadedList = bal.LoadedListId,
                    IncomingActive = bal.IncomingActive,
                    IncomingPassive = bal.IncomingPassive,
                    Debit = bal.Debit,
                    Credit = bal.Credit,
                    OutgoingActive = bal.OutgoingActive,
                    OutgoingPassive = bal.OutgoingPassive
                }))
            {
                var info = new AccountGroupInfo()
                {
                    Id = accountGroupInfo.Id,
                    IncomingActive = accountGroupInfo.IncomingActive,
                    IncomingPassive = accountGroupInfo.IncomingPassive,
                    Debit = accountGroupInfo.Debit,
                    Credit = accountGroupInfo.Credit,
                    OutgoingActive = accountGroupInfo.OutgoingActive,
                    OutgoingPassive = accountGroupInfo.OutgoingPassive
                };
                info.AccountInfos = GetAccountInfos(loadedListId, info.Id, context).ToArray();
                result.Add(info);
            }

            return result;
        }

        private List<OperationsClassInfo> GetOperationClassesInfos(int loadedListId, ApplicationDbContext context)
        {
            List<OperationsClassInfo> result = new List<OperationsClassInfo>();

            foreach (var operationClassBalance in context.OperationsClassesBalances.Where(balance => balance.LoadedListId == loadedListId))
            {
                var operationClass = context.OperationsClasses.Where(operationClass => operationClass.Id == operationClassBalance.OperationsClassId).First();
                var operationClassInfo = new OperationsClassInfo()
                {
                    Name = operationClass.Name,
                    IncomingActive = operationClassBalance.IncomingActive,
                    IncomingPassive = operationClassBalance.IncomingPassive,
                    Debit = operationClassBalance.Debit,
                    Credit = operationClassBalance.Credit,
                    OutgoingActive = operationClassBalance.OutgoingActive,
                    OutgoingPassive = operationClassBalance.OutgoingPassive
                };
                operationClassInfo.AccountGroupInfos = GetAccountGroupsInfos(loadedListId, operationClass.Id, context).ToArray();
                result.Add(operationClassInfo);
            }

            return result;
        }
        public async Task<TotalInfo> GetTotalFileInfoAsync(int id, ApplicationDbContext context)
        {
            return await new Task<TotalInfo>(() =>
            {
                LoadedList listInfo = context.LoadedLists.Where(list => list.Id == id).First();
                FullListBalance balance = context.FullListsBalances.Where(listBalance => listBalance.LoadedListId == id).First();

                TotalInfo totalInfo = new TotalInfo()
                {
                    Name = listInfo.Name,
                    IncomingActive = balance.IncomingActive,
                    IncomingPassive = balance.IncomingPassive,
                    Debit = balance.Debit,
                    Credit = balance.Credit,
                    OutgoingActive = balance.OutgoingActive,
                    OutgoingPassive = balance.OutgoingPassive
                };

                totalInfo.OperationClassInfos = GetOperationClassesInfos(id, context).ToArray();
                return totalInfo;
            });
        }

        private void InsertAccountsInfo(List<string[]> result, int listId, int accountGroupId, ApplicationDbContext context)
        {
            foreach (var accountBalance in context.AccountsBalances.Where(bal => bal.AccountGroupId == accountGroupId && bal.LoadedListId == listId))
            {
                var singleRowArray = new string[7];
                singleRowArray[0] = accountBalance.AccountId.ToString();
                FillBalanceRow(accountBalance, ref singleRowArray);
                result.Add(singleRowArray);
            }
        }
        
        private void InsertAccountGroupsInfo(List<string[]> result, int listId, int operationsClassId, ApplicationDbContext context)
        {
            foreach (var accountGroupInfo in context.AccountGroups.Where(group => group.OperationsClassId == operationsClassId).Join(
                context.AccountGroupsBalances.Where(balance => balance.LoadedListId == listId),
                gr => gr.Id, bal => bal.AccountGroupId,
                (gr, bal) => new {
                    Id = gr.Id,
                    OperationClass = gr.OperationsClassId,
                    LoadedList = bal.LoadedListId,
                    IncomingActive = bal.IncomingActive,
                    IncomingPassive = bal.IncomingPassive,
                    Debit = bal.Debit,
                    Credit = bal.Credit,
                    OutgoingActive = bal.OutgoingActive,
                    OutgoingPassive = bal.OutgoingPassive
                }))
            {
                InsertAccountsInfo(result, listId, accountGroupInfo.Id, context);

                var singleRowArray = new string[7];
                singleRowArray[0] = accountGroupInfo.Id.ToString();
                singleRowArray[1] = accountGroupInfo.IncomingActive.ToString();
                singleRowArray[2] = accountGroupInfo.IncomingPassive.ToString();
                singleRowArray[3] = accountGroupInfo.Debit.ToString();
                singleRowArray[4] = accountGroupInfo.Credit.ToString();
                singleRowArray[5] = accountGroupInfo.OutgoingActive.ToString();
                singleRowArray[6] = accountGroupInfo.OutgoingPassive.ToString();
                result.Add(singleRowArray);
            }
        }
        
        private void InsertOperationClassesInfo(List<string[]> result, int listId, ApplicationDbContext context)
        {
            foreach (var operationClassBalance in context.OperationsClassesBalances.Where(balance => balance.LoadedListId == listId))
            {
                var operationClass = context.OperationsClasses.Where(operationClass => operationClass.Id == operationClassBalance.OperationsClassId).First();
                result.Add(new string[] { $"КЛАСС  {operationClass.Id} {operationClass.Name}" });

                InsertAccountGroupsInfo(result, listId, operationClass.Id, context);

                var singleRowArray = new string[7];
                singleRowArray[0] = "ПО КЛАССУ";
                FillBalanceRow(operationClassBalance, ref singleRowArray);

                result.Add(singleRowArray);
            }
        }

        public void FillBalanceRow(IBalance balance, ref string[] singleRowArray)
        {
            singleRowArray[1] = balance.IncomingActive.ToString();
            singleRowArray[2] = balance.IncomingPassive.ToString();
            singleRowArray[3] = balance.Debit.ToString();
            singleRowArray[4] = balance.Credit.ToString();
            singleRowArray[5] = balance.OutgoingActive.ToString();
            singleRowArray[6] = balance.OutgoingPassive.ToString();
        }

        public async Task<string[][]> GetFileAsTableAsync(int id, ApplicationDbContext context)
        {
            List<string[]> fileTable = new List<string[]>();

            LoadedList loadedList = context.LoadedLists.Where(list => list.Id == id).First();

            var singleRowArray = new string[1];
            singleRowArray[0] = loadedList.Path;
            fileTable.Add(singleRowArray); //first line contains filePath

            singleRowArray = new string[1];
            singleRowArray[0] = loadedList.Name;
            fileTable.Add(singleRowArray); //second line contains spreadsheet name (including bank name)

            singleRowArray = new string[1];
            singleRowArray[0] = $"за период с {loadedList.PeriodFrom.ToString("dd.MM.yyyy")} по {loadedList.PeriodFrom.ToString("dd.MM.yyyy")}";
            fileTable.Add(singleRowArray); //third line contains balance period

            singleRowArray = new string[7]; //fourth line contains balance columns names
            singleRowArray[0] = "Б/сч";
            singleRowArray[1] = "Входящий актив";
            singleRowArray[2] = "Входящий пассив";
            singleRowArray[3] = "Дебет";
            singleRowArray[4] = "Кредит";
            singleRowArray[5] = "Исходящий актив";
            singleRowArray[6] = "Исходящий пассив";
            fileTable.Add(singleRowArray);

            InsertOperationClassesInfo(fileTable, id, context); //then balances by categories

            FullListBalance fullListBalance = context.FullListsBalances.Where(listBalance => listBalance.LoadedListId == id).First();
            singleRowArray = new string[7];
            singleRowArray[0] = "БАЛАНС";
            FillBalanceRow(fullListBalance, ref singleRowArray);
            fileTable.Add(singleRowArray); //and total balance info

            return fileTable.ToArray();
        }

        /// <summary>
        /// Reads info from cells in row and writes it into balanceEntity
        /// </summary>
        private void FillBalance(IBalance balanceEntity, List<string> row)
        {
            int columnIndex = 0;
            foreach(var cell in row.Skip(1)) //skip first cell with balance name
            {
                var numberFromCell = ToDouble(cell);
                switch (columnIndex)
                {
                    case 0:
                        balanceEntity.IncomingActive = numberFromCell;
                        break;
                    case 1:
                        balanceEntity.IncomingPassive = numberFromCell;
                        break;
                    case 2:
                        balanceEntity.Debit = numberFromCell;
                        break;
                    case 3:
                        balanceEntity.Credit = numberFromCell;
                        break;
                    case 4:
                        balanceEntity.OutgoingActive = numberFromCell;
                        break;
                    case 5:
                        balanceEntity.OutgoingPassive = numberFromCell;
                        break;
                }
                columnIndex++;
            }
        }

        public async Task LoadFileToDbAsync(string fileName, ApplicationDbContext context)
        {
            ExcelIO excelIO = new ExcelIO();
            var data = excelIO.ReadExcelFile(fileName);

            var accountRegex = new Regex("^\\d{4}$"); //four-digits number
            var accountGroupRegex = new Regex("^\\d{2}$"); //two-digits number
            var dateRegex = new Regex("\\d{2}[.]\\d{2}[.]\\d{4}"); //date: dd.MM.yyyy (for balance periods)

            var loadedList = new LoadedList()
            {
                Path = fileName.Split('/',2).Last(),
                Name = $"Балансовая ведомость банка '{data.First().First()}'"
            };

            var operationsClass = new OperationsClass();

            foreach (var row in data) //checks all rows according to data in the first cell, ignores unnecessary information
            {
                if (row.First().StartsWith("за период с")) //row with balance periods
                {
                    var periods = dateRegex.Matches(row.First()).ToArray();

                    var periodFromElements = periods[0].Value.Split('.');
                    var periodFrom =
                        new DateTime(ToInt32(periodFromElements[2]), ToInt32(periodFromElements[1]), ToInt32(periodFromElements[0]));
                    loadedList.PeriodFrom = periodFrom;

                    var periodToElements = periods[1].Value.Split('.');
                    var periodTo =
                        new DateTime(ToInt32(periodToElements[2]), ToInt32(periodToElements[1]), ToInt32(periodToElements[0]));
                    loadedList.PeriodTo = periodTo;

                    context.Add(loadedList);
                    context.SaveChanges(); //save context to autogenerate id for loaded file
                }
                if (row.First().StartsWith("КЛАСС ")) //operations class name and id
                {
                    var classInfoSubstrings = row.First().Split(' ', 4);
                    operationsClass.Id = ToInt32(classInfoSubstrings[2]);
                    operationsClass.Name = classInfoSubstrings[3];
                }
                if (row.First().StartsWith("ПО КЛАССУ")) //operations class balance
                {
                    var operationsClassBalance = new OperationsClassBalance()
                    {
                        OperationsClassId = operationsClass.Id,
                        LoadedListId = loadedList.Id
                    };
                    FillBalance(operationsClassBalance, row);

                    context.Add(operationsClass);
                    context.Add(operationsClassBalance);
                    operationsClass = new OperationsClass();
                }
                if (row.First().StartsWith("БАЛАНС")) //total balance
                {
                    var fullListBalance = new FullListBalance()
                    {
                        LoadedListId = loadedList.Id
                    };
                    FillBalance(fullListBalance, row);
                    context.Add(fullListBalance);
                    await context.SaveChangesAsync(); //save context because list must finish
                }
                if (accountGroupRegex.IsMatch(row.First())) //account group balance
                {
                    var accountGroup = new AccountGroup()
                    {
                        Id = ToInt32(row.First()),
                        OperationsClassId = operationsClass.Id
                    };
                    context.Add(accountGroup);
                    var accountGroupBalance = new AccountGroupBalance()
                    {
                        AccountGroupId = accountGroup.Id,
                        LoadedListId = loadedList.Id
                    };
                    FillBalance(accountGroupBalance, row);

                    context.Add(accountGroupBalance);
                }
                if (accountRegex.IsMatch(row.First())) //single account balance
                {
                    var account = new Account()
                    {
                        Id = ToInt32(row.First()),
                        AccountGroupId = ToInt32(row.First()) / 100
                    };
                    context.Add(account);
                    var accountBalance = new AccountBalance()
                    {
                        AccountId = account.Id,
                        AccountGroupId = account.AccountGroupId,
                        LoadedListId = loadedList.Id
                    };
                    FillBalance(accountBalance, row);
                    await context.AddAsync(accountBalance);
                }
            }
        }

        public async Task SaveDbToFileAsync(int loadedListId, string fileName, ApplicationDbContext context)
        {
            ExcelIO excelIO = new ExcelIO();
            var info = (await GetFileAsTableAsync(loadedListId, context)).Skip(1).ToArray();
            excelIO.WriteExcelFile(fileName, info);
        }
    }
}

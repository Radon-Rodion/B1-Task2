using System.Runtime.InteropServices;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelLibrary
{
    public class ExcelIO
    {
        public List<List<string>> ReadExcelFile(string fileName)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileName);
            Excel.Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            var result = new List<List<string>>();

            bool listFinished = false;
            int rowNumber = 1;
            do
            {
                var row = new List<string>();

                for (int columnNumber = 1; columnNumber <= 7; columnNumber++)
                {
                    if (xlRange.Cells[rowNumber, columnNumber] != null && xlRange.Cells[rowNumber, columnNumber].Value2 != null)
                    {
                        row.Add(xlRange.Cells[rowNumber, columnNumber].Value2.ToString());
                    }
                }
                rowNumber++;
                result.Add(row);
                listFinished = row.First() == "БАЛАНС";
            } while(!listFinished);

            return result;
        }

        public void WriteExcelFile(string fileName, string[][] info)
        {
            Excel.Application xlApp = new Excel.Application();
            var misValue = System.Reflection.Missing.Value;

            Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(misValue);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            for(int row = 0; row < info.Length; row++)
            {
                for(int column = 0; column < info[row].Length; column++)
                {
                    xlWorksheet.Cells[row+1, column+1] = info[row][column];
                }
            }

            xlWorkbook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkbook.Close(true, misValue, misValue);
            xlApp.Quit();
        }
    }
}
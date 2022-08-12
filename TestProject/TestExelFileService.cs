using ExcelLibrary;
using System.Runtime.InteropServices;
using System.Text;
using Task2.Services;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestProject
{
    [TestClass]
    public class TestExelFileService
    {
        [TestMethod]
        public void TestExelFileProcessor()
        {
            var processor = new ExcelIO();
            processor.ReadExcelFile("example.xls");
        }

        [TestMethod]
        public void TestExelWriting()
        {
            var processor = new ExcelIO();
            string[][] info =
            {
                new string[] { "row1, col1", "row1, clo2"},
                new string[] {"row2, col1"}
            };
            processor.WriteExcelFile("sharp.xls", info);
        }
    }
}
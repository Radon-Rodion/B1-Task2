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
            var res = processor.ReadExcelFile("D:/example.xls");
            StringBuilder builder = new StringBuilder();

            foreach(var row in res)
            {
                foreach(var cell in row)
                {
                    builder.Append(cell);
                    builder.Append("   |   ");
                }
                builder.Append("\r\n");
            }
            Assert.Fail(builder.ToString());
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
            processor.WriteExcelFile("D:\\sharp.xls", info);
        }
    }
}
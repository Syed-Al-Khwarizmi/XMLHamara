using Excel;
using System;

namespace akExcelAsZipDemo
{
    class Program : Workbook
    {
        //  from
        //  https://www.codeproject.com/tips/801032/csharp-how-to-read-xlsx-excel-file-with-lines-ofthe
        //
        //  inspired:
        //  https://github.com/ahmadalli/ExcelReader

        static void Main(string[] args)
        {
            const string fileName = @"C:\Users\Ali_H\Desktop\TestCsvToXml\ots-enforcement-order-listing.xlsx";

            var worksheets = Worksheets(fileName);

            foreach (worksheet ws in worksheets)
            {
                foreach (var row in ws.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (cell != null)
                        {
                            Console.Write(cell.Text + "\t");
                        }
                    }
                    Console.WriteLine("");
                }
            }

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
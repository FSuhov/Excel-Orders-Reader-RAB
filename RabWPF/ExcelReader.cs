using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace Rab_Forms
{
    public class ExcelReader
    {
        Excel.Workbook MyBook = null;
        Excel.Application MyApp = null;
        Excel.Worksheet MySheet = null;
        public int lastRow = 0;

        //конструктор с параметрами - путь к файлу и индекс листа. Запустит excel и файл, откроет лист 
        public ExcelReader(string source, int sheetIdx = 1)
        {
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(source);
            MySheet = MyBook.Sheets[sheetIdx];
            lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        }
        //конструктор по умолчанию - просто запустит excel        
        public ExcelReader()
        {
            MyApp = new Excel.Application();
            MyApp.Visible = false;
        }

        //метод открытия файла
        public void OpenWorkBook(string path, int sheetIdx = 1)
        {
            try
            {
                MyBook = MyApp.Workbooks.Open(path);
                MySheet = MyBook.Sheets[sheetIdx];
                lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            }
            catch (Exception err)
            {
                Console.WriteLine($"Could not open file {path}...");
            }
        }

        public void CreateWorkBook(string path)
        {
            MyBook = MyApp.Workbooks.Add(Type.Missing);
            MySheet = MyBook.ActiveSheet as Excel.Worksheet;
        }


        public Array OneLineReader(int idx)
        {
            return (System.Array)MySheet.get_Range(Globals.rabFirstCell + idx.ToString(), Globals.rabLastCell + idx.ToString()).Cells.Value;
        }



        public void OneLineWriter(System.Array data, int idx)
        {
            Excel.Range range = MySheet.get_Range(Globals.rabRangeFirst + idx, Globals.rabRangeLast + idx);
            range.Value = data;
            range = MySheet.get_Range(Globals.rabRangeFirstDecimal + idx, Globals.rabRangeLast + idx);
            range.NumberFormat = "#,##0.00";            
        }

        //закрытие файла без сохранения
        public void QuiteBook()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(MySheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(MyBook.Sheets);
            MyBook.Close(true);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(MyBook);
            MyBook = null;
            MySheet = null;
        }

        //закрытие файла и программы
        public void QuiteExcel()
        {
            MyApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(MyApp);
            MyBook = null;
            MyApp = null;
            MySheet = null;
        }

        public void SaveAndQuiteExcel(string path)
        {
            MyBook.SaveAs(path);
            QuiteBook();
            QuiteExcel();
        }
    }
}
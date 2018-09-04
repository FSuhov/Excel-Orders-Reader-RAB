using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rab_Forms
{
    public class RabData
    {
        public delegate void data_read_status(string msg);
        data_read_status _sts;
        data_read_status _fls;
        public List<LineRab> Lines { get; set; } = new List<LineRab>();
        
        public void RegisterHandler(data_read_status sts, data_read_status fls)
        {
            _sts = sts;
            _fls = fls; 
        }

        public void ReadData(string filePath)
        {
            ExcelReader xReader = new ExcelReader(filePath, 2);
            try
            {
                for (int i = Globals.rabShiftToFirstRow; i < xReader.lastRow; i++)
                {
                    Array worksheetRowData = xReader.OneLineReader(i);
                    if (Globals.RabArticulParser(worksheetRowData))
                    {
                        Lines.Add(new LineRab(xReader.OneLineReader(i)));
                    }
                }
                _sts("Данные считаны, можно работать дальше");
            }
            finally
            {
                xReader.QuiteExcel();
            }
        }

        public void ReadOrders(List<string> listOfFiles)
        {
            ExcelReader xr = new ExcelReader();
            try
            {
                foreach (string item in listOfFiles)
                {
                    ReadOrder(item, xr);
                }
            }
            finally
            {
                xr.QuiteExcel();
            }
        }


        public void ReadOrder(string filePath, ExcelReader xr)
        {
            //debug real test
            int sumBefore = 0;
            int sumAdded = 0;
            for (int i = 0; i < Lines.Count; i++)
            {
                sumBefore += Lines[i].Sum();
            }
            //end debug

            _fls($"Открываю файл {filePath}...\n");
            xr.OpenWorkBook(filePath, 2);
            
            try
            {
                for (int i = Globals.rabShiftToFirstRow; i < xr.lastRow; i++)
                {
                    Array worksheetRowData = xr.OneLineReader(i);
                    if (Globals.RabArticulParser(worksheetRowData))
                    {
                        string art = worksheetRowData.GetValue(1, 1).ToString();
                        string col = worksheetRowData.GetValue(1, 4)?.ToString();

                        if (col == null) col = "single color";
                        try
                        {
                            var product = Lines.Where(item => item.Articul == art && item.Color == col);


                            LineRab lineToAddNumbers = product.ToList()[0] as LineRab;

                            if (lineToAddNumbers != null)
                            {
                                lineToAddNumbers.AddQuantities(worksheetRowData);
                            }
                        }
                        catch
                        {
                            Console.WriteLine($"File {filePath} articul {art}");
                        }
                    }
                }

                //debug real test
                int sumAfter = 0;
                for (int i = 0; i < Lines.Count; i++)
                {
                    sumAfter += Lines[i].Sum();
                }
                sumAdded = sumAfter - sumBefore;
                Console.WriteLine($"Order: {filePath} || Items added: {sumAfter - sumBefore}!");
                //end debug                
            }
            finally
            {
                xr.QuiteBook();
                _fls($"Данные из Файла {filePath} прочитаны.\n Добавлено {sumAdded} "  );
            }
        }

        public void WriteData(string filePath)
        {
            ExcelReader xr = new ExcelReader();
            xr.CreateWorkBook(filePath);
            xr.OneLineWriter(Globals.titlesRab, 1);
            List<LineRab> orderedLines = (Lines.Where(item => item.Sum() > 0)).ToList();

            for (int i = 0; i < orderedLines.Count; i++)
            {
                xr.OneLineWriter(orderedLines[i].ToOutput(), i + 2);
                //Console.WriteLine("Amin");
            }

            xr.SaveAndQuiteExcel(filePath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Rab_Forms
{
    public static class Globals
    {
        public static int counter = 0;
        

        public static int rabNumberOfSizes = 6;
        public static string[] rabSizes = new string[] { "XS", "S", "M", "L", "XL", "XXL" };
        public static int rabShiftToFirstRow = 12;
        public static int rabShiftToQuantity = 6; //смещение до размера XS в заказной форме
        public static string rabFirstCell = "A";
        public static string rabLastCell = "N";

        public static string rabRangeFirst = "A"; //первая колонка при записи данных
        public static string rabRangeLast = "R"; //последняя колонка при записи данных
        public static string rabRangeFirstDecimal = "P"; //первая колонка, с которой данные должны быть с двумя знаками после запятой
        public static string[] titlesRab = new string[] {"Articul",
                                                      "Category",
                                                      "Model",
                                                      "Color",
                                                       "Size Type",
                                                       "XS", "S", "M", "L", "XL", "XXL", "One Size", "Left", "Right", "total", "ExPrice", "Price","sum"};

        public enum RabSizeTypes
        {
            Regular,
            One,
            LeftRight
        };

        public static bool RabArticulParser(Array rowData)
        {
            string pattern = @"\w\w\w?-\w?\d\d-?\w?\w?";
            if (rowData.GetValue(1, 1) != null)
            {
                string data = rowData.GetValue(1, 1).ToString();
                if (data != null)
                {
                    Regex regex = new Regex(pattern);
                    return regex.IsMatch(data);
                }
            }
            return false;
        }


    }
}
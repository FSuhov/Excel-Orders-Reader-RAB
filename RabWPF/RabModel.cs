using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rab_Forms
{
    public class LineRab
    {
        public int ID { get; set; }
        public string Articul { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string AvailableSizes { get; set; }
        public Globals.RabSizeTypes SizeType { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal ExPrice { get; set; }
        public List<Unit> ListOfSKU { get; set; } = new List<Unit>();

        public int TotalOrdered
        {
            get { return Sum(); }
        }
        public decimal Amount
        {
            get { return Sum() * Price; }
        }

        public LineRab(Array OneRowValues)
        {
            ID = ++Globals.counter;
            try
            {
                Articul = OneRowValues.GetValue(1, 1).ToString();
                Name = OneRowValues.GetValue(1, 3).ToString();
                if (OneRowValues.GetValue(1, 4) != null)
                {
                    Color = OneRowValues.GetValue(1, 4).ToString();
                }
                else Color = "single color";
                if (OneRowValues.GetValue(1, 5) != null)
                {
                    AvailableSizes = OneRowValues.GetValue(1, 5).ToString();
                }
                Category = OneRowValues.GetValue(1, 2).ToString();
                SizeType = Globals.RabSizeTypes.Regular;

                if (Category == "SLEEPING BAGS")
                {
                    SizeType = Globals.RabSizeTypes.LeftRight;
                }
                if (OneRowValues.GetValue(1, 5).ToString() == "One Size") { SizeType = Globals.RabSizeTypes.One; }

                Price = decimal.Parse(OneRowValues.GetValue(1, 14).ToString());
                this.CreateSKUList();
            }
            catch (Exception err)
            {
                Console.WriteLine($"error, item ID {ID} has not been created");
            }
        }

        public void CreateSKUList()
        {
            for (int i = 0; i < Globals.rabNumberOfSizes; i++)
            {
                ListOfSKU.Add(new Unit(Globals.rabSizes[i]));
            }
        }

        public void AddQuantities(Array OneRowValues)
        {
            for (int i = 0, j = Globals.rabShiftToQuantity; i < ListOfSKU.Count; i++, j++)
            {
                int order = 0;
                string strOrder = OneRowValues.GetValue(1, j)?.ToString();
                if (strOrder != null) { int.TryParse(OneRowValues.GetValue(1, j).ToString(), out order); }
                if (order > 0) { ListOfSKU[i].Quantity += order; }
            }
        }

        public override string ToString()
        {
            return $"{ID} {Name} | {Articul} | {Color}";
        }

        public int Sum()
        {
            int sumOfOrder = 0;
            foreach (Unit elem in ListOfSKU)
            {
                sumOfOrder += elem.Quantity;
            }
            return sumOfOrder;
        }

        public object[] ToOutput()
        {
            object[] data = new object[18];
            data[0] = Articul;
            data[1] = Category;
            data[2] = Name;
            data[3] = Color;
            data[4] = SizeType.ToString();

            int i = 5;
            if (SizeType == Globals.RabSizeTypes.Regular)
            {

                for (int j = 0; j < ListOfSKU.Count; i++, j++)
                {
                    if (ListOfSKU[j].Quantity > 0)
                    {
                        data[i] = ListOfSKU[j].Quantity;
                    }
                    else data[i] = "";
                }
                data[i++] = "";
                data[i++] = "";
                data[i++] = "";
            }
            if (SizeType == Globals.RabSizeTypes.One)
            {
                for (int j = 0; j < ListOfSKU.Count; i++, j++) { data[i] = ""; }
                if (ListOfSKU[0].Quantity > 0) { data[i++] = ListOfSKU[0].Quantity; }
                else data[i++] = "";
                data[i++] = "";
                data[i++] = "";
            }
            if (SizeType == Globals.RabSizeTypes.LeftRight)
            {
                for (int j = 0; j < ListOfSKU.Count + 1; i++, j++) { data[i] = ""; }

                if (ListOfSKU[0].Quantity > 0) { data[i++] = ListOfSKU[0].Quantity; }
                else data[i++] = "";
                if (ListOfSKU[1].Quantity > 0) { data[i++] = ListOfSKU[1].Quantity; }
                else data[i++] = "";
            }

            data[i++] = Sum();
            data[i++] = Price;
            data[i++] = ExPrice;
            data[i++] = Sum() * Price;
            return data;
        }
    }
}

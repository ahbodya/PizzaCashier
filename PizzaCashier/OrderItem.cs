using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaCashier
{
    public class OrderItem
    {
        public string PizzaName {  get; set; }
        public string SizeName { get; set; }
        public List<string> Extras { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public bool HasDiscount { get; set; }
        public decimal Total {  get; set; }

        public override string ToString()
        {
            string extras = Extras.Count > 0 ? " + " + string.Join(", ", Extras) : "";
            string discount = HasDiscount ? $" | -{PizzaCalculator.DiscountRate * 100:F0}% discount" : "";
            return $"{Quantity} x {PizzaName} ({SizeName}){extras} | price for one unit {UnitPrice:F2} euro." +
                $"{discount} | Total: {Total:F2} euro.";
        }

        public List<string> ToLines()
        {
            List<string> lines = new List<string>();
            lines.Add($"{Quantity} x {PizzaName} ({SizeName})");

            if ( Extras.Count > 0)
            {
                lines.Add("     +" + string.Join (", ", Extras));
            }

            string discount = HasDiscount ? $"  (-{PizzaCalculator.DiscountRate * 100:F0}% discount)" : "";
            lines.Add($"     = {Total:F2} eur.{discount}");

            lines.Add("");
            return lines ;
        }
    }
}

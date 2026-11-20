using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaCashier
{
    public class PizzaCalculator
    {
        public static readonly string[] PizzaNames = { "Margarita", "Caprichosa", "Peperoni", "Burger Pizza", "Formaji" };
        public static readonly decimal[] PizzaPrices = { 10.00m, 13.00m, 15.00m, 20.00m, 18.00m };

        public const decimal ExtraPrice = 1.55m;
        public const decimal DiscountThreshold = 40.00m;
        public const decimal DiscountRate = 0.25m;

        public static decimal GetSizeMultiplier(int sizeIndex)
        {
            switch (sizeIndex)
            {
                case 1: return 1.25m;
                case 2: return 1.50m;
                default: return 1.00m;
            }
        }

        public static OrderItem CreateItem(int pizzaIndex, int sizeIndex, string sizeName, List<string> extras, int quantity)
        {
            decimal basePrice = PizzaPrices[pizzaIndex];
            decimal unitPrice = basePrice * GetSizeMultiplier(sizeIndex)+extras.Count * ExtraPrice;
            decimal subtotal = unitPrice * quantity;

            bool hasDiscount = subtotal > DiscountThreshold;
            decimal total = hasDiscount ? subtotal * (1 - DiscountRate) : subtotal;

            return new OrderItem
            {
                PizzaName = PizzaNames[pizzaIndex],
                SizeName = sizeName,
                Extras = extras,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Subtotal = subtotal,
                HasDiscount = hasDiscount,
                Total = Math.Round(total, 2)
            };
        }

    }
}

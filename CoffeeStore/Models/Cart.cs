﻿using System.Collections.Generic;
using System.Linq;

namespace CoffeeStore.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = Lines
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public virtual void Clear() => Lines.Clear();

        public decimal ComputeTotalValue() =>
            Lines.Sum(e => e.Product.Price * e.Quantity);


        public class CartLine
        {
            public int CartLineID { get; set; }
            public Product Product { get; set; } = new Product();
            public int Quantity { get; set; }
            public decimal Price => Product.Price;
        }
    }
}

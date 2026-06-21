using System;
using System.Collections.Generic;

class Product
{
    public int Id;
    public string Name;
}

class Program
{
    static Product SearchProduct(List<Product> products, string name)
    {
        foreach (Product product in products)
        {
            if (product.Name == name)
            {
                return product;
            }
        }

        return null;
    }

    static void Main()
    {
        List<Product> products = new List<Product>()
        {
            new Product{Id=1,Name="Laptop"},
            new Product{Id=2,Name="Mobile"},
            new Product{Id=3,Name="Keyboard"}
        };

        Product result = SearchProduct(products, "Mobile");

        if(result != null)
        {
            Console.WriteLine("Product Found");
            Console.WriteLine(result.Name);
        }
        else
        {
            Console.WriteLine("Product Not Found");
        }
    }
}
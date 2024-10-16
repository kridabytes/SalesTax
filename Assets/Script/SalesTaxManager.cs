// Makkajai Dev challenge task - (Harshit Bhatt)”

using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// SalesTaxManager - Manages the calculation and display of sales tax and total costs for items in a shopping basket.
/// PrintReceipt - Method to calculate and print the receipt details
/// IsExemptItem - Mehthod to Check if the item belongs to exempt categories
/// ParseInput -  Method to parse the input and generate items
/// </summary>
public class SalesTaxManager : MonoBehaviour
{
    readonly List<string> exemptItems = new List<string> { "book", "chocolate", "pill" };

    void Start()
    {
        string[] input1 = { "1 book at 12.49", "1 music CD at 14.99", "1 chocolate bar at 0.85" };
        Debug.Log("Output 1:");
        PrintReceipt(ParseInput(input1));

        string[] input2 = { "1 imported box of chocolates at 10.00", "1 imported bottle of perfume at 47.50" };
        Debug.Log("Output 2:");
        PrintReceipt(ParseInput(input2));

        string[] input3 = { "1 imported bottle of perfume at 27.99", "1 bottle of perfume at 18.99", "1 packet of headache pills at 9.75", "1 box of imported chocolates at 11.25" };
        Debug.Log("Output 3:");
        PrintReceipt(ParseInput(input3));
    }

    public void PrintReceipt(List<Item> items)
    {
        double totalTaxes = 0;
        double totalCost = 0;

        foreach (var item in items)
        {
            double itemTax = item.GetTotalTax();
            double itemTotalPrice = item.GetTotalPrice();

            totalTaxes += itemTax;
            totalCost += itemTotalPrice;

            Debug.Log(item.Quantity + " " + item.Name + " " + itemTotalPrice);
        }

        Debug.Log("Sales Taxes: " + totalTaxes );
        Debug.Log("Total: " + totalCost);
    }
    public List<Item> ParseInput(string[] input)
    {
        List<Item> items = new List<Item>();

        foreach (var line in input)
        {
            string[] parts = line.Split(new string[] { " at " }, StringSplitOptions.None);
            string[] quantityAndName = parts[0].Trim().Split(' ');
            int quantity = Convert.ToInt32(quantityAndName[0]);
            string name = string.Join(' ', quantityAndName, 1, quantityAndName.Length - 1).Trim();
            double price = Convert.ToDouble(parts[1].Trim());

            bool isImported = name.Contains("imported");
            bool isExempt = IsExemptItem(name);

            items.Add(new Item(name, isImported, isExempt, price, quantity));
        }

        return items;
    }
    public bool IsExemptItem(string name)
    {
        foreach (var exempt in exemptItems)
        {
            if (name.Contains(exempt))
            {
                return true;
            }
        }
        return false;
    }


}
/// <summary>
/// Item class to store the properties of each item
/// CalculateTaxedPrice() Calculate the taxed price based on import and sales tax rules
/// GetTotalPrice() Get total price for the item including tax and quantity
/// GetTotalTax() Get total tax for the item including quantity
/// </summary>
public class Item
{
    public string Name;
    public bool IsImported;
    public bool IsExempt;
    public double Price;
    public double TaxedPrice;
    public int Quantity;

    public Item(string name, bool isImported, bool isExempt, double price, int quantity)
    {
        Name = name;
        IsImported = isImported;
        IsExempt = isExempt;
        Price = price;
        Quantity = quantity;
        TaxedPrice = CalculateTaxedPrice();
    }

    private double CalculateTaxedPrice()
    {
        double salesTax = 0;

        if (!IsExempt)
            salesTax += 0.10 * Price;
        

        if (IsImported)
            salesTax += 0.05 * Price;
        

        salesTax = Math.Ceiling(salesTax * 20) / 20;

        return Price + salesTax;
    }

    public double GetTotalPrice()
    {
        return TaxedPrice * Quantity;
    }

    public double GetTotalTax()
    {
        return (TaxedPrice - Price) * Quantity;
    }
}


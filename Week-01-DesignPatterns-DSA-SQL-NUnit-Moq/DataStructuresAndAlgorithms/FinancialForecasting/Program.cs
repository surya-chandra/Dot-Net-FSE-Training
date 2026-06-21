using System;

class Program
{
    static double Forecast(double amount, int years)
    {
        if (years == 0)
            return amount;

        return Forecast(amount * 1.10, years - 1);
    }

    static void Main()
    {
        double result = Forecast(10000, 5);

        Console.WriteLine("Future Value : " + result);
    }
}
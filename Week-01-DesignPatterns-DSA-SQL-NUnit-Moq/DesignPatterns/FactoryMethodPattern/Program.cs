using System;

interface INotification
{
    void Send();
}

class EmailNotification : INotification
{
    public void Send()
    {
        Console.WriteLine("Email Sent");
    }
}

class SMSNotification : INotification
{
    public void Send()
    {
        Console.WriteLine("SMS Sent");
    }
}

class NotificationFactory
{
    public static INotification CreateNotification(string type)
    {
        if (type == "Email")
            return new EmailNotification();

        return new SMSNotification();
    }
}

class Program
{
    static void Main()
    {
        INotification notification =
            NotificationFactory.CreateNotification("Email");

        notification.Send();
    }
}
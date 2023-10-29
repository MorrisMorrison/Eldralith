using System;
using Godot;

public class GDPrint
{
    public static string GetTimestamp()
    {
        return DateTime.Now.ToString("dd.MM.yyyy-HH:mm:ss");
    }

    public static void Print(string text)
    {
        GD.Print($"{GetTimestamp()}: {text}");
    }

    public static void PrintErr(string text)
    {
        GD.PrintErr($"{GetTimestamp()}: {text}");
    }
}
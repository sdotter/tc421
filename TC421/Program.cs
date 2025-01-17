﻿namespace TC421
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            Main main = new Main();
            Thread t = new Thread(() => main.DoWork(args.Length > 0 ? args : new string[] { "--help" }));

            t.IsBackground = true;
            t.Start();

            TimeSpan waitTime = TimeSpan.FromSeconds(0.5);
            DateTime expireTime = DateTime.Now + waitTime;

            ConsoleKeyInfo keyInfo;

            while (TC421.Main.KeepGoing || (DateTime.Now < expireTime))
            {
                if (Console.KeyAvailable)
                {
                    keyInfo = Console.ReadKey(true);
                    expireTime = DateTime.Now + waitTime;

                    if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers == ConsoleModifiers.Control)
                    {
                        TC421.Main.KeepGoing = false;
                        break;
                    }
                }
            }
            t.Join();
        }
    }
}
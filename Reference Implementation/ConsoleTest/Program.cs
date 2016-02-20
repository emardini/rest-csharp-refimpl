namespace ConsoleTest
{
    using System;

    using ConsoleTest.Testing;

    using OANDARestLibrary;

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var proxy = new Rest("https://api-fxpractice.oanda.com/v1/",
                "https://api-fxpractice.oanda.com/v1/",
                "https://stream-fxpractice.oanda.com/v1/",
                "https://stream-fxpractice.oanda.com/v1/",
                "https://api-fxpractice.oanda.com/labs/v1/",
                "eae6770a420a9d34e26e72476f7ba0b9-a4cf6da4641a217853a309b5257c5420");
            var tests = new RestTest(5027596, proxy);
            var task = tests.RunTest();
            task.Wait();
            Console.WriteLine("Tests complete");
            Console.ReadLine();
        }

        #endregion
    }
}
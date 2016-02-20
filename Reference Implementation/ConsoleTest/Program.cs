namespace ConsoleTest
{
    using System;

    using ConsoleTest.Testing;

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var tests = new RestTest();
            var task = tests.RunTest();
            task.Wait();
            Console.WriteLine("Tests complete");
            Console.ReadLine();
        }

        #endregion
    }
}
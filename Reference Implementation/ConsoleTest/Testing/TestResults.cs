namespace ConsoleTest.Testing
{
    using System;
    using System.Collections.Generic;

    internal class TestResults
    {
        #region Fields

        private readonly List<TestResult> _results = new List<TestResult>();

        #endregion

        #region Public Methods and Operators

        public void Add(TestResult testResult)
        {
            this._results.Add(testResult);
        }

        public bool Verify(string success, string testDescription)
        {
            return this.Verify(!string.IsNullOrEmpty(success), testDescription);
        }

        public bool Verify(bool success, string testDescription)
        {
            this._results.Add(new TestResult { Success = success, Details = testDescription });
            if (!success)
            {
                Console.WriteLine(success + ": " + testDescription);
            }
            return success;
        }

        #endregion
    }
}
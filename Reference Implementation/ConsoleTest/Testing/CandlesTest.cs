namespace ConsoleTest.Testing
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using OANDARestLibrary;
    using OANDARestLibrary.TradeLibrary.DataTypes.Communications.Requests;

    internal class CandlesTest
    {
        #region Fields

        private readonly TestResults _results;

        #endregion

        #region Constructors and Destructors

        public CandlesTest(TestResults results)
        {
            this._results = results;
        }

        #endregion

        #region Public Methods and Operators

        public async Task Run()
        {
            // Setup minimum requirements
            Func<CandlesRequest> request = () => new CandlesRequest { instrument = "EUR_USD" };
            // This handles the unmodified, and all defaults
            this.RunBasicTests(request);

            // test count max
            this.TestCount(request, 5000, false, "Retrieved max candles");
            // test count max + 1
            this.TestCount(request, 5001, true, "Exceeded max candles");
            // test count min
            this.TestCount(request, 1, false, "Retrieved min candles");
            // test count min - 1
            this.TestCount(request, 0, true, "Below min candles");
        }

        public async Task RunBasicTests(Func<CandlesRequest> request)
        {
            // Most basic request
            var result = await Rest.GetCandlesAsync(request());
            this._results.Verify(result.Count > 0, "Retrieved basic candles");

            // Test default values
            var defaultValueProps =
                request().GetType().GetTypeInfo().DeclaredFields.Where(
                    x => null != x.GetCustomAttribute(typeof(DefaultValueAttribute)));
            foreach (var defaultValueProp in defaultValueProps)
            {
                var defaultValue = (DefaultValueAttribute)defaultValueProp.GetCustomAttribute(typeof(DefaultValueAttribute));
                var newRequest = request();

                var smartProp = (ISmartProperty)defaultValueProp.GetValue(newRequest);
                smartProp.SetValue(defaultValue.Value);
                defaultValueProp.SetValue(newRequest, smartProp);

                try
                {
                    var testResult = await Rest.GetCandlesAsync(newRequest);
                    this._results.Verify(result.SequenceEqual(testResult), "Testing default value of " + defaultValueProp.Name);
                }
                catch (Exception ex)
                {
                    this._results.Verify(false, "Testing default value of " + defaultValueProp.Name + "\n" + ex);
                }
            }
        }

        #endregion

        #region Methods

        private async Task TestCount(Func<CandlesRequest> request, int count, bool isError, string message)
        {
            var testRequest = request();
            testRequest.count = count;
            if (!isError)
            {
                var result = await Rest.GetCandlesAsync(testRequest);
                this._results.Verify(result.Count == count, message);
            }
            else
            {
                var stringResult = await Rest.MakeErrorRequest(testRequest);
                this._results.Verify(stringResult != null, message);
            }
        }

        #endregion
    }
}
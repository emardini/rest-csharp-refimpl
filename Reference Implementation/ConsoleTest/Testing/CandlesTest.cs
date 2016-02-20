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

        private readonly TestResults results;

        private readonly Rest proxy;

        #endregion

        #region Constructors and Destructors

        public CandlesTest(TestResults results, Rest proxy)
        {
            this.results = results;
            this.proxy = proxy;
        }

        #endregion

        #region Public Methods and Operators

        public async Task Run()
        {
            // Setup minimum requirements
            Func<CandlesRequest> request = () => new CandlesRequest { instrument = "EUR_USD" };
            // This handles the unmodified, and all defaults
            await this.RunBasicTests(request);

            // test count max
            await this.TestCount(request, 5000, false, "Retrieved max candles");
            // test count max + 1
            await this.TestCount(request, 5001, true, "Exceeded max candles");
            // test count min
            await this.TestCount(request, 1, false, "Retrieved min candles");
            // test count min - 1
            await this.TestCount(request, 0, true, "Below min candles");
        }

        public async Task RunBasicTests(Func<CandlesRequest> request)
        {
            // Most basic request
            var result = await this.proxy.GetCandlesAsync(request());
            this.results.Verify(result.Count > 0, "Retrieved basic candles");

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
                    var testResult = await this.proxy.GetCandlesAsync(newRequest);
                    this.results.Verify(result.SequenceEqual(testResult), "Testing default value of " + defaultValueProp.Name);
                }
                catch (Exception ex)
                {
                    this.results.Verify(false, "Testing default value of " + defaultValueProp.Name + "\n" + ex);
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
                var result = await this.proxy.GetCandlesAsync(testRequest);
                this.results.Verify(result.Count == count, message);
            }
            else
            {
                var stringResult = await this.proxy.MakeErrorRequest(testRequest);
                this.results.Verify(stringResult != null, message);
            }
        }

        #endregion
    }
}
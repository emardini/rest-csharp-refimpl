namespace ConsoleTest.Testing
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OANDARestLibrary;

    internal class LabsTest
    {
        #region Fields

        private readonly TestResults _results;

        #endregion

        #region Constructors and Destructors

        public LabsTest(TestResults results)
        {
            this._results = results;
        }

        #endregion

        #region Public Methods and Operators

        public async Task Run()
        {
            // Test Calendar
            await this.RunCalendarTest();
            // Test HPR
            await this.RunHistoricalPositionRatioTest();
            // Test spreads
            await this.RunSpreadsTest();
            // Test COT
            await this.RunCommitmentsOfTradersTest();
            // Test Orderbook
            await this.RunOrderbookTest();
            // Test Autochartist
            await this.RunAutochartistTest();
        }

        #endregion

        #region Methods

        private async Task RunAutochartistTest()
        {
            var autochartistData = await Rest.GetAutochartistData(new Dictionary<string, string> { { "type", "keylevel" } });
            if (this._results.Verify(autochartistData != null, "autochartistData retrieved"))
            {
                this._results.Verify(autochartistData.Count > 0, "autochartist signals retrieved");
            }
        }

        private async Task RunCalendarTest()
        {
            var calendarEvents = await Rest.GetCalendarData("EUR_USD", 2592000);
            var detailsVerified = false;
            if (this._results.Verify(calendarEvents != null, "Retrieved calendar list"))
            {
                this._results.Verify(calendarEvents.Count > 0, "Retrieved calendar events");
                foreach (var calEvent in calendarEvents)
                {
                    this._results.Verify(calEvent.title, "Event Title retrieved");
                    this._results.Verify(calEvent.timestamp, "Event timestamp retrieved");
                    this._results.Verify(calEvent.currency, "Event currency retrieved");
                    if (!string.IsNullOrEmpty(calEvent.unit))
                    {
                        // Forecast isn't always present
                        //_results.Verify(calEvent.forecast, "Event forecast retrieved");
                        this._results.Verify(calEvent.previous, "Event previous retrieved");
                        this._results.Verify(calEvent.actual, "Event actual retrieved");
                        // Market is only present sometimes
                        detailsVerified = detailsVerified || !string.IsNullOrEmpty(calEvent.market);
                    }
                }
            }
            this._results.Verify(detailsVerified, "Confirmed details checked");
        }

        private async Task RunCommitmentsOfTradersTest()
        {
            var cotData = await Rest.GetCommitmentOfTradersData("EUR_USD");
            if (this._results.Verify(cotData != null, "Retrieved cotData"))
            {
                this._results.Verify(cotData.Count > 0, "Retrieved cotData snapshots");
                foreach (var period in cotData)
                {
                    this._results.Verify(period.oi > 0, "cotData oi retrieved");
                    this._results.Verify(period.ncl > 0, "cotData ncl retrieved");
                    this._results.Verify(period.price > 0, "cotData price retrieved");
                    this._results.Verify(period.date > 0, "cotData date retrieved");
                    this._results.Verify(period.ncs > 0, "cotData ncs retrieved");
                    this._results.Verify(period.unit, "cotData unit retrieved");
                }
            }
        }

        private async Task RunHistoricalPositionRatioTest()
        {
            var hprData = await Rest.GetHistoricalPostionRatioData("EUR_USD", 2592000);
            if (this._results.Verify(hprData != null, "Retrieved hprData"))
            {
                this._results.Verify(hprData.Count > 0, "Retrieved hprData snapshots");
                foreach (var hpr in hprData)
                {
                    this._results.Verify(hpr.timestamp > 0, "hpr timestamp retrieved");
                    this._results.Verify(hpr.longPositionRatio > 0, "hpr longPositionRatio retrieved");
                    this._results.Verify(hpr.exchangeRate > 0, "hpr exchangeRate retrieved");
                }
            }
        }

        private async Task RunOrderbookTest()
        {
            var orderbookData = await Rest.GetOrderbookData("EUR_USD", 604800);
            this._results.Verify(orderbookData, "Retrieved orderbook data");
        }

        private async Task RunSpreadsTest()
        {
            var spreadsData = await Rest.GetSpreadData("EUR_USD", 2592000);
            if (this._results.Verify(spreadsData != null, "Retrieved spreadsData"))
            {
                this._results.Verify(spreadsData.Count > 0, "Retrieved spreadsData snapshots");
                foreach (var period in spreadsData)
                {
                    this._results.Verify(period.timestamp > 0, "spreads timestamp retrieved");
                    this._results.Verify(period.max > 0, "spreads max retrieved");
                    this._results.Verify(period.min > 0, "spreads min retrieved");
                    this._results.Verify(period.avg > 0, "spreads avg retrieved");
                }
            }
        }

        #endregion
    }
}
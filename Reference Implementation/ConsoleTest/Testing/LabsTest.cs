namespace ConsoleTest.Testing
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OANDARestLibrary;

    internal class LabsTest
    {
        #region Fields

        private readonly Rest proxy;

        private readonly TestResults results;

        #endregion

        #region Constructors and Destructors

        public LabsTest(TestResults results, Rest proxy)
        {
            this.results = results;
            this.proxy = proxy;
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
            var autochartistData = await this.proxy.GetAutochartistData(new Dictionary<string, string> { { "type", "keylevel" } });
            if (this.results.Verify(autochartistData != null, "autochartistData retrieved"))
            {
                this.results.Verify(autochartistData.Count > 0, "autochartist signals retrieved");
            }
        }

        private async Task RunCalendarTest()
        {
            var calendarEvents = await this.proxy.GetCalendarData("EUR_USD", 2592000);
            var detailsVerified = false;
            if (this.results.Verify(calendarEvents != null, "Retrieved calendar list"))
            {
                this.results.Verify(calendarEvents.Count > 0, "Retrieved calendar events");
                foreach (var calEvent in calendarEvents)
                {
                    this.results.Verify(calEvent.title, "Event Title retrieved");
                    this.results.Verify(calEvent.timestamp, "Event timestamp retrieved");
                    this.results.Verify(calEvent.currency, "Event currency retrieved");
                    if (!string.IsNullOrEmpty(calEvent.unit))
                    {
                        // Forecast isn't always present
                        //_results.Verify(calEvent.forecast, "Event forecast retrieved");
                        this.results.Verify(calEvent.previous, "Event previous retrieved");
                        this.results.Verify(calEvent.actual, "Event actual retrieved");
                        // Market is only present sometimes
                        detailsVerified = detailsVerified || !string.IsNullOrEmpty(calEvent.market);
                    }
                }
            }
            this.results.Verify(detailsVerified, "Confirmed details checked");
        }

        private async Task RunCommitmentsOfTradersTest()
        {
            var cotData = await this.proxy.GetCommitmentOfTradersData("EUR_USD");
            if (this.results.Verify(cotData != null, "Retrieved cotData"))
            {
                this.results.Verify(cotData.Count > 0, "Retrieved cotData snapshots");
                foreach (var period in cotData)
                {
                    this.results.Verify(period.oi > 0, "cotData oi retrieved");
                    this.results.Verify(period.ncl > 0, "cotData ncl retrieved");
                    this.results.Verify(period.price > 0, "cotData price retrieved");
                    this.results.Verify(period.date > 0, "cotData date retrieved");
                    this.results.Verify(period.ncs > 0, "cotData ncs retrieved");
                    this.results.Verify(period.unit, "cotData unit retrieved");
                }
            }
        }

        private async Task RunHistoricalPositionRatioTest()
        {
            var hprData = await this.proxy.GetHistoricalPostionRatioData("EUR_USD", 2592000);
            if (this.results.Verify(hprData != null, "Retrieved hprData"))
            {
                this.results.Verify(hprData.Count > 0, "Retrieved hprData snapshots");
                foreach (var hpr in hprData)
                {
                    this.results.Verify(hpr.timestamp > 0, "hpr timestamp retrieved");
                    this.results.Verify(hpr.longPositionRatio > 0, "hpr longPositionRatio retrieved");
                    this.results.Verify(hpr.exchangeRate > 0, "hpr exchangeRate retrieved");
                }
            }
        }

        private async Task RunOrderbookTest()
        {
            var orderbookData = await this.proxy.GetOrderbookData("EUR_USD", 604800);
            this.results.Verify(orderbookData, "Retrieved orderbook data");
        }

        private async Task RunSpreadsTest()
        {
            var spreadsData = await this.proxy.GetSpreadData("EUR_USD", 2592000);
            if (this.results.Verify(spreadsData != null, "Retrieved spreadsData"))
            {
                this.results.Verify(spreadsData.Count > 0, "Retrieved spreadsData snapshots");
                foreach (var period in spreadsData)
                {
                    this.results.Verify(period.timestamp > 0, "spreads timestamp retrieved");
                    this.results.Verify(period.max > 0, "spreads max retrieved");
                    this.results.Verify(period.min > 0, "spreads min retrieved");
                    this.results.Verify(period.avg > 0, "spreads avg retrieved");
                }
            }
        }

        #endregion
    }
}
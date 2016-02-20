namespace TradingApp2.DataModel
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using OANDARestLibrary;

    using TradingApp2.Data;
    using TradingApp2.TradeLibrary.DataModels;

    public class CandleData : DataGroup
    {
        #region Constructors and Destructors

        public CandleData(string instrument, string granularity)
            : base(instrument + granularity, instrument, granularity, "", "")
        {
            this.Instrument = instrument;
            this.Granularity = granularity;
            this.RequestDataUpdate();
        }

        #endregion

        #region Public Properties

        public string Granularity { get; set; }

        public string Instrument { get; set; }

        #endregion

        #region Public Methods and Operators

        public async void RequestDataUpdate()
        {
            var candles = await Rest.GetCandlesAsync(this.Instrument, this.Granularity);
            this.Items.Clear();
            foreach (var candle in candles)
            {
                this.Items.Add(new CandleViewModel(candle, this));
            }
        }

        #endregion
    }

    public class RatesDataSource
    {
        #region Static Fields

        private static readonly RatesDataSource _ratesDataSource = new RatesDataSource();

        #endregion

        #region Fields

        private readonly ObservableCollection<CandleData> _allCandles = new ObservableCollection<CandleData>();

        #endregion

        #region Public Properties

        public ObservableCollection<CandleData> AllCandles
        {
            get { return this._allCandles; }
        }

        #endregion

        #region Public Methods and Operators

        public static CandleData GetCandles(string instrumentName, string granularity)
        {
            var matches = _ratesDataSource._allCandles.Where(group => group.UniqueId.Equals(instrumentName + granularity));
            if (matches.Count() == 1)
            {
                return matches.First();
            }
            // request the missing data
            var newGroup = new CandleData(instrumentName, granularity);
            _ratesDataSource._allCandles.Add(newGroup);
            return newGroup;
        }

        #endregion
    }
}
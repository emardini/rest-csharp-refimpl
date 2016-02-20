namespace TradingApp2.TradeLibrary.DataModels
{
    using System;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    public class PriceViewModel : DataItem
    {
        #region Fields

        protected Price _model;

        private readonly ChartViewModel _chart;

        #endregion

        #region Constructors and Destructors

        public PriceViewModel(Price data, DataGroup group)
            : base(group)
        {
            this._model = data;
            this._chart = new ChartViewModel(this.Instrument);
        }

        #endregion

        #region Public Properties

        public double Ask
        {
            get { return this._model.ask; }
        }
        public double Bid
        {
            get { return this._model.bid; }
        }

        public override ChartViewModel Chart
        {
            get { return this._chart; }
        }
        public override string Content
        {
            get { return "Last Tick: " + this.Time; }
            set { base.Content = value; }
        }

        //public int Id { get { return _model.id; } }
        //public string Type { get { return _model.type; } }
        //public string Direction { get { return _model.direction; } }
        public string Instrument
        {
            get { return this._model.instrument; }
        }
        public override string Subtitle
        {
            get { return this.Bid + " / " + this.Ask; }
            set { throw new NotSupportedException(); }
        }
        //public int Units { get { return _model.units; } }
        public string Time
        {
            get { return this._model.time; }
        }
        public override string Title
        {
            get { return this.Instrument; }
            set { throw new NotSupportedException(); }
        }
        public override string UniqueId
        {
            get { return this.Instrument; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        //public double StopLoss { get { return _model.stopLoss; } }
        //public long Expiry { get { return _model.expiry; } }
        //public double HighLimit { get { return _model.highLimit; } }
        //public double LowLimit { get { return _model.lowLimit; } }
        //public int TrailingStop { get { return _model.trailingStop; } }
        //public int OcaGroupId { get { return _model.ocaGroupId; } }

        #region Public Methods and Operators

        public void Update(Price price)
        {
            this._model.update(price);
            this.OnPropertyChanged("Subtitle");
            this.OnPropertyChanged("Content");
        }

        #endregion
    }
}
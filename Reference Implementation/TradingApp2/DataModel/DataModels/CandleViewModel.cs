namespace TradingApp2.TradeLibrary.DataModels
{
    using System;
    using System.Collections.Generic;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    public class CandleViewModel : DataItem
    {
        #region Fields

        private Candle _model;

        #endregion

        #region Constructors and Destructors

        public CandleViewModel(Candle data, DataGroup group)
            : base(group)
        {
            this._model = data;
        }

        #endregion

        #region Public Properties

        public double CloseMid
        {
            get { return this._model.closeMid; }
        }
        public bool Complete
        {
            get { return this._model.complete; }
        }

        public override string Content
        {
            get { return "Time: " + this.Time; }
            set { base.Content = value; }
        }

        public double HighMid
        {
            get { return this._model.highMid; }
        }
        public double LowMid
        {
            get { return this._model.lowMid; }
        }
        public double OpenMid
        {
            get { return this._model.openMid; }
        }
        public override string Subtitle
        {
            get { return this.HighMid + " / " + this.LowMid; }
            set { throw new NotSupportedException(); }
        }
        public string Time
        {
            get { return this._model.time; }
        }
        public override string Title
        {
            get { return this.OpenMid + " / " + this.CloseMid; }
            set { throw new NotSupportedException(); }
        }
        public override string UniqueId
        {
            get { return this.Time; }
            set { throw new NotSupportedException(); }
        }
        public List<double> YValues
        {
            get { return new List<double> { this.OpenMid, this.HighMid, this.LowMid, this.CloseMid }; }
        }

        #endregion

        //public double StopLoss { get { return _model.stopLoss; } }
        //public long Expiry { get { return _model.expiry; } }
        //public double HighLimit { get { return _model.highLimit; } }
        //public double LowLimit { get { return _model.lowLimit; } }
        //public int TrailingStop { get { return _model.trailingStop; } }
        //public int OcaGroupId { get { return _model.ocaGroupId; } }
    }
}
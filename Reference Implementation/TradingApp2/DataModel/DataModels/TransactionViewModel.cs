namespace TradingApp2.TradeLibrary.DataModels
{
    using System;
    using System.Reflection;
    using System.Text;

    using OANDARestLibrary.Framework;
    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    public class TransactionViewModel : DataItem
    {
        #region Fields

        protected Transaction _model;

        #endregion

        #region Constructors and Destructors

        public TransactionViewModel(Transaction data, DataGroup group)
            : base(group)
        {
            this._model = data;
        }

        #endregion

        #region Public Properties

        public int AccountId
        {
            get { return this._model.accountId; }
        }
        public double Balance
        {
            get { return this._model.accountBalance; }
        }

        public override string Content
        {
            get
            {
                // use reflection to display all the properties that have non default values
                var result = new StringBuilder();
                var props = typeof(TransactionViewModel).GetTypeInfo().DeclaredProperties;
                foreach (var prop in props)
                {
                    if (prop.Name != "Content" && prop.Name != "Subtitle" && prop.Name != "Title" && prop.Name != "UniqueId")
                    {
                        var value = prop.GetValue(this);
                        var valueIsNull = value == null;
                        var defaultValue = Common.GetDefault(prop.PropertyType);
                        var defaultValueIsNull = defaultValue == null;
                        if ((valueIsNull != defaultValueIsNull) // one is null when the other isn't
                            || (!valueIsNull && (value.ToString() != defaultValue.ToString()))) // both aren't null, so compare as strings
                        {
                            result.AppendLine(prop.Name + " : " + prop.GetValue(this));
                        }
                    }
                }

                return result.ToString();
            }
            set { base.Content = value; }
        }
        public string Expiry
        {
            get { return this._model.expiry; }
        }

        public long Id
        {
            get { return this._model.id; }
        }
        public string Instrument
        {
            get { return this._model.instrument; }
        }
        public double Interest
        {
            get { return this._model.interest; }
        }
        public double LowerBound
        {
            get { return this._model.lowerBound; }
        }
        public double Price
        {
            get { return this._model.price; }
        }
        public double ProfitLoss
        {
            get { return this._model.pl; }
        }
        public string Reason
        {
            get { return this._model.reason; }
        }
        public string Side
        {
            get { return this._model.side; }
        }
        public double StopLoss
        {
            get { return this._model.stopLossPrice; }
        }
        public override string Subtitle
        {
            get { return "P/L: " + this.ProfitLoss + "\tBalance: " + this.Balance; }
            set { throw new NotSupportedException(); }
        }
        public double TakeProfit
        {
            get { return this._model.takeProfitPrice; }
        }
        public string Time
        {
            get { return this._model.time; }
        }
        public override string Title
        {
            get { return this.Type + " " + this.Instrument + " " + this.Units; }
            set { throw new NotSupportedException(); }
        }
        public double TrailingStop
        {
            get { return this._model.trailingStopLossDistance; }
        }
        public string Type
        {
            get { return this._model.type; }
        }
        public override string UniqueId
        {
            get { return this.Id.ToString(); }
            set { throw new NotSupportedException(); }
        }
        public int Units
        {
            get { return this._model.units; }
        }
        public double UpperBound
        {
            get { return this._model.upperBound; }
        }

        #endregion
    }
}
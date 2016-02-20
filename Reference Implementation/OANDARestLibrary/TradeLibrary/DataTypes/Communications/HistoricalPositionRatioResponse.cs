namespace OANDARestLibrary.TradeLibrary.DataTypes.Communications
{
    using System.Collections.Generic;
    using System.Reflection;

    public class HprData
    {
        #region Fields

        public List<List<string>> data;

        public string label;

        #endregion

        #region Public Methods and Operators

        public List<HistoricalPositionRatio> GetData()
        {
            var result = new List<HistoricalPositionRatio>();
            foreach (var list in this.data)
            {
                var hpr = new HistoricalPositionRatio
                {
                    exchangeRate = double.Parse(list[2]),
                    longPositionRatio = double.Parse(list[1]),
                    timestamp = long.Parse(list[0])
                };
                result.Add(hpr);
            }
            return result;
        }

        #endregion
    }

    public class InnerHprResponse
    {
        #region Fields

        public HprData AUD_JPY;

        public HprData AUD_USD;

        public HprData EUR_AUD;

        public HprData EUR_CHF;

        public HprData EUR_GBP;

        public HprData EUR_JPY;

        public HprData EUR_USD;

        public HprData GBP_CHF;

        public HprData GBP_JPY;

        public HprData GBP_USD;

        public HprData NZD_USD;

        public HprData USD_CAD;

        public HprData USD_CHF;

        public HprData USD_JPY;

        public HprData XAG_USD;

        public HprData XAU_USD;

        #endregion
    }

    public class HistoricalPositionRatio
    {
        #region Fields

        public double exchangeRate;

        public double longPositionRatio;

        public long timestamp;

        #endregion
    }

    public class HistoricalPositionRatioResponse
    {
        #region Fields

        public InnerHprResponse data;

        #endregion

        #region Public Methods and Operators

        public List<HistoricalPositionRatio> GetData()
        {
            // Built in assumption, there's only one HprData in this object (since we can only request data for one instrument at a time)
            foreach (var field in typeof(InnerHprResponse).GetTypeInfo().DeclaredFields)
            {
                var hprData = (HprData)field.GetValue(this.data);
                if (hprData != null)
                {
                    return hprData.GetData();
                }
            }
            return null;
        }

        #endregion
    }
}
﻿namespace OANDARestLibrary.TradeLibrary.DataTypes.Communications.Requests
{
    using System.ComponentModel;

    public class CandlesRequest : Request
    {
        #region Fields

        [IsOptional]
        [DefaultValue(ECandleFormat.bidask)]
        public SmartProperty<ECandleFormat> candleFormat;
        [IsOptional]
        [DefaultValue(500)]
        public SmartProperty<int> count;
        [IsOptional]
        public SmartProperty<string> dailyAlignment;

        [IsOptional]
        public SmartProperty<string> end;
        [IsOptional]
        [DefaultValue(EGranularity.S5)]
        public SmartProperty<EGranularity> granularity;

        [IsOptional]
        //[DefaultValue(true)]
        public SmartProperty<bool> includeFirst;

        public SmartProperty<string> instrument;

        [IsOptional]
        public SmartProperty<string> start;

        [IsOptional]
        public SmartProperty<string> weeklyAlignment;

        #endregion

        #region Public Properties

        public override string EndPoint
        {
            get { return "candles"; }
        }

        #endregion

        #region Public Methods and Operators

        public override EServer GetServer()
        {
            return EServer.Rates;
        }

        #endregion
    }

    public enum ECandleFormat
    {
        bidask,

        midpoint
    }

    public enum EGranularity
    {
        S5,

        S10,

        S15,

        S30,

        M1,

        M2,

        M3,

        M5,

        M10,

        M15,

        M30,

        H1,

        H2,

        H3,

        H4,

        H6,

        H8,

        H12,

        D,

        W,

        M
    }
}
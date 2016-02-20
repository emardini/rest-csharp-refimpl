namespace TradingApp2.DataModel.DataModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OANDARestLibrary;

    using TradingApp2.Data;

    /// <summary>
    ///     Storace for helper data to help make playing with requests more effective
    /// </summary>
    internal class RequestHelpers
    {
        #region Static Fields

        public static long lastOrderId = 0;

        public static long lastTradeId = 0;

        #endregion
    }

    internal class OrderPostViewModel : RequestViewModel
    {
        #region Constructors and Destructors

        public OrderPostViewModel(string name, DataGroup group, Dictionary<string, string> parameters)
            : base(name, group)
        {
            this.requestParams = parameters;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task MakeRequest()
        {
            await this.InternalMakeRequest(async () =>
            {
                var result = await Rest.PostOrderAsync(AccountDataSource.DefaultDataSource.Id, this.requestParams);
                if (result.tradeOpened != null)
                {
                    RequestHelpers.lastTradeId = result.tradeOpened.id;
                }
                if (result.orderOpened != null)
                {
                    RequestHelpers.lastOrderId = result.orderOpened.id;
                }
                return result;
            });
        }

        #endregion
    }

    internal class OrderPatchViewModel : RequestViewModel
    {
        #region Constructors and Destructors

        public OrderPatchViewModel(string name, DataGroup group, Dictionary<string, string> parameters)
            : base(name, group)
        {
            this.requestParams = parameters;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task MakeRequest()
        {
            await
                this.InternalMakeRequest(
                    () => Rest.PatchOrderAsync(AccountDataSource.DefaultDataSource.Id, RequestHelpers.lastOrderId, this.requestParams));
        }

        #endregion
    }

    internal class OrderDeleteViewModel : RequestViewModel
    {
        #region Constructors and Destructors

        public OrderDeleteViewModel(string name, DataGroup group)
            : base(name, group)
        {
            this.requestParams = new Dictionary<string, string>();
        }

        #endregion

        #region Public Methods and Operators

        public override async Task MakeRequest()
        {
            await this.InternalMakeRequest(() => Rest.DeleteOrderAsync(AccountDataSource.DefaultDataSource.Id, RequestHelpers.lastOrderId));
        }

        #endregion
    }

    internal class TradePatchViewModel : RequestViewModel
    {
        #region Constructors and Destructors

        public TradePatchViewModel(string name, DataGroup group, Dictionary<string, string> parameters)
            : base(name, group)
        {
            this.requestParams = parameters;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task MakeRequest()
        {
            await
                this.InternalMakeRequest(
                    () => Rest.PatchTradeAsync(AccountDataSource.DefaultDataSource.Id, RequestHelpers.lastTradeId, this.requestParams));
        }

        #endregion
    }

    internal class TradeDeleteViewModel : RequestViewModel
    {
        #region Constructors and Destructors

        public TradeDeleteViewModel(string name, DataGroup group)
            : base(name, group)
        {
            this.requestParams = new Dictionary<string, string>();
        }

        #endregion

        #region Public Methods and Operators

        public override async Task MakeRequest()
        {
            await this.InternalMakeRequest(() => Rest.DeleteTradeAsync(AccountDataSource.DefaultDataSource.Id, RequestHelpers.lastTradeId));
        }

        #endregion
    }

    internal class PositionDeleteViewModel : RequestViewModel
    {
        #region Constructors and Destructors

        public PositionDeleteViewModel(string name, DataGroup group, string instrument)
            : base(name, group)
        {
            this.requestParams = new Dictionary<string, string> { { "instrument", instrument } };
        }

        #endregion

        #region Public Methods and Operators

        public override async Task MakeRequest()
        {
            await this.InternalMakeRequest(() => Rest.DeletePositionAsync(AccountDataSource.DefaultDataSource.Id, this.requestParams["instrument"]));
        }

        #endregion
    }
}
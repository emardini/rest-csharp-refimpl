namespace OANDARestLibrary.TradeLibrary
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    public class RatesSession : StreamSession<RateStreamResponse>
    {
        #region Fields

        private readonly List<Instrument> _instruments;

        #endregion

        #region Constructors and Destructors

        public RatesSession(int accountId, List<Instrument> instruments) : base(accountId)
        {
            this._instruments = instruments;
        }

        #endregion

        #region Methods

        protected override async Task<WebResponse> GetSession()
        {
            return await Rest.StartRatesSession(this._instruments, this._accountId);
        }

        #endregion
    }
}
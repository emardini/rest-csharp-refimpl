namespace OANDARestLibrary.TradeLibrary
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    public class EventsSession : StreamSession<Event>
    {
        #region Constructors and Destructors

        public EventsSession(int accountId) : base(accountId)
        {
        }

        #endregion

        #region Methods

        protected override async Task<WebResponse> GetSession()
        {
            return await Rest.StartEventsSession(new List<int> { this._accountId });
        }

        #endregion
    }
}
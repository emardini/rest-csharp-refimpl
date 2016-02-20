namespace OANDARestLibrary
{
    using System.Collections.Generic;

    public enum EServer
    {
        Account,

        Rates,

        StreamingRates,

        StreamingEvents,

        Labs
    }

    public enum EEnvironment
    {
        Sandbox,

        Practice,

        Trade
    }

    public class Credentials
    {
        #region Static Fields

        private static readonly Dictionary<EEnvironment, Dictionary<EServer, string>> Servers = new Dictionary
            <EEnvironment, Dictionary<EServer, string>>
        {
            {
                EEnvironment.Sandbox, new Dictionary<EServer, string>
                {
                    { EServer.Account, "http://api-sandbox.oanda.com/v1/" },
                    { EServer.Rates, "http://api-sandbox.oanda.com/v1/" },
                    { EServer.StreamingRates, "http://stream-sandbox.oanda.com/v1/" },
                    { EServer.StreamingEvents, "http://stream-sandbox.oanda.com/v1/" },
                }
            },
            {
                EEnvironment.Practice, new Dictionary<EServer, string>
                {
                    { EServer.StreamingRates, "https://stream-fxpractice.oanda.com/v1/" },
                    { EServer.StreamingEvents, "https://stream-fxpractice.oanda.com/v1/" },
                    { EServer.Account, "https://api-fxpractice.oanda.com/v1/" },
                    { EServer.Rates, "https://api-fxpractice.oanda.com/v1/" },
                    { EServer.Labs, "https://api-fxpractice.oanda.com/labs/v1/" },
                }
            },
            {
                EEnvironment.Trade, new Dictionary<EServer, string>
                {
                    { EServer.StreamingRates, "https://stream-fxtrade.oanda.com/v1/" },
                    { EServer.StreamingEvents, "https://stream-fxtrade.oanda.com/v1/" },
                    { EServer.Account, "https://api-fxtrade.oanda.com/v1/" },
                    { EServer.Rates, "https://api-fxtrade.oanda.com/v1/" },
                    { EServer.Labs, "https://api-fxtrade.oanda.com/labs/v1/" },
                }
            }
        };

        private static Credentials _instance;

        #endregion

        #region Fields

        public string AccessToken;

        public int DefaultAccountId;

        public EEnvironment Environment;

        public string Username;

        #endregion

        #region Public Properties

        public bool IsSandbox
        {
            get { return this.Environment == EEnvironment.Sandbox; }
        }

        #endregion

        #region Public Methods and Operators

        public static Credentials GetDefaultCredentials()
        {
            return _instance ?? (_instance = GetPracticeCredentials());
        }

        public static void SetCredentials(EEnvironment environment, string accessToken, int defaultAccount = 0)
        {
            _instance = new Credentials
            {
                Environment = environment,
                AccessToken = accessToken,
                DefaultAccountId = defaultAccount
            };
        }

        public string GetServer(EServer server)
        {
            if (this.HasServer(server))
            {
                return Servers[this.Environment][server];
            }
            return null;
        }

        public bool HasServer(EServer server)
        {
            return Servers[this.Environment].ContainsKey(server);
        }

        #endregion

        #region Methods

        private static Credentials GetLiveCredentials()
        {
            // You'll need to add your own accessToken and account if desired
            return new Credentials
            {
                //defaultAccountId = 00000,
                //accessToken = "fhaishihfweaiuu2u892h829h829h92ha8rfa89",
                Environment = EEnvironment.Trade
            };
        }

        private static Credentials GetPracticeCredentials()
        {
            return new Credentials
            {
                DefaultAccountId = 5027596,
                Environment = EEnvironment.Practice,
                AccessToken = "e7bd9d8f8e943bd909a3a9abb4a842d1-f8a3c35d1262ddd392b6a4b589e0507c",
            };
        }

        private static Credentials GetSandboxCredentials()
        {
            return new Credentials
            {
                Environment = EEnvironment.Sandbox,
            };
        }

        #endregion
    }
}
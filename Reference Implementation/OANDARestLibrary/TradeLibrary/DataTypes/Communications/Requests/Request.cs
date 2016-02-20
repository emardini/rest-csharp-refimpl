namespace OANDARestLibrary.TradeLibrary.DataTypes.Communications.Requests
{
    using System.Reflection;
    using System.Text;

    public interface ISmartProperty
    {
        #region Public Properties

        bool HasValue { get; set; }

        #endregion

        #region Public Methods and Operators

        void SetValue(object obj);

        #endregion
    }

    // Functionally very similar to System.Nullable, could possibly just replace this
    public struct SmartProperty<T> : ISmartProperty
    {
        #region Fields

        private T _value;

        #endregion

        #region Public Properties

        public bool HasValue { get; set; }

        public T Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                this.HasValue = true;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static implicit operator SmartProperty<T>(T value)
        {
            return new SmartProperty<T> { Value = value };
        }

        public static implicit operator T(SmartProperty<T> value)
        {
            return value._value;
        }

        public void SetValue(object obj)
        {
            this.SetValue((T)obj);
        }

        public void SetValue(T value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            // This is ugly, but c'est la vie for now
            if (this._value is bool)
            {
                // bool values need to be lower case to be parsed correctly
                return this._value.ToString().ToLower();
            }
            return this._value.ToString();
        }

        #endregion
    }

    public abstract class Request
    {
        #region Public Properties

        public abstract string EndPoint { get; }

        #endregion

        #region Public Methods and Operators

        public string GetRequestString()
        {
            var result = new StringBuilder();
            result.Append(this.EndPoint);
            var firstJoin = true;
            foreach (var declaredField in this.GetType().GetTypeInfo().DeclaredFields)
            {
                var prop = declaredField.GetValue(this);
                var smartProp = prop as ISmartProperty;
                if (smartProp != null && smartProp.HasValue)
                {
                    if (firstJoin)
                    {
                        result.Append("?");
                        firstJoin = false;
                    }
                    else
                    {
                        result.Append("&");
                    }

                    result.Append(declaredField.Name + "=" + prop);
                }
            }
            return result.ToString();
        }

        public abstract EServer GetServer();

        #endregion
    }
}
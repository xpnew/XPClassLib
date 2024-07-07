using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win.HttpWeb
{
    public class HttpEncoder
    {
        private static HttpEncoder _customEncoder;
        private readonly bool _isDefaultEncoder;

        private static readonly Lazy<HttpEncoder> _customEncoderResolver =
            new Lazy<HttpEncoder>(GetCustomEncoderFromConfig);

        public static HttpEncoder Current
        {
            get
            {
                // always use the fallback encoder when rendering an error page so that we can at least display *something*
                // to the user before closing the connection

                HttpContext httpContext = HttpContext.Current;
                if (httpContext != null && httpContext.DisableCustomHttpEncoder)
                {
                    return _defaultEncoder;
                }
                else
                {
                    if (_customEncoder == null)
                    {
                        _customEncoder = _customEncoderResolver.Value;
                    }
                    return _customEncoder;
                }
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _customEncoder = value;
            }
        }
        public static HttpEncoder Default
        {
            get
            {
                return _defaultEncoder;
            }
        }


    }
}

using System.Collections.Specialized;
using System.Web;

namespace ApiManager
{
    public abstract class ARequest
    {
        protected const string _QUERY_PREFIX = @"?";
        protected const string _DELIMETER = @"/";

        public AManager.ResponseFormats ResponseFormat { get; set; }
        public readonly NameValueCollection Query;

        /// <summary>
        /// Abstract class that stores the request data
        /// </summary>
        /// <param name="responseFormat"></param>
        protected ARequest(AManager.ResponseFormats responseFormat)
        {
            ResponseFormat = responseFormat;
            Query = HttpUtility.ParseQueryString(string.Empty);
        }

        /// <summary>
        /// Clears the API request data
        /// </summary>
        public virtual void Clear() { Query.Clear(); }

        /// <summary>
        /// Returns a string representation of the API request
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Query.ToString(); }
    }
}

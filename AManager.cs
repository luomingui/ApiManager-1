using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiManager
{
    public class AManager
    {
        #region Constants & Enums
        protected const string _MEDIA_TYPE = "application/";
        public enum ResponseFormats { xml = 0, json = 1 }
        #endregion

        #region Protected Properties
        protected readonly Uri _DefaultBaseUri;
        protected HttpClient _ApiClient;
        #endregion

        public AManager(Uri defaultBaseUri)
        {
            _DefaultBaseUri = defaultBaseUri;
            _ApiClient = new HttpClient(new WebRequestHandler { AllowAutoRedirect = false })
                {
                    BaseAddress = _DefaultBaseUri
                };
        }

        /// <summary>
        /// Sends an HttpRequest and returns the response string.
        /// Handles redirect responses by updating the httpclient's header.location
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="requestString"></param>
        /// <param name="responseFormat"></param>
        /// <returns></returns>
        public static async Task<string> GetResponseStringAsync(HttpClient httpClient, string requestString, ResponseFormats responseFormat)
        {
            bool resendRequest;
            HttpResponseMessage responseMessage;
            
            do
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_MEDIA_TYPE + responseFormat));
                
                resendRequest = false;
                responseMessage = httpClient.GetAsync(requestString).Result;
                switch (responseMessage.StatusCode)
                {
                    case HttpStatusCode.Redirect:
                        var newBaseUri = responseMessage.Headers.Location.ToString().Replace(requestString, string.Empty);
                        httpClient = new HttpClient(new WebRequestHandler {AllowAutoRedirect = false})
                            {
                                BaseAddress = new Uri(newBaseUri)
                            };
                        resendRequest = true;
                        break;
                }
            } while (resendRequest);

            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}

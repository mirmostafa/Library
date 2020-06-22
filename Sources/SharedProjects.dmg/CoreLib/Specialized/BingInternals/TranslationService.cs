using System;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;

namespace Mohammad.Specialized.Bing
{
    public class TranslationService
    {
        private readonly string _ClientId;
        private readonly string _ClientSecret;

        public TranslationService(string clientId = "hamed324210", string clientSecret = "TCI30W5vO6E9C8sR9c/2sZlqfXVOzqhch0fag6+VjVU=")
        {
            this._ClientId = clientId;
            this._ClientSecret = clientSecret;
        }

        public string Translate(string text, string from, string to)
        {
            LibrarySupervisor.Logger.Debug($"Translating {text} from {from}, to {to}");
            var auth = new AdmAuthentication(this._ClientId, this._ClientSecret);
            var uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
            LibrarySupervisor.Logger.Debug("Getting access token");
            var authToken = "Bearer" + " " + auth.GetAccessToken().access_token;

            var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);

            try
            {
                LibrarySupervisor.Logger.Debug("Getting response");
                var response = httpWebRequest.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    var dcs = new DataContractSerializer(Type.GetType("System.String"));
                    var translation = (string) dcs.ReadObject(stream);
                    LibrarySupervisor.Logger.Debug($"Translation text is: {translation}");
                    return translation;
                }
            }
            catch (Exception ex)
            {
                LibrarySupervisor.Logger.Exception("Exception occurred while translating", ex);
                return string.Empty;
            }
        }
    }

    [DataContract]
    internal class AdmAccessToken
    {
        [DataMember]
        internal string access_token { get; set; }

        [DataMember]
        internal string token_type { get; set; }

        [DataMember]
        internal string expires_in { get; set; }

        [DataMember]
        internal string scope { get; set; }
    }

    internal class AdmAuthentication
    {
        private readonly Timer accessTokenRenewer;
        private readonly string clientId;
        private readonly string request;
        private AdmAccessToken token;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;
        internal static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";

        internal AdmAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            //If clientid or client secret has special characters, encode before sending request
            this.request =
                $"grant_type=client_credentials&client_id={HttpUtility.UrlEncode(clientId)}&client_secret={HttpUtility.UrlEncode(clientSecret)}&scope=http://api.microsofttranslator.com";
            this.token = HttpPost(DatamarketAccessUri, this.request);
            //renew the token every specfied minutes
            this.accessTokenRenewer = new Timer(this.OnTokenExpiredCallback, this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        internal AdmAccessToken GetAccessToken() { return this.token; }

        private void RenewAccessToken()
        {
            var newAccessToken = HttpPost(DatamarketAccessUri, this.request);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            this.token = newAccessToken;
            Console.WriteLine("Renewed token for user: {0} is: {1}", this.clientId, this.token.access_token);
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                this.RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed renewing access token. Details: {0}", ex.Message);
            }
            finally
            {
                try
                {
                    this.accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message);
                }
            }
        }

        private static AdmAccessToken HttpPost(string datamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            var webRequest = WebRequest.Create(datamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            var bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (var outputStream = webRequest.GetRequestStream())
                outputStream.Write(bytes, 0, bytes.Length);
            using (var webResponse = webRequest.GetResponse())
            {
                var serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                var token = (AdmAccessToken) serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using System.Net.Http;
//using System.Threading.Tasks;
//using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

//namespace JWLibrary.StaticMethod
//{
//    public static class JHttpClient
//    {
//        public static async Task<string> ToGetAsync(this IHttpClientFactory clientFactory, HttpRequestMessage request)
//        {
//            var client = clientFactory.CreaetClient();

//            var response = await client.SendAsync(request);

//            if (response.IsSuccessStatusCode)
//            {
//                var responseString = await response.Content.ReadAsStringAsync();   
//                return  responseString;             
//            }
//            else
//            {
//                throw new HttpRequestException("response is not success : " + response.StatusCode.ToString());
//            }
//        }
//    }

//}
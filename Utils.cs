using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;

namespace IntegracionDesarrollo3
{
    public class Utils
    {
        public static void RequestNeedsAuthentication(HttpRequest Request, HttpClient _http)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web
{
    public class CookieManager
    {
        public static string GetCookie(HttpRequest request, string cookieName)
        {
            return request?.Cookies[cookieName];
        }
    }
}

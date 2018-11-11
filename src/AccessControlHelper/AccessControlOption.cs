using System;
using Microsoft.AspNetCore.Http;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public class AccessControlOption
    {
        public string AccessHeaderKey { get; set; } = "X-Access-Key";

        public Action<HttpResponse> DefaultUnauthorizedOperation { get; set; }
    }
}

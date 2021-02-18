using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScriptingLib.WebAccess
{
    public class WebProvider
    {
        public HttpClient Client { get; private set; }
        public WebProvider()
        {
            Client = new HttpClient();
        }

        public HttpResponseMessage Get(string url) =>  Client.GetAsync(url).Result;
        public HttpResponseMessage Post(string url,string content) => 
                          Client.PostAsync(url, new StringContent(content)).Result;

    }
}

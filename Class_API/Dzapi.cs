using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiZhuaChart.NET.Class_API
{
    public class Dzapi
    {
        public string JWT { get; }
        public Dzapi(string token)
        {
            this.JWT = token;
        }

        public List<Party> GetPartiesInfo()
        {
            string url = "https://apis-ff.zaih.com/flash-whisper/v2/applications?filter=all&page=1&per_page=100";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ios dizhuaApp 1.9.4");
            client.DefaultRequestHeaders.Add("Host", "apis-ff.zaih.com");
            client.DefaultRequestHeaders.Add("Authorization", String.Format("JWT {0}", JWT));
            var response = client.GetAsync(url).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Party>>(json);
        }

    }
}

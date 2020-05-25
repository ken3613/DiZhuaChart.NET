using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiZhuaChart.NET.Class_API
{
    public class Dzapi
    {
        public string JWT { get; }
        public Dzapi(string token)
        {
            this.JWT = token;
        }

        /// <summary>
        /// 获取参加过的聚会列表
        /// </summary>
        /// <returns>参加过的聚会列表</returns>
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

        public Hashtable GetUserInfo()
        {
            Hashtable ht = new Hashtable();
            string url = "https://apis-ff.zaih.com/flash-whisper/v2/accounts";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ios dizhuaApp 1.9.4");
            client.DefaultRequestHeaders.Add("Host", "apis-ff.zaih.com");
            client.DefaultRequestHeaders.Add("Authorization", String.Format("JWT {0}", JWT));
            var response = client.GetAsync(url).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var user_json = JsonConvert.DeserializeObject<JObject>(json);
            ht.Add("nickname", user_json["nickname"].ToString());
            ht.Add("avatar", user_json["avatar"].ToString());
            ht.Add("topic_exp_info", user_json["topic_exp_info"].ToString());
            ht.Add("total_gift", user_json["total_gift"].ToString());
            ht.Add("uid", user_json["uid"].ToString());
            ht.Add("enable_parlor", user_json["enable_parlor"].ToObject<bool>());
            return ht;
        }

        /// <summary>
        /// 获取该聚会的成员列表
        /// </summary>
        /// <param name="partyID">聚会ID</param>
        /// <returns>该聚会的成员列表</returns>
        public List<PartyMember> GetPartyMembers(string partyID)
        {
            string url = string.Format("https://apis-ff.zaih.com/flash-whisper/v2/application/{0}/room_detail", partyID);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ios dizhuaApp 1.9.4");
            client.DefaultRequestHeaders.Add("Host", "apis-ff.zaih.com");
            client.DefaultRequestHeaders.Add("Authorization", String.Format("JWT {0}", JWT));
            List<PartyMember> members_list = new List<PartyMember>();
            var response = client.GetAsync(url).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var members = JsonConvert.DeserializeObject<JObject>(json)["members"].Children().ToList();
            foreach(var member in members)
            {
                PartyMember mb = member.ToObject<PartyMember>();
                members_list.Add(mb);
            }
            return members_list;
        }

        /// <summary>
        /// 获取爪友列表
        /// </summary>
        /// <returns>爪友列表</returns>
        public List<Friend> GetRelationships()
        {
            string url = "https://apis-ff.zaih.com/flash-whisper/v3/relationships/all";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ios dizhuaApp 1.9.4");
            client.DefaultRequestHeaders.Add("Host", "apis-ff.zaih.com");
            client.DefaultRequestHeaders.Add("Authorization", String.Format("JWT {0}", JWT));
            List<Friend> friends_list = new List<Friend>();
            var response = client.GetAsync(url).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var friends = JsonConvert.DeserializeObject<JArray>(json);
            foreach(var friend in friends)
            {
                var fd = new Friend();
                fd.close_grade = friend["close_grade"].ToObject<float>();
                fd.user_id = friend["to_user"]["user_id"].ToString();
                fd.nickname = friend["to_user"]["nickname"].ToString();
                friends_list.Add(fd);
            }
            return friends_list;
        }

        /// <summary>
        /// 请求验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>是否发送成功</returns>
        public static bool Verification(string mobile)
        {
            string url = "https://apis-ff.zaih.com/flash-auth/v2/mobile/verification";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ios dizhuaApp 1.9.4");
            client.DefaultRequestHeaders.Add("Host", "apis-ff.zaih.com");
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW9zOmtvcWVyMjFvaGFmZG8xNDA5YXNkZmU=");
            var requestClass = new
            {
                mobile = mobile
            };
            string requestJson = JsonConvert.SerializeObject(requestClass);
            HttpContent requestContent = new StringContent(requestJson);
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync(url, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 请求登录
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns>JWT Token</returns>
        public static string Login(string mobile, string code)
        {
            string url = "https://apis-ff.zaih.com/flash-auth/v2/oauth/jwt";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ios dizhuaApp 1.9.4");
            client.DefaultRequestHeaders.Add("Host", "apis-ff.zaih.com");
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW9zOmtvcWVyMjFvaGFmZG8xNDA5YXNkZmU=");
            var requestClass = new
            {
                username = mobile,
                password = code,
                auth_approach = "mobile",
                grant_type = "password"
            };
            string requestJson = JsonConvert.SerializeObject(requestClass);
            HttpContent requestContent = new StringContent(requestJson);
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync(url, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseJson = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<JObject>(responseJson)["access_token"].ToString();
            }
            else
            {
                return null;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Musli_MultiTool
{
    
    class MusliAPI
    {
        protected const string API_URL = "https://clicker.vitiacat.xyz/api/";
        private int userid;
        private string token;
        private int vk_ts;
        public MusliAPI(int userid, string token, int vk_ts)
        {
            this.userid = userid;
            this.token = token;
            this.vk_ts = vk_ts;
        }

        protected async Task<string> GetRequest(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task<int> GetMusliAsync()
        {
            string UserInfo = await GetRequest(API_URL + $"user.php?vk_access_token_settings=&vk_app_id=8013734&vk_are_notifications_enabled=0&vk_is_app_user=0&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=catalog_recent&vk_ts={vk_ts}&vk_user_id={userid}&sign={token}");
            using (JsonDocument responce = JsonDocument.Parse(UserInfo))
            {
                JsonElement User = responce.RootElement;
                return User.GetProperty("muesli").GetInt32();
            }
        }

        public async Task<double> GetMoneyAsync()
        {
            string UserInfo = await GetRequest(API_URL + $"user.php?vk_access_token_settings=&vk_app_id=8013734&vk_are_notifications_enabled=0&vk_is_app_user=0&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=catalog_recent&vk_ts={vk_ts}&vk_user_id={userid}&sign={token}");
            using (JsonDocument responce = JsonDocument.Parse(UserInfo))
            {
                JsonElement User = responce.RootElement;
                return User.GetProperty("money").GetDouble();
            }
        }

        public async Task<int> Click()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + $"click.php?vk_access_token_settings=&vk_app_id=8013734&vk_are_notifications_enabled=0&vk_is_app_user=0&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=catalog_recent&vk_ts={vk_ts}&vk_user_id={userid}&sign={token}");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if ((int)response.StatusCode == 429) return 0;
                if ((int)response.StatusCode == 500) return -1;
                if ((int)response.StatusCode == 403) return -1;
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    await reader.ReadToEndAsync();
                    return 1;
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cj.Http
{
    class HttpService
    {
        public static byte[] SendGetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {

                request.Method = "get";
                /*
                if (!string.IsNullOrEmpty(uid))
                {
                    CookieContainer _cookiesContainer = CookiesContainerDic[uid];

                    if (_cookiesContainer == null)
                    {
                        _cookiesContainer = new CookieContainer();
                    }
                    if (!CookiesContainerDic.ContainsKey(uid))
                    {
                        CookiesContainerDic.Add(uid, _cookiesContainer);
                    }

                    request.CookieContainer = _cookiesContainer;  //启用cookie
                }
                */
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("协议冲突"))
                {
                    return null;
                }
                else
                {
                    MessageBox.Show("SendGetRequest" + ex.Message);
                }

                return null;
            }
            finally
            {
                request.Abort();
            }
        }
        /// <summary>
         /// get请求，并返回cookie
         /// </summary>
         /// <param name="url"></param>
         /// <param name="cookieContainer"></param>
         /// <returns></returns>
        public static byte[] SendGetRequest(string url, ref CookieContainer cookieContainer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {

                request.Method = "get";
                request.CookieContainer = cookieContainer;  //启用cookie               

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendGetRequest Cook" + ex.Message);
                return null;
            }
            finally
            {
                request.Abort();
            }
        }
    }
}

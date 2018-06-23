using cj.Http.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cj.Http
{
    class LoginService
    {
        public string GetSidUid(string login_redirect)
        {
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                byte[] bytes = HttpService.SendGetRequest(login_redirect + "&fun=new&version=v2&lang=zh_CN", ref cookieContainer);
                string pass_ticket = Encoding.UTF8.GetString(bytes);
                string url = login_redirect;
                Uri uri = new Uri(url);
                string WXUser_url = (uri.Host);
                string pass_Ticket = pass_ticket.Split(new string[] { "pass_ticket" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
                string sKey = pass_ticket.Split(new string[] { "skey" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
                string wxSid = pass_ticket.Split(new string[] { "wxsid" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
                string wxUin = pass_ticket.Split(new string[] { "wxuin" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');


                var passticketEntity = new PassTicketEntity()
                {
                    PassTicket = pass_Ticket,
                    SKey = sKey,
                    WxSid = wxSid,
                    WxUin = wxUin,
                    WxHost = WXUser_url
                };
/*
                LoginCore.PassTicket(wxUin, passticketEntity);
                if (HttpService.CookiesContainerDic.ContainsKey(wxUin))
                {
                    HttpService.CookiesContainerDic.Remove(wxUin);
                }
                WxSerializable s = new WxSerializable(wxUin, EnumContainer.SerializType.cookie);
                HttpService.CookiesContainerDic.Add(wxUin, cookieContainer);
                s.Serializable(HttpService.CookiesContainerDic);
                */
                return wxUin;
            }
            catch
            {
                return null;
            }

        }
    }
}

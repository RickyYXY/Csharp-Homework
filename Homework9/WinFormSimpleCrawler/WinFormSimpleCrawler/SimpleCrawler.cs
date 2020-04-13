using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace WinFormSimpleCrawler
{
    public delegate void DownloadHandler(string sender, DownloadEventArgs args);
    public class DownloadEventArgs { };

    class SimpleCrawler
    {
        private Hashtable urls; //网页记录表
        private int count; //网页数量
        private DownloadEventArgs args = new DownloadEventArgs();
        public int UrlNum { get; set; }
        
        public event DownloadHandler PageDownloaded;
        //string Starturl { get; set; }

        //static void Main(string[] args)
        //{
        //    SimpleCrawler myCrawler = new SimpleCrawler();
        //    string startUrl = "http://www.cnblogs.com/dstang2000/";
        //    if (args.Length >= 1) startUrl = args[0];
        //    myCrawler.urls.Add(startUrl, false);//加入初始页面
        //    new Thread(myCrawler.Crawl).Start();
        //}
        public SimpleCrawler()
        {
           count = 0;
           urls = new Hashtable();
        }

        public void Clear()
        {
            urls.Clear();
            count = 0;
            UrlNum = 0;
        }

        public void AddStartUrl(string url)
        {
            urls.Add(url, false);//加入初始页面
        }

        public void Crawl()
        {
            //Console.WriteLine("开始爬行了.... ");
            PageDownloaded("开始爬行了.... ", args);
            while (true)
            {
                string current = null;
                foreach (string url in urls.Keys)
                {
                    if ((bool)urls[url]) continue;
                    current = url;
                }

                if (current == null || count > UrlNum) break;
                //Console.WriteLine("爬行" + current + "页面!");
                PageDownloaded("爬行" + current + "页面!", args);
                string html = DownLoad(current); // 下载
                urls[current] = true;
                count++;
                Parse(html, current);//解析,并加入新的链接
                //Console.WriteLine("爬行结束");
                PageDownloaded("爬行结束", args);
            }
            PageDownloaded("爬取完成!", args);
        }

        public string DownLoad(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string html = webClient.DownloadString(url);
                string fileName = count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
                return html;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                PageDownloaded(ex.Message, args);
                return "";
            }
        }

        private void Parse(string html, string current)
        {
            string strRef = @"(href|HREF)[]*=[]*[""'][^""'#>]+[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            string pattern1 = @"(/.+[.]html$|/$)";
            string pattern2 = @"https://.+[.]com/";
            Regex regex1 = new Regex(pattern1);
            Regex regex2 = new Regex(pattern2);
            string temp = current;

            foreach (Match match in matches)
            {
                strRef = match.Value.Substring(match.Value.IndexOf('=') + 1)
                          .Trim('"', '\"', '#', '>');
                if (strRef.Length == 0) continue;

                if (!Regex.IsMatch(strRef, @"^https"))//相对地址处理
                {
                    if (Regex.IsMatch(strRef, @"^/"))
                    {
                        temp = regex1.Replace(temp, ""); //current处理，去掉末尾的html
                        strRef = temp + strRef;
                    }
                    else
                    {
                        temp = regex2.Match(temp).ToString();
                        strRef = temp + strRef;
                    }
                }

                if (!Regex.IsMatch(strRef, regex2.Match(current).ToString())) continue;
                if (!Regex.IsMatch(strRef, @"html$")) continue;
                if (urls[strRef] == null) urls[strRef] = false;
            }
        }
    }

}

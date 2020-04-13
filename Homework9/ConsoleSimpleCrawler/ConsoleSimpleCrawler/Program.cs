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

namespace ConsoleSimpleCrawler
{
    class ConsoleSimpleCrawler
    {
        private Hashtable urls = new Hashtable(); //网页记录表
        private int count = 0; //网页数量

        static void Main(string[] args)
        {
            ConsoleSimpleCrawler myCrawler = new ConsoleSimpleCrawler();
            string startUrl = "http://www.cnblogs.com/dstang2000/";
            if (args.Length >= 1) startUrl = args[0];
            myCrawler.urls.Add(startUrl, false);//加入初始页面
            new Thread(myCrawler.Crawl).Start();
        }

        private void Crawl()
        {
            Console.WriteLine("开始爬行了.... ");
            while (true)
            {
                string current = null;
                foreach (string url in urls.Keys)
                {
                    if ((bool)urls[url]) continue;
                    current = url;
                }

                if (current == null || count > 10) break;
                Console.WriteLine("爬行" + current + "页面!");
                string html = DownLoad(current); // 下载
                urls[current] = true;
                count++;
                Parse(html,current);//解析,并加入新的链接
                Console.WriteLine("爬行结束");
            }
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
                Console.WriteLine(ex.Message);
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
                
                if(!Regex.IsMatch(strRef, @"^https"))//相对地址处理
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

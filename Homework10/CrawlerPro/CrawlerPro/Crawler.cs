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
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace CrawlerForm
{
    class Crawler
    {
        public event Action<Crawler> CrawlerStopped;
        public event Action<Crawler, string, string> PageDownloaded;

        //所有已下载和待下载URL，key是URL，value表示是否下载成功
        private ConcurrentDictionary<string, bool> urls = new ConcurrentDictionary<string, bool>();

        //待下载队列
        private ConcurrentQueue<string> pending = new ConcurrentQueue<string>();

        //URL检测表达式，用于在HTML文本中查找URL
        private readonly string urlDetectRegex = @"(href|HREF)[]*=[]*[""'](?<url>[^""'#>]+)[""']";

        //URL解析表达式
        public static readonly string urlParseRegex = @"^(?<site>https?://(?<host>[\w.-]+)(:\d+)?($|/))(\w+/)*(?<file>[^#?]*)";
        public string HostFilter { get; set; } //主机过滤规则
        public string FileFilter { get; set; } //文件过滤规则
        public int MaxPage { get; set; } //最大下载数量
        public string StartURL { get; set; } //起始网址
        public Encoding HtmlEncoding { get; set; } //网页编码
        public ConcurrentDictionary<string, bool> DownloadedPages { get => urls; } //已下载网页

        //锁对象
        private string gridLock = "gridLock";

        public Crawler()
        {
            MaxPage = 100;
            HtmlEncoding = Encoding.UTF8;
        }

        public void Start()
        {
            urls.Clear();
            pending = new ConcurrentQueue<string>();
            pending.Enqueue(StartURL);
            List<Task> tasks = new List<Task>();
            do
            {
                tasks.RemoveAll(task => task.IsCompleted || task.IsFaulted);
                pending.TryDequeue(out string url);
                try
                {
                    if (url != null)
                    {
                        Task task = Task.Run(() => ConcurrentDownLoad(url));
                        tasks.Add(task);
                    }
                }
                catch (AggregateException ex)
                {
                    foreach (Exception inner in ex.InnerExceptions)
                    {
                        Console.WriteLine(
                            "Exception type {0} from {1}",
                            inner.GetType(), inner.Source);
                    }
                }
            } while (tasks.Count > 0);
            CrawlerStopped(this);
        }

        private void ConcurrentDownLoad(string url)
        { 
            if (urls.Count < this.MaxPage)
            {
                string html = DownLoad(url); // 下载
                urls[url] = true;
                lock (gridLock)
                {
                    PageDownloaded(this, url, "success");
                }
                Parse(html, url);//解析,并加入新的链接
            }
        }

        private string DownLoad(string url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string html = webClient.DownloadString(url);
            string fileName;
            lock(urls)
            {
                fileName = urls.Count.ToString();
                File.WriteAllText(fileName, html, Encoding.UTF8);
            }
            return html;
        }

        private void Parse(string html, string pageUrl)
        {
            var matches = new Regex(urlDetectRegex).Matches(html);
            foreach (Match match in matches)
            {
                string linkUrl = match.Groups["url"].Value;
                if (linkUrl == null || linkUrl == "") continue;
                linkUrl = FixUrl(linkUrl, pageUrl);//转绝对路径
                                                   //解析出host和file两个部分，进行过滤
                Match linkUrlMatch = Regex.Match(linkUrl, urlParseRegex);
                string host = linkUrlMatch.Groups["host"].Value;
                string file = linkUrlMatch.Groups["file"].Value;
                if (file == "") file = "index.html";
                if (Regex.IsMatch(host, HostFilter) && Regex.IsMatch(file, FileFilter))
                {
                    if (!urls.ContainsKey(linkUrl))
                    {
                        pending.Enqueue(linkUrl);
                        urls.TryAdd(linkUrl, false);
                    }
                }
            }
        }


        //将相对路径转为绝对路径
        static private string FixUrl(string url, string pageUrl)
        {
            if (url.Contains("://"))
            {
                return url;
            }
            if (url.StartsWith("//"))
            {
                return "http:" + url;
            }
            if (url.StartsWith("/"))
            {
                Match urlMatch = Regex.Match(pageUrl, urlParseRegex);
                String site = urlMatch.Groups["site"].Value;
                return site.EndsWith("/") ? site + url.Substring(1) : site + url;
            }

            if (url.StartsWith("../"))
            {
                url = url.Substring(3);
                int idx = pageUrl.LastIndexOf('/');
                return FixUrl(url, pageUrl.Substring(0, idx));
            }

            if (url.StartsWith("./"))
            {
                return FixUrl(url.Substring(2), pageUrl);
            }

            int end = pageUrl.LastIndexOf("/");
            return pageUrl.Substring(0, end) + "/" + url;
        }
    }


}

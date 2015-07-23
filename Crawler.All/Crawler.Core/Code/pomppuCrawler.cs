﻿using Sitecrawler.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Crawler.Data.DbContext;
using System.Text.RegularExpressions;
using Crawler.DTO.ResponseDTO;
using Crawler.API.Helper;

namespace Sitecrawler.Core.Code
{
    public class pomppuCrawler : commonCrawler
    {
        private readonly int _perpage = 15;
        protected override int perpage { get { return _perpage; } }

        protected override List<ContentRevisionDTO> GetContentsPerPage(int page)
        {
            var contentrevisionList = new List<ContentRevisionDTO>();
            List<int> snapTocontentList = new List<int>();

            //commonCrawler로 부터 상속받은 부분
            //뽐뿌
            var ppomppu = webGetkr.Load("http://m.ppomppu.co.kr/new/bbs_list.php?id=freeboard&page=" + page);

            var ppomppu_ul = ppomppu.DocumentNode.SelectNodes("//ul[@class= 'bbsList']").FirstOrDefault();
            var articles = ppomppu_ul.Descendants("li");

            foreach (var ppomppu_li in articles)
            {
                var anchortag = ppomppu_li.ChildNodes.SingleOrDefault(c => c.Name.Equals("a"));
                var titlenode = anchortag.SelectSingleNode("./strong");

                var url = new Uri(new Uri("http://m.ppomppu.co.kr/new/"), anchortag.GetAttributeValue("href", "undefined"));
                
                contentrevisionList.Add(new ContentRevisionDTO
                {
                    Crawled = DateTime.Now,
                    For_BoardId = 12,
                    isDepricate = false,
                    Content = new ContentDTO
                    {
                        Article = titlenode.InnerText.Trim(),
                        Contents_URL = url.AbsoluteUri,
                        Url_Params = url.PathAndQuery,
                        Checksum = Convert.ToBase64String(HashHelper.ObjectToMD5Hash(url.PathAndQuery))
                    }
                });
            }
            return contentrevisionList;
        }
        protected override void ParseContent(ContentRevisionDTO contentrevision)
        {
            try
            {
                var loadedContent = webGetkr.Load(contentrevision.Content.Contents_URL);

                var articlecontent = loadedContent.DocumentNode.SelectNodes("//div[@id = 'KH_Content']").SingleOrDefault();
                contentrevision.Details = articlecontent.InnerText.Trim();
                contentrevision.Details_Html = articlecontent.InnerHtml.Trim();
                contentrevision.isDepricate = false;

                var imgnodes = articlecontent.SelectNodes(".//img");

                contentrevision.SrcDatas = new List<SrcdataDTO>();
                if (imgnodes == null) return;

                foreach (var img in imgnodes)
                {
                    var srcurl = new Uri(img.GetAttributeValue("src", "default"));
                    var srcdata = new SrcdataDTO
                    {
                        SourceUrl = srcurl.AbsoluteUri,
                        IsDepricated = false,
                        FileName = System.IO.Path.GetFileName(srcurl.LocalPath),
                        SrcGuId = Guid.NewGuid(),
                    };
                    img.SetAttributeValue("guid", srcdata.SrcGuId.ToString());

                    contentrevision.SrcDatas.Add(srcdata);
                }
            }
            catch (ArgumentNullException)
            {
                contentrevision.isDepricate = true;
                return;
            }
            catch (UriFormatException)
            {
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("ReStart");
                ParseContent(contentrevision);
            }
        }
    }
}

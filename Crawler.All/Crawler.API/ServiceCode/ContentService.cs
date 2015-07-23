﻿using AutoMapper;
using Crawler.Data.DbContext;
using Crawler.DTO.RequestDTO;
using Crawler.DTO.ResponseDTO;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crawler.API.ServiceCode
{
    public partial class CrawlerService : ServiceBase
    {
        /// <summary>
        /// Content를 ContentDTO를 통해서 Create 하는 함수
        /// </summary>
        /// <param name="req">
        /// ContentDOT
        /// </param>
        /// <returns></returns>
        public EnvelopeDTO<GenericDummyDTO> Any(ContentCreateRequestDTO req)
        {
            var Content = req.Content;
            List<SrcdataDTO> SrcdataList = new List<SrcdataDTO> (); 

            using (var entities = new CrawlerStorage())
            {
                try
                {
                    var content = new Content();
                    
                    content.Contents_URL = Content.Contents_URL;
                    content.Article = Content.Article;
                    content.Url_Params = Content.Url_Params;
                    content.ContentGuId = Guid.NewGuid();
                    
                    entities.Contents.Add(content);

                    //foreach (var data in Content.SrcDatas)
                    //{
                    //    var srcdata = new Srcdata();
                    //    srcdata.SrcGuId = Guid.NewGuid();
                    //    srcdata.SourceUrl = data.SourceUrl;
                    //    srcdata.Content = content;

                    //    entities.Srcdatas.Add(srcdata);
                    //}

                    entities.SaveChanges();
                    return Succeeded(new GenericDummyDTO());
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        /// <summary>
        /// Cotent를 contentId를 통해서 불러오는 함수
        /// </summary>
        /// <param name="req">
        /// ContentId
        /// </param>
        /// <returns></returns>
        //public EnvelopeDTO<ContentDTO> Any(ContentGetbyIdRequestDTO req)
        //{
        //    var contentId = req.ContentId;

        //    if (!contentId.HasValue)
        //    {
        //        return Fail<ContentDTO>("ContentGetbyIdRequestDTO : parameter 'contentId' is empty.");
        //    }
        //    else
        //    {
        //        using (var entities = new CrawlerStorage())
        //        {
        //            try
        //            {
        //                var content = (from c in entities.Contents.AsNoTracking()
        //                                where c.Id == contentId
        //                               select new ContentDTO
        //                               {
        //                                   Id = c.Id,
        //                                   ContentGuId = c.ContentGuId,
        //                                   Article = c.Article,
        //                                   Contents_URL = c.Contents_URL,
        //                                   //SrcDatas = (from src in c.Srcdatas
        //                                   //            select new SrcdataDTO
        //                                   //            {
        //                                   //                Id = src.Id,
        //                                   //                SrcGuId = src.SrcGuId,
        //                                   //                For_ContentId = src.For_ContentId,
        //                                   //                SourceUrl = src.SourceUrl
        //                                   //            }).ToList()
        //                               }).SingleOrDefault();

        //                if (content == null)
        //                {
        //                    return Fail<ContentDTO>("ContentGetbyIdRequestDTO : Contents matching given 'contentId' does not exist.");
        //                }

        //                return Succeeded(content);
        //            }

        //            catch (Exception e)
        //            {
        //                return Fail<ContentDTO>("ContentGetbyIdRequestDTO : Exception - " + e.Message);
        //            }
        //        }
        //    }
        //}
        public EnvelopeDTO<GenericDummyDTO> Any(ContentUpdateDeatilsRequestDTO req)
        {
            var contentId = req.ContentId;

            if (contentId == null)
            {
                return Fail<GenericDummyDTO>("ContentUpdateDeatilsRequestDTO : parameter 'ContentId' is empty.");
            }

            using (var entities = new CrawlerStorage())
            {
                try
                {
                    var content = entities.Contents.SingleOrDefault(p => p.Id == contentId);
                    entities.SaveChanges();

                    return Succeeded(new GenericDummyDTO());
                }
                catch (Exception e)
                {
                    return Fail<GenericDummyDTO>("ContentUpdateDeatilsRequestDTO : Exception - " + e.Message);
                }
            }
        }
        public EnvelopeDTO<GenericDummyDTO> Any(ContentUpdateImagesRequestDTO req)
        {
            var contentId = req.ContentId;

            if (contentId == null)
            {
                return Fail<GenericDummyDTO>("ContentUpdateImagesRequestDTO : parameter 'ContentId' is empty.");
            }

            using (var entities = new CrawlerStorage())
            {
                try
                {
                    var content = entities.Contents.SingleOrDefault(p => p.Id == contentId);
                    //content.Srcdatas = req.SrcData.Select(p => new Srcdata
                    //{
                    //    SourceUrl = p.SourceUrl,
                    //    Content = content,
                    //    SrcGuId = new Guid(),
                        
                    //}).ToList();

                    entities.SaveChanges();

                    return Succeeded(new GenericDummyDTO());
                }
                catch (Exception e)
                {
                    return Fail<GenericDummyDTO>("ContentUpdateImagesRequestDTO : Exception - " + e.Message);
                }
            }
        }
    }
}
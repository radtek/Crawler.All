﻿using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.DTO.ResponseDTO
{
    [Serializable]
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ContentDTO
    {
        public int Id { get; set; }
        public Guid ContentGuId { get; set; }
        public string Url_Params { get; set; }

        public string Contents_URL { get; set; }

        public string Checksum { get; set; }

        public string Article { get; set; }

    }
      [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ContentRevisionDTO
    {
        public int id { get; set; }
        public DateTime Crawled { get; set; }

        public string Details { get; set; }

        public string Details_Html { get; set; }

        public int For_BoardId { get; set; }
        public int For_ContentId { get; set; }

        public int recommandCount { get; set; }

        public int viewCount { get; set; }

        public bool isDepricate { get; set; }

        public string CheckSum { get; set; }

        public ContentDTO Content { get; set; }

        public BoardDTO Board { get; set; }

        public List<SrcdataDTO> SrcDatas { get; set; }
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SrcdataDTO
    {
        public int Id { get; set; }
        public Guid SrcGuId { get; set; }

        public int? For_ContentId { get; set; }

        public string SourceUrl { get; set; }

        public string FileName { get; set; }

        public byte[] OriginalPayload { get; set; }

        public long OriginalPayload_Size { get; set; }

        public bool IsDepricated { get; set; }
        public ContentDTO Content { get; set; }

    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SnapshotDTO
    {
        public int Id { get; set; }

        public int For_BoardId { get; set; }
        public int For_Timeperiod { get; set; }

        public DateTime Taken { get; set; }

        public BoardDTO Board { get; set; }

        public TimePeriodDTO TimePeriod { get; set; }

     
        //public List<ContentDTO> Contents { get; set; }
        public List<ContentRevisionDTO>  ContentRevisions { get; set; }

        
        public List<SnapshotToContentRevisionDTO> SnapshotToContent { get; set; }
    }

      [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SnapshotToContentRevisionDTO
    {
        public int For_SnapshotId { get; set; }

        public int Seqno { get; set; }

        public int Has_ContentRevisionId { get; set; }
        
        public SnapshotDTO Snapshot { get; set; }
        
        public ContentRevisionDTO ContentRevision { get; set; }
    }

      [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class BoardDTO
    {
        public int Id { get; set; }

        public int For_WebsiteId { get; set; }

        public string Label { get; set; }

        public WebsiteDTO Website { get; set; }

        public List<SnapshotDTO> Snapshots { get; set; }

        public List<ContentRevisionDTO> ContentRevisions { get; set; }
    }

      [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class WebsiteDTO
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Website_URL { get; set; }

        public string Mobile_URL { get; set; }

        public string Website_logo { get; set; }
    }

      [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class TimePeriodDTO
    {
        public int Id { get; set; }

        public string ShortGuid { get; set; }

        public string Label { get; set; }

        public DateTime Scheduled { get; set; }

        public DateTime Crawled { get; set; }
    }

      [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ErrorLogDTO
    {
        public int Id { get; set; }

        public string Error_Address { get; set; }

        public string Error_URL { get; set; }

        public string Error_Details { get; set; }

        public int Hresult { get; set; }
    }
}

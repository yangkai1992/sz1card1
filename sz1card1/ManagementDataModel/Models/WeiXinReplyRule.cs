//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManagementDataModel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WeiXinReplyRule
    {
        public System.Guid Guid { get; set; }
        public byte[] SID { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public bool IsFullMatch { get; set; }
        public string ReplyContent { get; set; }
        public string Title { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
        public string MusicUrl { get; set; }
        public string HQMusicUrl { get; set; }
        public bool FuncFlag { get; set; }
        public int Sort { get; set; }
        public bool IsEnable { get; set; }
        public System.Guid msrepl_tran_version { get; set; }
    }
}
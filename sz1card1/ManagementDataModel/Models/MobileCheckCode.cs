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
    
    public partial class MobileCheckCode
    {
        public System.Guid Guid { get; set; }
        public string TermIdentity { get; set; }
        public string Mobile { get; set; }
        public string CheckCode { get; set; }
        public System.DateTime GenerateTime { get; set; }
        public System.Guid msrepl_tran_version { get; set; }
    }
}

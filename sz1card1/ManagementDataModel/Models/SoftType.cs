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
    
    public partial class SoftType
    {
        public System.Guid Guid { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int OperatingEnvironment { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string Creater { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Modifier { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public string PermitionXml { get; set; }
    }
}

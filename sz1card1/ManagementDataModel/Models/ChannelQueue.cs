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
    
    public partial class ChannelQueue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ChannelQueue()
        {
            this.Channels = new HashSet<Channel>();
        }
    
        public System.Guid Guid { get; set; }
        public string QueueName { get; set; }
        public int Type { get; set; }
        public int MaxThreadsCount { get; set; }
        public string Meno { get; set; }
        public System.Guid msrepl_tran_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Channel> Channels { get; set; }
    }
}
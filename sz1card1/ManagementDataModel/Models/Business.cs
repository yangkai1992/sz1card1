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
    
    public partial class Business
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Business()
        {
            this.AgentBusinesses = new HashSet<AgentBusiness>();
            this.AgentBusinessMoneyNotes = new HashSet<AgentBusinessMoneyNote>();
            this.BusinessInterfaces = new HashSet<BusinessInterface>();
            this.BusinessMoneyNotes = new HashSet<BusinessMoneyNote>();
            this.CardNotes = new HashSet<CardNote>();
            this.MmsSends = new HashSet<MmsSend>();
            this.MobileMemberCards = new HashSet<MobileMemberCard>();
            this.MobileMessageFilters = new HashSet<MobileMessageFilter>();
            this.MobileUpdateNotes = new HashSet<MobileUpdateNote>();
            this.MobileUpdateSets = new HashSet<MobileUpdateSet>();
            this.PlatformBugs = new HashSet<PlatformBug>();
            this.SmsReceives = new HashSet<SmsReceive>();
            this.SmsSends = new HashSet<SmsSend>();
            this.WebAddValueNotes = new HashSet<WebAddValueNote>();
            this.WeiXinMappings = new HashSet<WeiXinMapping>();
        }
    
        public byte[] SID { get; set; }
        public System.Guid Guid { get; set; }
        public string BusinessName { get; set; }
        public string Account { get; set; }
        public string DomainAccount { get; set; }
        public string DomainPassword { get; set; }
        public int IndustryId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int CountyId { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Contact { get; set; }
        public int Sex { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string LinkQQ { get; set; }
        public string Logo { get; set; }
        public string Introduction { get; set; }
        public string WebAddress { get; set; }
        public System.DateTime RegisterTime { get; set; }
        public string Meno { get; set; }
        public string PermitionXml { get; set; }
        public decimal RankRate { get; set; }
        public Nullable<System.Guid> AgentGuid { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal AvailableMoney { get; set; }
        public decimal TotalGiftMoney { get; set; }
        public decimal AvailableGiftMoney { get; set; }
        public int TotalSms { get; set; }
        public int AvailableSms { get; set; }
        public int TotalMms { get; set; }
        public int AvailableMms { get; set; }
        public int TotalMessage { get; set; }
        public int AvailableMessage { get; set; }
        public int TotalAuthorizeNum { get; set; }
        public int AvailableAuthorizeNum { get; set; }
        public string SubChannelNumber { get; set; }
        public Nullable<System.Guid> ChannelPackageGuid { get; set; }
        public string SingleMobileSmsChannel { get; set; }
        public string SingleUnicomSmsChannel { get; set; }
        public string SingleTelecomSmsChannel { get; set; }
        public string MultipleMobileSmsChannel { get; set; }
        public string MultipleUnicomSmsChannel { get; set; }
        public string MultipleTelecomSmsChannel { get; set; }
        public string SingleMmsKeyName { get; set; }
        public string MultiMmsKeyName { get; set; }
        public int SmsPriority { get; set; }
        public string SmsSignature { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAlliance { get; set; }
        public bool IsRealUsed { get; set; }
        public bool IsInterface { get; set; }
        public bool IsCloseNav { get; set; }
        public bool IsCloseBind { get; set; }
        public bool IsRegisteMember { get; set; }
        public bool IsCouponBusiness { get; set; }
        public bool IsReadQRCode { get; set; }
        public int LandWay { get; set; }
        public int RegisterSource { get; set; }
        public string SelectedKeyword { get; set; }
        public Nullable<int> GatewayType { get; set; }
        public string GatewayAccount { get; set; }
        public int CardMode { get; set; }
        public int PasswordMode { get; set; }
        public int CardType { get; set; }
        public string ApiKey { get; set; }
        public string ApiIpAddress { get; set; }
        public string MemberApiAddress { get; set; }
        public string SmsAddress { get; set; }
        public string Domain { get; set; }
        public string LoginSkin { get; set; }
        public string CardColor { get; set; }
        public string MemberRight { get; set; }
        public string MemberCardRight { get; set; }
        public byte[] RecommendSID { get; set; }
        public string OpenId { get; set; }
        public string Secret { get; set; }
        public System.Guid msrepl_tran_version { get; set; }
        public string VipCloudOpenId { get; set; }
        public string VipCloudSecret { get; set; }
        public string BusinessKey { get; set; }
        public Nullable<int> TotalFreeSms { get; set; }
        public Nullable<int> AvailableFreeSms { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentBusiness> AgentBusinesses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentBusinessMoneyNote> AgentBusinessMoneyNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessInterface> BusinessInterfaces { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessMoneyNote> BusinessMoneyNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CardNote> CardNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MmsSend> MmsSends { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileMemberCard> MobileMemberCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileMessageFilter> MobileMessageFilters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileUpdateNote> MobileUpdateNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileUpdateSet> MobileUpdateSets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlatformBug> PlatformBugs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmsReceive> SmsReceives { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SmsSend> SmsSends { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebAddValueNote> WebAddValueNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeiXinMapping> WeiXinMappings { get; set; }
    }
}

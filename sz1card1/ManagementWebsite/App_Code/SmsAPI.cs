using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using sz1card1.Common;
using sz1card1.Management.Logic;
using sz1card1.Management.Data;
using sz1card1.Management.Data.Entities;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
// 注意: 如果更改此处的类名 "SmsAPI"，也必须更新 Web.config 中对 "SmsAPI" 的引用。
public class SmsAPI : ISmsAPI
{
    /// <summary>
    /// 商家发送短信统一接口(单发)
    /// </summary>
    /// <param name="businessGuid">商家唯一编号</param>
    /// <param name="key">密钥</param>
    /// <param name="mobile">手机号</param>
    /// <param name="smsContent">短信内容</param>
    /// <param name="message">结果反馈</param>
    /// <returns>状态(true,成功;false,失败)</returns>
    public bool SingleSendSms(string businessGuid, string key, string mobile, string smsContent, out int smsCount, out string message)
    {
        return SingleSendSms_New(businessGuid, key, mobile, smsContent, null, null, out smsCount, out message);
    }

    public bool SingleSendSms_New(string businessGuid, string key, string mobile, string smsContent, string userAccount, Guid? chainStoreGuid, out int smsCount, out string message)
    {
        try
        {
            SmsMmsLogic smsMmsLogic = new SmsMmsLogic();
            ManagementDataContext dataContext = new ManagementDataContext();
            Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
            if (smsMmsLogic.CheckApiPermition(business.SID, key, out message))
            {
                return smsMmsLogic.SingleSendSms(business.SID, mobile, smsContent, userAccount, chainStoreGuid, out smsCount, out message);
            }
            else
            {
                smsCount = 0;
                return false;
            }
        }
        catch (Exception ex)
        {
            sz1card1.Common.Log.LoggingService.Warn(this, ex);
            message = ex.Message;
            smsCount = 0;
            return false;
        }
    }


    /// <summary>
    /// 商家发送短信统一接口(群发)
    /// </summary>
    /// <param name="businessGuid">商家唯一编号</param>
    /// <param name="key">密钥</param>
    /// <param name="count">条数</param>
    /// <param name="mobile">手机号</param>
    /// <param name="smsContent">短信内容</param>
    /// <param name="smsCount"></param>
    /// <param name="message">结果反馈</param>
    /// <returns>状态(true,成功;false,失败)</returns>
    public bool MultipSendSms(string businessGuid, string key, int count, string[] mobile, string[] smsContent, out int[] smsCount, out string message)
    {
        return MultipSendSms_New(businessGuid, key, count, mobile, smsContent, null, null, out smsCount, out message);
    }

    public bool MultipSendSms_New(string businessGuid, string key, int count, string[] mobile, string[] smsContent, string userAccount, Guid? chainStoreGuid, out int[] smsCount, out string message)
    {
        try
        {
            //判断手机号格式
            int newCount = count;
            SmsMmsLogic smsMmsLogic = new SmsMmsLogic();
            List<string> newMobile = mobile.ToList<string>();
            List<string> newSmsContent = smsContent.ToList<string>();
            ManagementDataContext dataContext = new ManagementDataContext();
            Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
            if (!smsMmsLogic.CheckMobiles(newMobile, newSmsContent, out newCount, out message))
            {
                smsCount = new int[count];
                return false;
            }

            //判断商家是否开启API调用功能
            if (!smsMmsLogic.CheckApiPermition(business.SID, key, out message))
            {
                smsCount = new int[count];
                return false;
            }

            //过滤敏感关键字
            List<string> keyword = null;
            if (!smsMmsLogic.KeywordFilter(newSmsContent, newMobile, out keyword))
            {
                smsCount = new int[count];
                message = string.Format("短信内容包含敏感字[{0}]", string.Join(",", keyword.ToArray()));
                return false;
            }

            //群发短信
            int j = 0;
            int[] newSmsCount;
            smsCount = new int[count];
            newCount = newSmsContent.ToArray().Length;
            if (smsMmsLogic.MultipleSendSms(business.SID, newCount, newMobile.ToArray(), newSmsContent.ToArray(), userAccount, chainStoreGuid, out newSmsCount, out message))
            {
                if (newSmsCount.Length == smsCount.Length)
                {
                    smsCount = newSmsCount;
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (mobile[i] == newMobile[j])
                        {
                            smsCount[i] = newSmsCount[j];
                            j++;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            sz1card1.Common.Log.LoggingService.Warn(this, ex);
            smsCount = new int[count];
            message = ex.Message;
            return false;
        }
    }


    /// <summary>
    /// 获取商家剩余短信条数
    /// </summary>
    /// <param name="businessGuid"></param>
    /// <returns></returns>
    public int GetBusinessAvailableSms(string businessGuid)
    {
        ManagementDataContext dataContext = new ManagementDataContext();
        Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
        BusinessAddValueLogic logic = new BusinessAddValueLogic(business.SID);
        return logic.GetBusinessAvailalbeSms();
    }

    /// <summary>
    /// 获取商家剩余彩信条数
    /// </summary>
    /// <param name="businessGuid">商家唯一编号</param>
    /// <param name="key">密钥</param>
    /// <param name="count">输出剩余彩信条数</param>
    /// <param name="message">结果反馈</param>
    /// <returns>状态(true,成功;false,失败)</returns>
    public bool GetBusAvailableSms(string businessGuid, string key, out int count, out string message)
    {
        SmsMmsLogic smsMmsLogic = new SmsMmsLogic();
        ManagementDataContext dataContext = new ManagementDataContext();
        Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
        if (smsMmsLogic.CheckApiPermition(business.SID, key, out message))
        {
            BusinessAddValueLogic logic = new BusinessAddValueLogic(business.SID);
            count = logic.GetBusinessAvailalbeSms();
            return true;
        }
        else
        {
            count = -1;
            return false;
        }
    }

    /// <summary>
    /// 商家发送彩信接口（单发）
    /// </summary>
    /// <param name="businessGuid">商家唯一编号</param>
    /// <param name="key">密钥</param>
    /// <param name="xml">彩信类</param>
    /// <param name="mmsCount">返回实际条数</param>
    /// <param name="message">结果反馈</param>
    /// <returns>状态(true,成功;false,失败)</returns>
    public bool SingleSendMms(string businessGuid, string key, string xml, out int mmsCount, out string message)
    {
        try
        {
            SmsMmsLogic smsMmsLogic = new SmsMmsLogic();
            ManagementDataContext dataContext = new ManagementDataContext();
            Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
            if (smsMmsLogic.CheckApiPermition(business.SID, key, out message))
            {
                if (smsMmsLogic.CheckSingleMms(business.SID, xml, out message) > 0)
                {
                    smsMmsLogic.SingleSendMms(business.SID, xml, out mmsCount, out message);
                    return true;
                }
                else
                {
                    mmsCount = 0;
                    return false;
                }
            }
            else
            {
                mmsCount = 0;
                return false;
            }
        }
        catch (Exception ex)
        {
            sz1card1.Common.Log.LoggingService.Warn(this, ex);
            mmsCount = 0;
            message = ex.Message;
            return false;
        }
    }


    /// <summary>
    /// 商家发送彩信接口（群发）
    /// </summary>
    /// <param name="businessGuid">商家唯一编号</param>
    /// <param name="xml">彩信类</param>
    /// <param name="mmsCount">返回实际条数</param>
    /// <param name="message">结果反馈</param>
    /// <returns>状态(true,成功;false,失败)</returns>
    public bool MultipSendMms(string businessGuid, string key, string[] xml, out int[] mmsCount, out string message)
    {
        try
        {
            SmsMmsLogic smsMmsLogic = new SmsMmsLogic();
            ManagementDataContext dataContext = new ManagementDataContext();
            Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
            if (smsMmsLogic.CheckApiPermition(business.SID, key, out message))
            {
                if (smsMmsLogic.CheckMultipMms(business.SID, xml, out mmsCount, out message) > 0)
                {
                    smsMmsLogic.MultipleSendMms(business.SID, xml, mmsCount, out message);
                    return true;
                }
                else
                {
                    mmsCount = new int[xml.Length];
                    return false;
                }
            }
            else
            {
                mmsCount = new int[xml.Length];
                return false;
            }
        }
        catch (Exception ex)
        {
            sz1card1.Common.Log.LoggingService.Warn(this, ex);
            mmsCount = new int[xml.Length];
            message = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// 获取商家剩余彩信条数
    /// </summary>
    /// <param name="businessGuid">商家唯一编号</param>
    /// <param name="key">密钥</param>
    /// <param name="count">输出剩余彩信条数</param>
    /// <param name="message">结果反馈</param>
    /// <returns>状态(true,成功;false,失败)</returns>
    public bool GetBusinessAvailableMms(string businessGuid, string key, out int count, out string message)
    {
        SmsMmsLogic smsMmsLogic = new SmsMmsLogic();
        ManagementDataContext dataContext = new ManagementDataContext();
        Business business = dataContext.Businesses.SingleOrDefault<Business>(b => b.Guid == new Guid(businessGuid));
        if (smsMmsLogic.CheckApiPermition(business.SID, key, out message))
        {
            BusinessAddValueLogic logic = new BusinessAddValueLogic(business.SID);
            count = logic.GetBusinessAvailalbeMms();
            return true;
        }
        else
        {
            count = -1;
            return false;
        }
    }
}

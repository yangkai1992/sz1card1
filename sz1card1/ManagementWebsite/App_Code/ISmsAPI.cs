using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 如果更改此处的接口名称 "ISmsAPI"，也必须更新 Web.config 中对 "ISmsAPI" 的引用。
[ServiceContract]
public interface ISmsAPI
{
    [OperationContract]
    bool SingleSendSms(string businessGuid, string key, string mobile, string smsContent, out int smsCount, out string message);

    [OperationContract]
    bool SingleSendSms_New(string businessGuid, string key, string mobile, string smsContent, string userAccount, Guid? chainStoreGuid, out int smsCount, out string message);

    [OperationContract]
    bool MultipSendSms(string businessGuid, string key, int count, string[] mobile, string[] smsContent, out int[] smsCount, out string message);

    [OperationContract]
    bool MultipSendSms_New(string businessGuid, string key, int count, string[] mobile, string[] smsContent, string userAccount, Guid? chainStoreGuid, out int[] smsCount, out string message);

    [OperationContract]
    int GetBusinessAvailableSms(string businessGuid);

    [OperationContract]
    bool GetBusAvailableSms(string businessGuid, string key, out int count, out string message);

    [OperationContract]
    bool SingleSendMms(string businessGuid, string key, string xml, out int mmsCount, out string message);

    [OperationContract]
    bool MultipSendMms(string businessGuid, string key, string[] xml, out int[] mmsCount, out string message);

    [OperationContract]
    bool GetBusinessAvailableMms(string businessGuid, string key, out int count, out string message);
}

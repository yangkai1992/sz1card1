using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    public class ZooKeeperUtility
    {
        /// <summary>
        /// 全局的发布者
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        public static void GlobalPublisher(string keyName, string value)
        {
            ZookeeperGlobalPublisher publisher = new ZookeeperGlobalPublisher(keyName);
            publisher.Publish(value);
        }
        /// <summary>
        /// 全局订阅者
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="timeSpan">超时时间</param>
        /// <param name="value"></param>
        /// <returns>是否超时</returns>
        public static bool GlobalSubscriber(string keyName, TimeSpan timeSpan, ref string value)
        {
            using (ZookeeperGlobalSubscriber Subscriber = new ZookeeperGlobalSubscriber(keyName))
            {
                return Subscriber.WaitForChanged(timeSpan, ref value);
            }
        }
        /// <summary>
        /// 商户级别的发布者
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        public static void WinIdentityPublisher(string keyName, string value)
        {
            ZooKeeperWinIdentityPublisher publisher = new ZooKeeperWinIdentityPublisher(keyName);
            publisher.Publish(value);
        }
        /// <summary>
        /// 商户级别的订阅者
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="timeSpan">超时时间</param>
        /// <param name="value"></param>
        /// <returns>是否超时</returns>
        public static bool WinIdentitySubscriber(string keyName, TimeSpan timeSpan, ref string value)
        {
            using(ZooKeeperWinIdentitySubscriber Subscriber = new ZooKeeperWinIdentitySubscriber(keyName))
            {
                return Subscriber.WaitForChanged(timeSpan, ref value);
            }
        }
        /// <summary>
        /// 应用级别的发布者
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        public static void ApplicationPublisher(string keyName, string value)
        {
            ZooKeeperApplicationPublisher publisher = new ZooKeeperApplicationPublisher(keyName);
            publisher.Publish(value);
        }
        /// <summary>
        /// 应用级别的订阅者
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="timeSpan">超时时间</param>
        /// <param name="value"></param>
        /// <returns>是否超时</returns>
        public static bool ApplicationSubscriber(string keyName, TimeSpan timeSpan, ref string value)
        {
            using (ZooKeeperApplicationSubscriber Subscriber = new ZooKeeperApplicationSubscriber(keyName))
            {
                return Subscriber.WaitForChanged(timeSpan, ref value);
            }
        }
    }
}

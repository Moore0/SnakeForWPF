using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;

namespace SnakeForWPF.DAL
{
    /// <summary>
    /// 简易配置文件管理类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 读写锁
        /// </summary>
        private static readonly ReaderWriterLock readerWriterLock = new ReaderWriterLock();

        /// <summary>
        /// 延迟时间
        /// </summary>
        public static int TIMEOUT { private set; get; } = 0;

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadConfig(string key)
        {
            readerWriterLock.AcquireReaderLock(TIMEOUT);
            string result = "";
            try
            {
                //刷新配置节
                RefreshSection();
                result = ConfigurationManager.AppSettings[key];
            }
            catch (ConfigurationErrorsException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                readerWriterLock.ReleaseReaderLock();
            }
            return result;
        }


        /// <summary>
        /// 移除配置
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveConfig(string key)
        {
            ConfigurationManager.AppSettings.Remove(key);
        }

        /// <summary>
        /// 刷新配置节
        /// </summary>
        public static void RefreshSection()
        {
            //刷新配置节
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteConfig(string key, object value)
        {
            readerWriterLock.AcquireWriterLock(TIMEOUT);
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (!config.AppSettings.Settings.AllKeys.Contains(key))
                {
                    config.AppSettings.Settings.Add(key, value.ToString());
                }
                else
                {
                    config.AppSettings.Settings[key].Value = value.ToString();
                }
                config.Save();
            }
            catch (ConfigurationErrorsException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                readerWriterLock.ReleaseWriterLock();
            }
        }
    }
}

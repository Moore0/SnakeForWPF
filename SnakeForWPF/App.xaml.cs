using NLog;
using SnakeForWPF.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeForWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        #region ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public App()
        {}
        #endregion

        #region prop

        /// <summary>
        /// 日志
        /// </summary>
        public static Logger Logger { private set; get; } = LogManager.GetLogger("Log");

        /// <summary>
        /// 游戏速度(单位:毫秒)
        /// </summary>
        public static long Speed
        {
            set => ConfigHelper.WriteConfig(nameof(Speed), value);
            get
            {
                if (long.TryParse(ConfigHelper.ReadConfig(nameof(Speed)), out long result))
                    return result;
                return 1000;
            }
        }
        #endregion

        /// <summary>
        /// 主线程全局异常捕获
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Debugger.Break();

            //记录日志
            Logger.Error(e);
            e.Handled = true;
        }
    }
}

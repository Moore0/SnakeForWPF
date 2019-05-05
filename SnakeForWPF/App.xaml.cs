using SnakeForWPF.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        /// <summary>
        /// 构造函数
        /// </summary>
        public App()
        {}

        /// <summary>
        /// 主线程全局异常捕获
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //记录日志




            e.Handled = true;
        }

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







    }
}

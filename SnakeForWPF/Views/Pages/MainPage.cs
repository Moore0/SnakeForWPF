using SnakeForWPF.Attributes;
using SnakeForWPF.ViewModels;
using SnakeForWPF.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeForWPF.Views.Pages
{
    /// <summary>
    /// 主页面
    /// </summary>
    [PageStyle(Uri = "SnakeForWPF;component/Styles/Pages/MainPageStyle.xaml", UriKind = UriKind.Relative, StyleKey = "MainPageStyle")]
    public class MainPage : PageBase<MainPageViewModel>
    {
        #region ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainPage()
        {}

        #endregion
    }
}

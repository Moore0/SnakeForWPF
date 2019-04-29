using SnakeForWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeForWPF.Views.Windows
{
    /// <summary>
    /// 窗口基类
    /// </summary>
    public class WindowBase<T> : Window
        where T : ViewModelBase, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WindowBase()
        {
            //设置DataContext
            DataContext = ViewModel;
        }

        /// <summary>
        /// ViewModel对象
        /// </summary>
        public virtual T ViewModel {  set; get; } = new T();
    }
}

using SnakeForWPF.Attributes;
using SnakeForWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SnakeForWPF.Views.Pages
{
    /// <summary>
    /// 页面基类(从UserControl继承)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageBase<T> : UserControl
        where T : ViewModelBase, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageBase()
        {
            //应用样式
            LoadStyleFromAssembly();

            //设置DataContext
            DataContext = ViewModel;
        }


        /// <summary>
        /// 获取并程序集中的样式
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="uriKind"></param>
        /// <param name="styleKey"></param>
        protected virtual void LoadStyleFromAssembly()
        {
            //获取templatepart定义的控件
            object[] attributes = this.GetType().GetCustomAttributes(typeof(PageStyleAttribute), true);

            if (attributes.Length != 1)
                throw new ArgumentException(nameof(PageStyleAttribute));

            //获取Attribute
            PageStyleAttribute controlStyleAttribute = (PageStyleAttribute)attributes[0];

            //获取样式
            ResourceDictionary dictionary = new ResourceDictionary
            {
                Source = new Uri(controlStyleAttribute.Uri, controlStyleAttribute.UriKind)
            };
            //应用样式
            SetValue(StyleProperty, dictionary[controlStyleAttribute.StyleKey]);
        }

        /// <summary>
        /// ViewModel对象
        /// </summary>
        public T ViewModel { private set; get; } = new T();
    }
}

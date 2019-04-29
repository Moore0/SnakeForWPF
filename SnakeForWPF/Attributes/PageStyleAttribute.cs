using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeForWPF.Attributes
{
    /// <summary>
    /// 页面样式特性,用于指定页面的样式
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PageStyleAttribute : Attribute
    {
        /// <summary>
        /// 模版路径
        /// </summary>
        public string Uri { set; get; }

        /// <summary>
        /// 路径类型
        /// </summary>
        public UriKind UriKind { set; get; }

        /// <summary>
        /// 样式的Key
        /// </summary>
        public string StyleKey { set; get; }
    }
}

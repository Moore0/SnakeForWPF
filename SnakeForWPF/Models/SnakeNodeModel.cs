using SnakeForWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SnakeForWPF.Models
{
    /// <summary>
    /// 贪吃蛇节点
    /// </summary>      
    [Serializable]
    public class SnakeNode : ModelBase
    {
        private string brush = "Black";
        /// <summary>
        /// 画刷
        /// </summary>
        public Brush Brush
        {
            get
            {
                BrushConverter brushConverter = new BrushConverter();
                return (Brush)brushConverter.ConvertFromString(brush);
            }
            set => brush = value.ToString();
        }


        /// <summary>
        /// X轴坐标
        /// </summary>
        public int X { set; get; }

        /// <summary>
        /// Y轴坐标
        /// </summary>
        public int Y { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SnakeForWPF.Controls
{
    /// <summary>
    /// 单个节点
    /// </summary>
    public sealed class SnakeNode : FrameworkElement
    {
        /// <summary>
        /// 画刷
        /// </summary>
        public Brush Brush { set; get; } = Brushes.Black;

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brush, null, new Rect(0, 0, ActualWidth, ActualHeight));
        }
    }
}

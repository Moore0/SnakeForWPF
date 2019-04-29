using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SnakeForWPF.Adorners
{
    /// <summary>
    /// 食物Adorner
    /// </summary>
    public class FoodAdorner : Adorner
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adornedElement">装饰的元素</param>
        /// <param name="point">位置</param>
        /// <param name="radiuX">水平半径</param>
        /// <param name="radiuY">垂直半径</param>
        public FoodAdorner(UIElement adornedElement, Point point, double radiuX, double radiuY)
            : base(adornedElement)
        {
            Point = point;
            RadiuX = radiuX;
            RadiuY = radiuY;
        }



        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawEllipse(Brushes.Red, null, Point, RadiuX, RadiuY);
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Point Point { set; get; }

        /// <summary>
        /// 水平半径
        /// </summary>
        public double RadiuX { set; get; }

        /// <summary>
        /// 垂直半径
        /// </summary>
        public double RadiuY { set; get; }
    }
}

using SnakeForWPF.Adorners;
using SnakeForWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeForWPF.Panels
{
    /// <summary>
    /// 贪吃蛇面板
    /// </summary>
    public class SnakePanel : Panel
    {
        #region ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public SnakePanel()
        { }


        #endregion

        #region propdp
        /// <summary>
        /// 贪吃蛇节点集合
        /// </summary>
        public IList<SnakeNode> SnakeNodes
        {
            get { return (IList<SnakeNode>)GetValue(SnakeNodesProperty); }
            set { SetValue(SnakeNodesProperty, value); }
        }
        public static readonly DependencyProperty SnakeNodesProperty =
            DependencyProperty.Register(nameof(SnakeNodes), typeof(IList<SnakeNode>), typeof(SnakePanel),
                new FrameworkPropertyMetadata(new List<SnakeNode>(), FrameworkPropertyMetadataOptions.AffectsArrange, (s, e) =>
           {




           }));

        /// <summary>
        /// 总行数
        /// </summary>
        public int LineX
        {
            get { return (int)GetValue(LineXProperty); }
            set
            {
                if (value < 19 || value > 38)
                    throw new ArgumentException(nameof(LineX));
                SetValue(LineXProperty, value);
            }
        }
        public static readonly DependencyProperty LineXProperty =
            DependencyProperty.Register(nameof(LineX), typeof(int), typeof(SnakePanel),
                new FrameworkPropertyMetadata(19, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback((s, e) =>
             {


             })));


        /// <summary>
        /// 总列数
        /// </summary>
        public int LineY
        {
            get { return (int)GetValue(LineYProperty); }
            set
            {
                if (value < 19 || value > 38)
                    throw new ArgumentException(nameof(LineY));
                SetValue(LineYProperty, value);
            }
        }
        public static readonly DependencyProperty LineYProperty =
            DependencyProperty.Register(nameof(LineY), typeof(int), typeof(SnakePanel), new FrameworkPropertyMetadata(19, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback((s, e) =>
             {



             })));


        #endregion



        #region propa


        #endregion



        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            DrawGrid(dc);
        }


        /// <summary>
        /// 绘制网格
        /// </summary>
        public void DrawGrid(DrawingContext dc)
        {
            //绘制列
            double xLength = ActualWidth / LineX;
            for (int i = 0; i < LineX; ++i)
            {
                dc.DrawLine(new Pen(Brushes.LightGray, 1), new Point(i * xLength, 0), new Point(i * xLength, ActualHeight));
            }

            //绘制行
            double yLength = ActualHeight / LineY;
            for (int i = 0; i < LineY; ++i)
            {
                dc.DrawLine(new Pen(Brushes.LightGray, 1), new Point(0, i * yLength), new Point(ActualWidth, i * yLength));
            }
        }




        /// <summary>
        /// 计算元素大小
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            //foreach (var node in Nodes)
            //{
            //    node.Measure(new Size(availableSize.Width / LineX, availableSize.Height / LineY));
            //}
            return availableSize;
        }


        protected override int VisualChildrenCount => SnakeNodes.Count;


        /// <summary>
        /// 排列元素
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double xLength = ActualWidth / LineX;
            double yLength = ActualHeight / LineY;

            //foreach (var node in Children.Cast<FrameworkElement>())
            //{
            //    node.Arrange(new Rect(new Point(GetX(node) * xLength, GetY(node) * yLength), new Size(xLength, yLength)));
            //}

            return finalSize;
        }
    }
}

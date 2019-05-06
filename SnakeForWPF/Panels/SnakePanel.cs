using SnakeForWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
        {
            //设计缺陷
            Panel = this;
        }

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
                new FrameworkPropertyMetadata(new List<SnakeNode>(), FrameworkPropertyMetadataOptions.AffectsRender, (d, e) =>
           {
               if (e.NewValue is INotifyCollectionChanged notifyCollection)
               {
                   NotifyCollectionChangedEventHandler handler = (sender, ee) =>
                   {
                       //触发重绘
                       Panel.InvalidateVisual();
                   };
                   notifyCollection.CollectionChanged -= handler;
                   notifyCollection.CollectionChanged += handler;
               }
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
                new FrameworkPropertyMetadata(19, FrameworkPropertyMetadataOptions.AffectsRender));


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
            DependencyProperty.Register(nameof(LineY), typeof(int), typeof(SnakePanel),
                new FrameworkPropertyMetadata(19, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 食物坐标
        /// </summary>
        public Point? FoodPoint
        {
            get { return (Point?)GetValue(FoodPointProperty); }
            set { SetValue(FoodPointProperty, value); }
        }
        public static readonly DependencyProperty FoodPointProperty =
            DependencyProperty.Register(nameof(FoodPoint), typeof(Point?), typeof(SnakePanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region propa


        #endregion

        #region prop

        /// <summary>
        /// 面板对象
        /// </summary>
        public static SnakePanel Panel { private set; get; }

        #endregion

        #region 绘制逻辑

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            //可使用依赖注入使这部分逻辑独立
            DrawGrid(dc);
            DrawSnakeNodes(dc);
            DrawFood(dc);
        }

        /// <summary>
        /// 绘制食物
        /// </summary>
        /// <param name="dc"></param>
        private void DrawFood(DrawingContext dc)
        {
            if (FoodPoint == null)
                return;

            double xLength = ActualWidth / LineX;
            double yLength = ActualHeight / LineY;

            dc.DrawEllipse(Brushes.Red, null, new Point((FoodPoint.Value.X + 0.5) * xLength, (FoodPoint.Value.Y + 0.5) * yLength), 10, 10);
        }

        /// <summary>
        /// 绘制贪吃蛇节点
        /// </summary>
        private void DrawSnakeNodes(DrawingContext dc)
        {
            double xLength = ActualWidth / LineX;
            double yLength = ActualHeight / LineY;

            foreach (var node in SnakeNodes)
            {
                dc.DrawRectangle(node.Brush, null, new Rect(xLength * node.X, yLength * node.Y, xLength, yLength));
            }
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

        #endregion
    }
}

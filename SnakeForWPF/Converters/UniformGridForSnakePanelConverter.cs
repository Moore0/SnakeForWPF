using SnakeForWPF.Converters.Base;
using SnakeForWPF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SnakeForWPF.Converters
{
    /// <summary>
    /// SnakePanel面板转换器
    /// </summary>
    public class SnakePanelControlConverter : BaseMultiValueConverter<SnakePanelControlConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() < 5)
                throw new ArgumentOutOfRangeException(nameof(values));

            if (!(values[0] is UniformGrid uniformGrid))
                throw new ArgumentNullException(nameof(values));

            if (!(values[1] is IList<SnakeNode> nodes))
                throw new ArgumentNullException(nameof(values));

            int lineX = (int)values[2];
            int lineY = (int)values[3];
            Point? foodPoint = (Point?)values[4];

            //格子数量发生变化
            if (uniformGrid.Children.Count != lineX * lineY)
            {
                uniformGrid.Children.Clear();

                for (int i = 0; i < lineX * lineY; ++i)
                {
                    Border border = new Border { Background = Brushes.White, Margin = new Thickness(1) };
                    uniformGrid.Children.Add(border);
                }
            }

            //节点的index集合(这里没有清空后重绘,确保操作不重叠)
            List<int> nodeIndexs = new List<int>();

            //获取食物的index
            int? foodIndex = null;
            if (foodPoint.HasValue)
                foodIndex = (int)foodPoint.Value.X + (int)foodPoint.Value.Y * lineX;

            //获得蛇的节点的index
            foreach (var node in nodes)
            {
                int index = node.X + node.Y * lineX;
                nodeIndexs.Add(index);
            }

            //遍历Blocks
            for (int i = 0; i < uniformGrid.Children.Count; ++i)
            {
                if (!(uniformGrid.Children[i] is Border border))
                    throw new ArgumentException(nameof(uniformGrid.Children));

                //绘制食物
                if (foodIndex != null && foodIndex == i)
                {
                    border.Background = new DrawingBrush(new GeometryDrawing(Brushes.Red, null, new EllipseGeometry(new Point(), 10, 10))) { Stretch = Stretch.None };
                    continue;
                }

                //绘制空格
                if (!nodeIndexs.Contains(i))
                {
                    border.Background = Brushes.White;
                }
                else if (nodeIndexs.Contains(i))
                {
                    //绘制节点
                    border.Background = Brushes.Black;
                }
            }

            return null;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

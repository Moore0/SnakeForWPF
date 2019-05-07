using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SnakeForWPF.Converters
{
    /// <summary>
    /// 方块背景画刷转换器
    /// </summary>
    public class BlockBrushConverter : BaseValueConverter<BlockBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var blockType = value.ToString();
            Brush brush = Brushes.White;

            
            switch (blockType)
            {
                //空
                case "Empty":
                    brush = Brushes.White;
                    break;
                //节点
                case "Node":
                    brush = Brushes.Black;
                    break;
                //食物
                case "Food":
                    //写成资源定义到xaml里更合适...
                    brush = new DrawingBrush(new GeometryDrawing(Brushes.Red, null, new EllipseGeometry(new Point(), 10, 10))) { Stretch = Stretch.None };
                    break;

                default:
                    Debugger.Break();
                    break;
            }

            return brush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

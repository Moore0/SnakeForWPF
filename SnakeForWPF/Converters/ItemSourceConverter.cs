using SnakeForWPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeForWPF.Converters
{
    public class ItemSourceConverter : BaseMultiValueConverter<ItemSourceConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            #region 获取参数

            if (values.Count() < 4)
                throw new ArgumentOutOfRangeException(nameof(values));

            if (!(values[0] is IList<SnakeNode> nodes))
                throw new ArgumentNullException(nameof(values));

            int lineX = (int)values[1];
            int lineY = (int)values[2];
            Point? foodPoint = (Point?)values[3];
            #endregion

            //构造新的数据集合
            dynamic[] array = new dynamic[lineX * lineY];

            //添加空白
            for (int i = 0; i < array.Count(); i++)
            {
                array[i]= new { BlockType = "Empty" };
            }

            //添加食物
            if (foodPoint.HasValue)
            {
                int index = (int)foodPoint.Value.X + (int)foodPoint.Value.Y * lineX;
                array[index] = new { BlockType = "Food" };
            }

            //添加蛇的节点
            foreach (var node in nodes)
            {
                int index = node.X + node.Y * lineX;
                array[index] = new { BlockType = "Node" };
            }

          
            return array;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

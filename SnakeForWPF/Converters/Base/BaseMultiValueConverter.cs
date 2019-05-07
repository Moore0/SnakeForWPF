using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SnakeForWPF.Converters
{
    /// <summary>
    /// 多值转换器基类
    /// </summary>
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter
        where T : class, new()
    {
        /// <summary>
        /// 转换器对象
        /// </summary>
        private static T converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new T());
        }


        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeForWPF.ViewModels
{
    /// <summary>
    /// ViewModel基类
    /// </summary>
    [PropertyChanged.ImplementPropertyChanged]
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性发生改变时调用
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 手动通知触发更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="oldValue">原始值</param>
        /// <param name="newValue">新的值</param>
        /// <param name="propertyName">属性名称</param>
        protected void UpdateProperty<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (Equals(oldValue, newValue))
                return;

            oldValue = newValue;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// 通知界面更新
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

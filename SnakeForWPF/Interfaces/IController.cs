using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SnakeForWPF.Interfaces
{
    /// <summary>
    /// 控制
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 开始
        /// </summary>
        void Start();
        /// <summary>
        /// 退出
        /// </summary>
        void Exit();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();

        /// <summary>
        /// 继续
        /// </summary>
        void Continue();
    }
}

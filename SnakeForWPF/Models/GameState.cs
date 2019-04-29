using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeForWPF.Models
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// 等待开始
        /// </summary>
        WaitBegin,
        /// <summary>
        /// 开始
        /// </summary>
        Begin,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause
    }
}

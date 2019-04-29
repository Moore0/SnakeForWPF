using SnakeForWPF.Commands;
using SnakeForWPF.Controls;
using SnakeForWPF.Interfaces;
using SnakeForWPF.Models;
using SnakeForWPF.Panels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeForWPF.ViewModels
{
    /// <summary>
    /// 主页面ViewModel
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainPageViewModel()
        {

        }

        /// <summary>
        /// 开始命令
        /// </summary>
        public ICommand StartCommand
        {
            //重新开始游戏
            get => new RelayCommand(e => (e as SnakePanel).Start(), e => (e as SnakePanel)?.GameState == GameState.WaitBegin);
        }

        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand ExitCommand
        {
            get => new RelayCommand(e => (e as SnakePanel).Exit(), _ => true);
        }

        /// <summary>
        /// 暂停命令
        /// </summary>
        public ICommand PauseComamnd
        {
            //暂停游戏
            get => new RelayCommand(e => (e as SnakePanel).Pause(), e => (e as SnakePanel)?.GameState == GameState.Begin);
        }

        /// <summary>
        /// 继续命令
        /// </summary>
        public ICommand ContinueComamnd
        {
            //继续游戏
            get => new RelayCommand(e => (e as SnakePanel).Continue(), e => (e as SnakePanel)?.GameState == GameState.Pause);
        }
    }
}

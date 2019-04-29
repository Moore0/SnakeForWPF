using SnakeForWPF.Commands;
using SnakeForWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SnakeForWPF.ViewModels
{
    /// <summary>
    /// 主窗口ViewModel
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 主窗口对象
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        /// <summary>
        /// 最小化命令
        /// </summary>
        public ICommand MinimizeCommand { get; set; }
        /// <summary>
        /// 最大化命令
        /// </summary>
        public ICommand MaximizeCommand { get; set; }
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand { get; set; }
        /// <summary>
        /// 菜单命令
        /// </summary>
        public ICommand MenuCommand { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="window"></param>
        public MainWindowViewModel(Window window) 
            : base()
        {
            mWindow = window ?? throw new ArgumentException(nameof(window));

            MinimizeCommand = new RelayCommand(_ => mWindow.WindowState = WindowState.Minimized, _ => true);
            MaximizeCommand = new RelayCommand(_ => mWindow.WindowState ^= WindowState.Maximized, _ => mWindow.ResizeMode != ResizeMode.NoResize);
            CloseCommand = new RelayCommand(_ => mWindow.Close(), _ => true);
            MenuCommand = new RelayCommand(_ => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()), _ => true);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        { }

        /// <summary>
        /// 获取鼠标位置
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            var position = Mouse.GetPosition(mWindow);
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }
    }
}

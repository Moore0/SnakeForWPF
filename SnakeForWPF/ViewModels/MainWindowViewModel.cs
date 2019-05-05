using SnakeForWPF.Commands;
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
        #region ctor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="window"></param>
        public MainWindowViewModel(Window window)
            : base()
        {
            mWindow = window ?? throw new ArgumentException(nameof(window));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        { }

        #endregion

        #region prop

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        #endregion

        #region command

        /// <summary>
        /// 最小化命令
        /// </summary>
        public ICommand MinimizeCommand { get => new RelayCommand(_ => mWindow.WindowState = WindowState.Minimized, _ => true); }
        /// <summary>
        /// 最大化命令
        /// </summary>
        public ICommand MaximizeCommand { get => new RelayCommand(_ => mWindow.WindowState ^= WindowState.Maximized, _ => mWindow.ResizeMode != ResizeMode.NoResize); }
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand { get => new RelayCommand(_ => mWindow.Close(), _ => true); }
        /// <summary>
        /// 菜单命令
        /// </summary>
        public ICommand MenuCommand { get=> new RelayCommand(_ => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()), _ => true); }

        #endregion

        /// <summary>
        /// 主窗口对象
        /// </summary>
        private readonly Window mWindow;

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

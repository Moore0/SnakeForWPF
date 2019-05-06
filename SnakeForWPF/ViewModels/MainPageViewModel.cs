using SnakeForWPF.Commands;
using SnakeForWPF.Models;
using SnakeForWPF.Panels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeForWPF.ViewModels
{
    /// <summary>
    /// 主页面ViewModel
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        #region ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainPageViewModel()
        {
            //关联键盘按下事件
            App.Current.MainWindow.KeyDown += KeyDown;
        }

        #endregion

        #region command
        /// <summary>
        /// 开始命令
        /// </summary>
        public ICommand StartCommand
        {
            //重新开始游戏
            get => new RelayCommand(_ => Start(), _ => GameState == GameState.WaitBegin);
        }

        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand ExitCommand
        {
            //退出
            get => new RelayCommand(_ => Exit(), _ => true);
        }

        /// <summary>
        /// 暂停命令
        /// </summary>
        public ICommand PauseComamnd
        {
            //暂停游戏
            get => new RelayCommand(_ => Pause(), _ => GameState == GameState.Begin);
        }

        /// <summary>
        /// 继续命令
        /// </summary>
        public ICommand ContinueComamnd
        {
            //继续游戏
            get => new RelayCommand(_ => Continue(), _ => GameState == GameState.Pause);
        }

        #endregion

        #region prop

        /// <summary>
        /// 贪吃蛇节点数据
        /// </summary>
        [PropertyChanged.DoNotCheckEquality]
        public ObservableCollection<SnakeNode> SnakeNodes {private set; get; } = new ObservableCollection<SnakeNode>();

        /// <summary>
        /// 游戏状态(默认处于等待开始)
        /// </summary>
        public GameState GameState { private set; get; } = GameState.WaitBegin;

        /// <summary>
        /// 上一个按键
        /// </summary>
        public Key? LastKey { private set; get; }

        /// <summary>
        /// 食物点
        /// </summary>
        public Point? FoodPoint { private set; get; }

        /// <summary>
        /// 行数
        /// </summary>
        public int LineX { set; get; } = 19;

        /// <summary>
        /// 列数
        /// </summary>
        public int LineY { set; get; } = 19;

        #endregion

        #region 主要逻辑

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="key"></param>
        private void Move(Key key)
        {
            if (!CanMove(key))
            {
                //判断通关
                if (SnakeNodes.Count == LineX * LineY)
                {
                    Win();
                }
                else
                {
                    Dead();
                }
                return;
            }

            switch (key)
            {
                case Key.Left:
                case Key.A:
                    MoveLeft();
                    break;
                case Key.Up:
                case Key.W:
                    MoveUp();
                    break;
                case Key.Right:
                case Key.D:
                    MoveRight();
                    break;
                case Key.Down:
                case Key.S:
                    MoveDown();
                    break;
            }
        }

        /// <summary>
        /// 死亡
        /// </summary>
        private void Dead()
        {
            GameState = GameState.WaitBegin;
            LastKey = null;
            MessageBox.Show("GameOver");
        }

        /// <summary>
        /// 通关
        /// </summary>
        private void Win()
        {
            GameState = GameState.WaitBegin;
            LastKey = null;
            MessageBox.Show("Win");
        }


        /// <summary>
        /// 能否移动
        /// </summary>
        /// <returns></returns>
        private bool CanMove(Key key)
        {
            //还未开始
            if (SnakeNodes.Count == 0)
                return false;

            //判断死亡
            switch (key)
            {
                case Key.Left:
                case Key.A:
                    return CanMoveLeft(key);
                case Key.Up:
                case Key.W:
                    return CanMoveUp(key);
                case Key.Right:
                case Key.D:
                    return CanMoveRight(key);
                case Key.Down:
                case Key.S:
                    return CanMoveDown(key);
            }

            return true;
        }

        /// <summary>
        /// 获取头节点位置
        /// </summary>
        /// <returns></returns>
        private Tuple<int, int> GetHeadXY()
        {
            if (SnakeNodes.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(SnakeNodes));

            int x = SnakeNodes[SnakeNodes.Count - 1].X;
            int y = SnakeNodes[SnakeNodes.Count - 1].Y;

            return new Tuple<int, int>(x, y);
        }

        /// <summary>
        /// 判断是否与自身相撞
        /// </summary>
        /// <returns></returns>
        private bool IsHitSelf(int x, int y)
        {
            //判断是否撞到自身
            for (int i = 0; i < SnakeNodes.Count - 1; ++i)
            {
                if (SnakeNodes[i].X == x && SnakeNodes[i].Y == y)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 能否向左移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveLeft(Key key)
        {
            var head = GetHeadXY();

            //超出界限
            if (head.Item1 - 1 < 0)
                return false;

            //判断是否与自身相撞
            if (IsHitSelf(head.Item1 - 1, head.Item2))
                return false;

            return true;
        }

        /// <summary>
        /// 能否向上移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveUp(Key key)
        {
            var head = GetHeadXY();

            //超出界限
            if (head.Item2 - 1 < 0)
                return false;

            //判断是否与自身相撞
            if (IsHitSelf(head.Item1, head.Item2 - 1))
                return false;

            return true;
        }


        /// <summary>
        /// 能否向右移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveRight(Key key)
        {
            var head = GetHeadXY();

            //超出界限
            if (head.Item1 + 1 > LineX - 1)
                return false;

            //判断是否与自身相撞
            if (IsHitSelf(head.Item1 + 1, head.Item2))
                return false;


            return true;
        }

        /// <summary>
        /// 能否向下移动
        /// </summary>
        /// <returns></returns>
        private bool CanMoveDown(Key key)
        {
            var head = GetHeadXY();

            //超出界限
            if (head.Item2 + 1 > LineY - 1)
                return false;

            //判断是否与自身相撞
            if (IsHitSelf(head.Item1, head.Item2 + 1))
                return false;

            return true;
        }

        /// <summary>
        /// 是否吃掉食物
        /// </summary>
        /// <returns></returns>
        public bool IsEatFood(Point p)
        {
            if (p == FoodPoint)
            {
                var node = new SnakeNode
                {
                    X = (int)p.X,
                    Y = (int)p.Y
                };

                SnakeNodes.Add(node);

                FoodPoint = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        private void MoveLeft()
        {
            //判断吃掉食物
            var lastNode = SnakeNodes[SnakeNodes.Count - 1];
            if (IsEatFood(new Point(lastNode.X - 1, lastNode.Y)))
                return;
            for (int i = 0; i < SnakeNodes.Count - 1; ++i)
            {
                SnakeNodes[i].X = SnakeNodes[i + 1].X;
                SnakeNodes[i].Y = SnakeNodes[i + 1].Y;
            }
            lastNode.X -= 1;
        }

        /// <summary>
        /// 向上移动
        /// </summary>
        private void MoveUp()
        {
            //判断吃掉食物
            var lastNode = SnakeNodes[SnakeNodes.Count - 1];
            if (IsEatFood(new Point(lastNode.X, lastNode.Y - 1)))
                return;

            for (int i = 0; i < SnakeNodes.Count - 1; ++i)
            {
                SnakeNodes[i].X = SnakeNodes[i + 1].X;
                SnakeNodes[i].Y = SnakeNodes[i + 1].Y;
            }
            lastNode.Y -= 1;
        }

        /// <summary>
        /// 向右移动
        /// </summary>
        private void MoveRight()
        {
            //判断吃掉食物
            var lastNode = SnakeNodes[SnakeNodes.Count - 1];
            if (IsEatFood(new Point(lastNode.X + 1, lastNode.Y)))
                return;

            for (int i = 0; i < SnakeNodes.Count - 1; ++i)
            {
                SnakeNodes[i].X = SnakeNodes[i + 1].X;
                SnakeNodes[i].Y = SnakeNodes[i + 1].Y;
            }
            lastNode.X += 1;
        }

        /// <summary>
        /// 向下移动
        /// </summary>
        private void MoveDown()
        {
            //判断吃掉食物
            var lastNode = SnakeNodes[SnakeNodes.Count - 1];
            if (IsEatFood(new Point(lastNode.X, lastNode.Y + 1)))
                return;

            for (int i = 0; i < SnakeNodes.Count - 1; ++i)
            {
                SnakeNodes[i].X = SnakeNodes[i + 1].X;
                SnakeNodes[i].Y = SnakeNodes[i + 1].Y;
            }
            lastNode.Y += 1;
        }

        /// <summary>
        /// 获取食物
        /// </summary>
        public Point GetFoodPoint()
        {
            //空的节点
            List<Point> spacePoints = new List<Point>();
            //已存在的节点
            List<Point> snakePoints = new List<Point>();

            //遍历节点,添加空的节点
            foreach (var node in SnakeNodes)
            {
                snakePoints.Add(new Point(node.X, node.Y));
            }

            for (int i = 0; i < LineX; ++i)
            {
                for (int j = 0; j < LineY; ++j)
                {
                    Point p = new Point(i, j);

                    //添加空的节点
                    if (!snakePoints.Contains(p))
                        spacePoints.Add(p);
                }
            }

            //从空的节点中随机获取一个
            int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, spacePoints.Count);
            return spacePoints[index];
        }




        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDown(object sender, KeyEventArgs e)
        {
            //不能掉头
            if ((LastKey == Key.A || LastKey == Key.Left)
                && (e.Key == Key.D || e.Key == Key.Right))
                return;
            if ((LastKey == Key.W || LastKey == Key.Up)
                && (e.Key == Key.S || e.Key == Key.Down))
                return;
            if ((LastKey == Key.D || LastKey == Key.Right)
                && (e.Key == Key.A || e.Key == Key.Left))
                return;
            if ((LastKey == Key.S || LastKey == Key.Down)
                && (e.Key == Key.W || e.Key == Key.Up))
                return;

            //记录上一次的按键
            LastKey = e.Key;
        }


        /// <summary>
        /// 重新开始
        /// </summary>
        public async void Start()
        {
            //清空数据
            SnakeNodes.Clear();
            //置为开始
            GameState = GameState.Begin;

            //初始化第一个节点
            //设置节点位置
            //第一个节点设置到中间
            SnakeNode node = new SnakeNode
            {
                X = LineX / 2,
                Y = LineY / 2
            };

            //添加节点到SnakePanel
            SnakeNodes.Add(node);

            //循环
            while (true)
            {
                //处于开始状态且上一个按键不为null
                if (GameState == GameState.Begin
                    && LastKey != null)
                {
                    //移动
                    Move((Key)LastKey);

                    //获取食物
                    if (FoodPoint == null)
                    {
                        //获取食物位置
                        Point p = GetFoodPoint();
                        FoodPoint = p;
                    }
                }
                else if (GameState == GameState.WaitBegin)
                {
                    break;
                }

                //触发更新
                SnakeNodes = new ObservableCollection<SnakeNode>(SnakeNodes);

                //延迟一段时间
                await Task.Delay(TimeSpan.FromMilliseconds(App.Speed));
            }
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Pause()
        {
            GameState = GameState.Pause;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Continue()
        {
            GameState = GameState.Begin;
        }

        #endregion
    }
}

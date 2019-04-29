using SnakeForWPF.Adorners;
using SnakeForWPF.Controls;
using SnakeForWPF.Interfaces;
using SnakeForWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeForWPF.Panels
{
    /// <summary>
    /// 贪吃蛇面板
    /// </summary>
    public class SnakePanel : Panel, IController
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SnakePanel()
        {
            //挂钩键盘处理事件
            Application.Current.MainWindow.KeyDown += SnakePanel_KeyDown;

        }

        /// <summary>
        /// 游戏状态(默认处于等待开始)
        /// </summary>
        public GameState GameState { set; get; } = GameState.WaitBegin;

        /// <summary>
        /// 上一个按键
        /// </summary>
        private Key? LastKey { set; get; }

        /// <summary>
        /// 获取AdornerLayer
        /// </summary>
        public AdornerLayer Layer { get => AdornerLayer.GetAdornerLayer(this); }

        /// <summary>
        /// 总行数
        /// </summary>
        public int LineX
        {
            get { return (int)GetValue(LineXProperty); }
            set
            {
                if (value < 19 || value > 38)
                    throw new ArgumentException(nameof(LineX));
                SetValue(LineXProperty, value);
            }
        }
        public static readonly DependencyProperty LineXProperty =
            DependencyProperty.Register(nameof(LineX), typeof(int), typeof(SnakePanel), new PropertyMetadata(19));

        /// <summary>
        /// 总列数
        /// </summary>
        public int LineY
        {
            get { return (int)GetValue(LineYProperty); }
            set
            {
                if (value < 19 || value > 38)
                    throw new ArgumentException(nameof(LineY));
                SetValue(LineYProperty, value);
            }
        }
        public static readonly DependencyProperty LineYProperty =
            DependencyProperty.Register(nameof(LineY), typeof(int), typeof(SnakePanel), new PropertyMetadata(19));



        public static int GetX(DependencyObject obj)
        {
            return (int)obj.GetValue(XProperty);
        }

        public static void SetX(DependencyObject obj, int value)
        {
            obj.SetValue(XProperty, value);
        }



        /// <summary>
        /// 附加属性-元素所在行数
        /// </summary>
        public static readonly DependencyProperty XProperty =
            DependencyProperty.RegisterAttached("X", typeof(int), typeof(SnakePanel), new PropertyMetadata(0));


        public static int GetY(DependencyObject obj)
        {
            return (int)obj.GetValue(YProperty);
        }

        public static void SetY(DependencyObject obj, int value)
        {
            obj.SetValue(YProperty, value);
        }
        /// <summary>
        /// 附加属性-元素所在列数
        /// </summary>
        public static readonly DependencyProperty YProperty =
            DependencyProperty.RegisterAttached("Y", typeof(int), typeof(SnakePanel), new PropertyMetadata(0));

        /// <summary>
        /// 食物点
        /// </summary>
        private Point? FoodPoint { set; get; }

        /// <summary>
        /// 绘制网格
        /// </summary>
        public void DrawGrid(DrawingContext dc)
        {
            //绘制列
            double xLength = ActualWidth / LineX;
            for (int i = 0; i < LineX; ++i)
            {
                dc.DrawLine(new Pen(Brushes.LightGray, 1), new Point(i * xLength, 0), new Point(i * xLength, ActualHeight));
            }

            //绘制行
            double yLength = ActualHeight / LineY;
            for (int i = 0; i < LineY; ++i)
            {
                dc.DrawLine(new Pen(Brushes.LightGray, 1), new Point(0, i * yLength), new Point(ActualWidth, i * yLength));
            }
        }


        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            DrawGrid(dc);
        }

        /// <summary>
        /// 计算元素大小
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var node in Children.Cast<FrameworkElement>())
            {
                node.Measure(new Size(availableSize.Width / LineX, availableSize.Height / LineY));
            }
            return availableSize;
        }

        /// <summary>
        /// 排列元素
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double xLength = ActualWidth / LineX;
            double yLength = ActualHeight / LineY;

            foreach (var node in Children.Cast<FrameworkElement>())
            {
                node.Arrange(new Rect(new Point(GetX(node) * xLength, GetY(node) * yLength), new Size(xLength, yLength)));
            }

            return finalSize;
        }



        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SnakePanel_KeyDown(object sender, KeyEventArgs e)
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
        /// 移动
        /// </summary>
        /// <param name="key"></param>
        private void Move(Key key)
        {
            if (!CanMove(key))
            {
                Dead();
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

            //重新布局
            InvalidateArrange();
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
            if (Children.Count == 0)
                return false;

            //判断通关
            if (Children.Count == LineX * LineY)
            {
                Win();
                return false;
            }

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
            if (Children.Count == 0)
                throw new ArgumentOutOfRangeException();

            int x = GetX(Children[Children.Count - 1]);
            int y = GetY(Children[Children.Count - 1]);

            return new Tuple<int, int>(x, y);
        }

        /// <summary>
        /// 判断是否与自身相撞
        /// </summary>
        /// <returns></returns>
        private bool IsHitSelf(int x, int y)
        {
            //判断是否撞到自身
            for (int i = 0; i < Children.Count - 1; ++i)
            {
                if (GetX(Children[i]) == x && GetY(Children[i]) == y)
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
                var node = new SnakeNode();
                //设置节点位置
                //第一个节点设置到中间
                SetX(node, (int)p.X);
                SetY(node, (int)p.Y);

                //添加节点到SnakePanel
                Children.Add(node);
                InvalidateArrange();

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
            var lastNode = Children[Children.Count - 1];
            if (IsEatFood(new Point(GetX(lastNode) - 1, GetY(lastNode))))
                return;

            for (int i = 0; i < Children.Count - 1; ++i)
            {
                SetX(Children[i], GetX(Children[i + 1]));
                SetY(Children[i], GetY(Children[i + 1]));
            }
            SetX(lastNode, GetX(lastNode) - 1);
        }

        /// <summary>
        /// 向上移动
        /// </summary>
        private void MoveUp()
        {
            //判断吃掉食物
            var lastNode = Children[Children.Count - 1];
            if (IsEatFood(new Point(GetX(lastNode), GetY(lastNode) - 1)))
                return;

            for (int i = 0; i < Children.Count - 1; ++i)
            {
                SetX(Children[i], GetX(Children[i + 1]));
                SetY(Children[i], GetY(Children[i + 1]));
            }
            SetY(lastNode, GetY(lastNode) - 1);

        }

        /// <summary>
        /// 向右移动
        /// </summary>
        private void MoveRight()
        {
            //判断吃掉食物
            var lastNode = Children[Children.Count - 1];
            if (IsEatFood(new Point(GetX(lastNode) + 1, GetY(lastNode))))
                return;

            for (int i = 0; i < Children.Count - 1; ++i)
            {
                SetX(Children[i], GetX(Children[i + 1]));
                SetY(Children[i], GetY(Children[i + 1]));
            }
            SetX(lastNode, GetX(lastNode) + 1);

        }

        /// <summary>
        /// 向下移动
        /// </summary>
        private void MoveDown()
        {
            //判断吃掉食物
            var lastNode = Children[Children.Count - 1];
            if (IsEatFood(new Point(GetX(lastNode), GetY(lastNode) + 1)))
                return;

            for (int i = 0; i < Children.Count - 1; ++i)
            {
                SetX(Children[i], GetX(Children[i + 1]));
                SetY(Children[i], GetY(Children[i + 1]));
            }
            SetY(lastNode, GetY(lastNode) + 1);

        }

        /// <summary>
        /// 获取食物
        /// </summary>
        public Point GetFoodPoint()
        {
            //空的节点
            List<Point> spacePoints = new List<Point>();

            List<Point> snakePoints = new List<Point>();


            foreach (var item in Children.Cast<FrameworkElement>())
            {
                snakePoints.Add(new Point(GetX(item), GetY(item)));
            }

            //遍历
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

            int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, spacePoints.Count);
            return spacePoints[index];
        }


        /// <summary>
        /// 重新开始
        /// </summary>
        public async void Start()
        {
            //清空数据
            Children.Clear();
            //置为开始
            GameState = GameState.Begin;

            //初始化第一个节点
            SnakeNode node = new SnakeNode();

            //设置节点位置
            //第一个节点设置到中间
            SetX(node, LineX / 2);
            SetY(node, LineY / 2);

            //添加节点到SnakePanel
            Children.Add(node);

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

                        //移除之前的
                        Adorner[] ads = Layer.GetAdorners(this);
                        if (ads != null)
                        {
                            for (int i = ads.Length - 1; i >= 0; --i)
                            {
                                Layer.Remove(ads[i]);
                            }
                        }

                        //添加到装饰层
                        Layer.Add(new FoodAdorner(this, new Point((p.X + 0.5) * ActualWidth / LineX, (p.Y + 0.5) * ActualHeight / LineY), 10, 10));
                    }
                }
                else if(GameState==GameState.WaitBegin)
                {
                    break;
                }

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
    }
}

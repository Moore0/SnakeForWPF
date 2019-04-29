using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeForWPF.Behaviors
{
    public abstract class Behavior<T> : BehaviorBase
    {
        protected new T AssociatedObject { get => (T)base.AssociatedObject; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeForWPF.Behaviors
{
    public abstract class BehaviorBase
    {
        protected object AssociatedObject
        {
            get;
            private set;
        }

        public void Attach(object obj)
        {
            if (obj != AssociatedObject)
            {
                AssociatedObject = obj;
                OnAttached();
            }
        }

        protected virtual void OnAttached()
        { }
    }
}

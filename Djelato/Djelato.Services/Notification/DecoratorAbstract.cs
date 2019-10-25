﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Djelato.Services.Notification
{
    public abstract class DecoratorAbstract /*: INotifier*/
    {
        protected INotifier _notifier;

        public DecoratorAbstract(INotifier notifier)
        {
            notifier = _notifier;
        }
        public void SetComponent(INotifier notifier)
        {
            this._notifier = notifier;
        }
    
    }
}

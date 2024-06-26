﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Provides an abstraction for notification implementations registered by <see cref="NotificationManager"/>.
    /// </summary>
    public interface INotificationRoot : INotificationSharedDataProvider, INotification
    {

    }

    /// <summary>
    ///  Provides an abstraction for notification implementations registered by <see cref="NotificationManager"/>.
    /// </summary>
    /// <typeparam name="T">Type of custom properties object.</typeparam>
    public interface INotificationRoot<T> : INotificationRoot, INotification<T> where T : new()
    {

    }
}

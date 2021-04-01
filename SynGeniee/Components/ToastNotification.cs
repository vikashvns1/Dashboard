using System;
using Microsoft.AspNetCore.Components;

namespace SynGeniee.Components
{
    public class ToastNotification
    {
        public event Action<ToastOptions> ToastInstance;

        public void Open(ToastOptions options)
        {
            // Invoke ToastComponent to update and show the toast with messages
            this.ToastInstance.Invoke(options);
        }
    }
}
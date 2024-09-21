using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp17.Classes
{
    internal class ChangeWindowState
    {
        private Window currentWindow;
        private Window newWindow;

        public ChangeWindowState(Window currentWindow, Window newWindow)
        {
            this.currentWindow = currentWindow;
            this.newWindow = newWindow;
        }

        public void ApplyChanges()
        {
            newWindow.WindowState = currentWindow.WindowState;
            newWindow.Left = currentWindow.Left;
            newWindow.Top = currentWindow.Top;
            newWindow.Show();
            currentWindow.Hide();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Ant_Hill
{
    // уровень здоровья объекта
    class HealthBar
    {
        public ProgressBar Bar { get; private set; }

        public HealthBar(int max, int width)
        {
            Bar = new ProgressBar();
            Bar.Width = width;
            Bar.Height = 5;
            Bar.Maximum = max;
        }

        public void SetValue(int newValue) 
        {
            if (Bar.InvokeRequired)
                Bar.Invoke(new MethodInvoker(() => {
                    Bar.Value = Math.Min(Bar.Maximum, Math.Max(0, newValue));
                }));
            else
                Bar.Value = Math.Min(Bar.Maximum, Math.Max(0, newValue));
        }
    }
}

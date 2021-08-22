using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Ant_Hill
{
    class FrmAntHill : Form
    {
        private static object locker = new object();
        private static Random rnd = new Random();
        private static FrmAntHill instance;

        public int FieldHeight { get; private set; }
        public int FieldWidth { get; private set; }



        FrmAntHill()
        {
            WindowState = FormWindowState.Maximized;
            Text = "Муравейник";

            FieldWidth = 1920;
            FieldHeight = 1000;

            TabControl tabControl = new TabControl();
            tabControl.Width = FieldWidth;
            tabControl.Height = FieldHeight;

            BackColor = Color.PaleGreen;

            Show();
        }

        public static FrmAntHill GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                    instance = new FrmAntHill();
            }
            return instance;
        }

     
        public void Add(PictureBox box)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => {
                    Controls.Add(box);
                }));
            else
            {
                Controls.Add(box);
            }
                
        }
        public void Add(ProgressBar pbr)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    Controls.Add(pbr);
                    Refresh();
                }));
            else
            {
                Controls.Add(pbr);
                Refresh();
            }
        }

        public void Remove(PictureBox box)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => {
                    Controls.Remove(box);
                }));
            else
                Controls.Remove(box);
        }

        public void Remove(ProgressBar pbr)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => {
                    Controls.Remove(pbr);
                }));
            else
                Controls.Remove(pbr);
        }

        public Point GetRndLocation(int width, int height)
        {
            return new Point(rnd.Next(FieldWidth - width), rnd.Next(FieldHeight - height - 100));
        }

        public void ChangeLocation(PictureBox box, Point Location)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    box.Location = Location;
                    Refresh();
                }));
            else
            {
                box.Location = Location;
                Refresh();
            }
                
        }

        public void ChangeLocation(ProgressBar pbr, Point Location)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    pbr.Location = Location;
                }));
            else
                pbr.Location = Location;
        }

        public void BringToFront(PictureBox box)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    box.BringToFront();
                }));
            else
                box.BringToFront();
        }

        public void SendToBack(PictureBox box)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    box.SendToBack();
                }));
            else
                box.SendToBack();
        }

        public void BringToFront(ProgressBar pbr)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    pbr.BringToFront();
                }));
            else
                pbr.BringToFront();
        }

        public void SendToBack(ProgressBar pbr)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() =>
                {
                    pbr.SendToBack();
                }));
            else
                pbr.SendToBack();
        }
    }
}

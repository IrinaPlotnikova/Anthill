using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.Drawing;

namespace Ant_Hill
{
    // фрукты на поле
    class Fruits : FieldPublisher
    {
        public Fruits() : base()
        {
            field.MouseClick += FieldClick;
            thread.Start();
        }

        override protected void RunThread()
        {
            for (int i = 0; i < 50; i++)
            {
                CreateItem(field.GetRndLocation(50, 50), DefaultView.GetPbxFood());
                Thread.Sleep(15000);
            }
        }

        // появление фруктов по щелчку левой кнопкой мыши
        private void FieldClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                CreateItem(e.Location, DefaultView.GetPbxFood());
        }
    }

   
}

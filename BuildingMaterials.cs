using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Ant_Hill
{
    // стройматериалы на поле
    class BuildingMaterials : FieldPublisher
    {
        public BuildingMaterials() : base()
        {
            field.MouseClick += FieldClick;
            thread.Start();
        }

        override protected void RunThread()
        {
            for (int i = 0; i < 50; i++)
            {
                CreateItem(field.GetRndLocation(50, 50), DefaultView.GetPbxMaterial());
                Thread.Sleep(10000);
            }
        }

        // появление строй материалов на поле
        // по щелчку правой кнопкой мыши
        private void FieldClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                CreateItem(e.Location, DefaultView.GetPbxMaterial());
        }
    }
}

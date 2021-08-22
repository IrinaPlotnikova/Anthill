using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Ant_Hill
{
    class SolderCreator : ICreator
    {
        public Insenct Create() => new AntSolder();
    }

    // муравей-солдат, собирает стройматериалы
    class AntSolder : Ant
    {
        public AntSolder() : base( 2)
        {
            antHill.NumberOfSolders++;
            thread.Start();
        }

        override protected void RunThread()
        {
            BehaveAsBaby(80);
            Point Location = ant.Location;
            field.Remove(ant);
            ant = DefaultView.GetPbxSolder();
            ant.Location = Location;
            field.Add(ant);
            field.BringToFront(ant);
            field.BringToFront(healthBar.Bar);

            antHill.Materials.AddSubscriber(this);
            antHill.Materials.NotifySubscriber(this);

            while (health > 0) // основной цмкл жизни
            {
                Move();
                ChangeGoalState();
                Eat();
                Thread.Sleep(10);
            }
            Disappear();
            antHill.NumberOfSolders--;
        }
         
        // изменение состояния цели
        private void ChangeGoalState()
        {
            lock (locker)
            {
                if (isCarringItem)
                {
                    if (ant.Location == antHill.AntHillCentre(ant.Location))
                    {
                        isCarringItem = false;
                        antHill.AddMaterials();
                        field.Remove(goal);
                        goal = null;
                        antHill.Materials.NotifySubscriber(this);
                    }
                }
                else if (goal != null)
                {
                    if (ant.Location == goal.Location)
                    {
                        isCarringItem = true;
                        antHill.Materials.UpdateStateOfItem(goal, false);
                        field.BringToFront(goal);
                        field.BringToFront(ant);
                        field.BringToFront(healthBar.Bar);
                    }
                }
            }
        }

        // отправка сообщения об изменении состояния объекта-цели
        override public void Update(PictureBox item, bool isAvailable) 
        {
            Update(antHill.Materials, item, isAvailable);
    
        }
    }
}

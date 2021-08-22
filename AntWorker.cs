using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Ant_Hill
{
    class WorkerCreator : ICreator
    {
        public Insenct Create() => new AntWorker();

    }

    // муравей-работник, собирает еду
    class AntWorker : Ant
    {
        public AntWorker() : base(2) {
            antHill.NumberOfWorkers++;
            antHill.Food.AddSubscriber(this);
            antHill.Food.NotifySubscriber(this);
            
            thread.Start();
        }

        override protected void RunThread()
        {
            BehaveAsBaby(90);
            Point Location = ant.Location;
            field.Remove(ant);
            ant = DefaultView.GetPbxWorker();
            ant.Location = Location;
            field.Add(ant);
            field.BringToFront(ant);
            field.BringToFront(healthBar.Bar);

            antHill.Food.AddSubscriber(this);
            antHill.Food.NotifySubscriber(this); // запрос цели

            while (health > 0) // основной жизненный цикл
            {
                Move();
                ChangeGoalState();
                Eat();
                Thread.Sleep(10);
            }
            antHill.Food.RemoveSubscriber(this);
            if (goal != null)
                antHill.Food.UpdateStateOfItem(goal, true);
            Disappear();
            antHill.NumberOfWorkers--;
        }

        // изменение состония цели
        private void ChangeGoalState()
        {
            lock (locker)
            {
                if (isCarringItem)
                {
                    if (ant.Location == antHill.AntHillCentre(ant.Location))
                    {
                        isCarringItem = false;
                        antHill.AddFood(); 
                        field.Remove(goal);
                        goal = null;
                        antHill.Food.NotifySubscriber(this);
                    }
                }
                else if (goal != null)
                {
                    if (ant.Location == goal.Location)
                    {
                        isCarringItem = true;
                        antHill.Food.UpdateStateOfItem(goal, false);
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
            Update(antHill.Food, item, isAvailable);
        }
    }
}

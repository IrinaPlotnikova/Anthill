using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Ant_Hill
{
    class PestCreator : ICreator
    {
        public Insenct Create() => new Pest();
    }

    // вредитель
    class Pest : Insenct
    {
        private PictureBox goal; // объект, к которому идет движение
        public Pest() : base(1)
        {
            thread.Start();
        }
        override protected void RunThread()
        {
            BehaveAsBaby(50); 
            Point Location = ant.Location;
            field.Remove(ant);
            ant = DefaultView.GetPbxPest();
            ant.Location = Location;
            field.Add(ant);
            field.BringToFront(ant);
            field.BringToFront(healthBar.Bar);
   
            antHill.Food.AddSubscriber(this);
            antHill.Food.NotifySubscriber(this); // запрос цели

            while (health > 0) // основной цикл жизни
            {
                Move();
                ChangeGoalState();
                DecreaseHealth();
                Thread.Sleep(10);
            }
            antHill.Food.RemoveSubscriber(this);
            Disappear();
        }

        // движение вредителя
        override protected void Move()
        {
            lock (locker)
            {
                Point nextLocation;
                if (goal != null)
                    nextLocation = GetNextLocation(goal.Location);
                else
                    nextLocation = GetNextLocation();
                field.ChangeLocation(ant, nextLocation);
                field.ChangeLocation(healthBar.Bar, new Point(nextLocation.X, nextLocation.Y - 5));
            }
            
        }

        // изменение состояния цели
        private void ChangeGoalState()
        {
            lock (locker)
            {
                if (goal != null && ant.Location == goal.Location)
                {
                    field.Remove(goal);
                    PictureBox tmp = goal;
                    goal = null;
                    health = 1000;
                    healthBar.SetValue(health);
                    antHill.Food.UpdateStateOfItem(tmp, false);
                    antHill.Food.NotifySubscriber(this);
                }
            }
        }

        // уменьшение количества здоровья
        private void DecreaseHealth()
        {
            health -= Math.Max(0, 1 + antHill.NumberOfSolders);
            healthBar.SetValue(health);
        }

        // обновление цели в зависимости от состояния item
        override public void Update(PictureBox item, bool isAvailable)
        {
            lock (locker)
            {
                if (isAvailable)
                {
                    if (goal == null || Distance(ant.Location, goal.Location) > Distance(ant.Location, item.Location))
                        goal = item;
                }
                else
                {
                    if (goal == item)
                    {
                        goal = null;
                        antHill.Food.NotifySubscriber(this);
                    }
                }
            }
        }

    }
}

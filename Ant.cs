using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ant_Hill
{
    // абстрактный класс муравья
    abstract class Ant : Insenct
    {
        protected PictureBox goal;
        protected bool isCarringItem;

        public Ant(int Hunger) : base(Hunger) 
        {
            goal = null;
            isCarringItem = false;
        }

        // движение к цели
        override protected void Move()
        {
            lock (locker)
            {
                Point nextLocation;
                if (isCarringItem)
                    nextLocation = GetNextLocation(antHill.AntHillCentre(ant.Location));
                else if (goal == null)
                    nextLocation = GetNextLocation();
                else
                    nextLocation = GetNextLocation(goal.Location);


                field.ChangeLocation(ant, nextLocation);
                field.ChangeLocation(healthBar.Bar, new Point(nextLocation.X, nextLocation.Y - 5));
                if (isCarringItem)
                    field.ChangeLocation(goal, nextLocation);
            }
        }

        // обновление состояния цели
        public void Update(FieldPublisher publisher, PictureBox item, bool isAvailable)
        {
            if (isAvailable)
            {
                if (goal == null || Distance(ant.Location, goal.Location) > Distance(ant.Location, item.Location))
                    goal = item;
            }
            else
            {
                if (!isCarringItem && goal == item)
                {
                    goal = null;
                    publisher.NotifySubscriber(this);
                }
            }
        }
    }
}

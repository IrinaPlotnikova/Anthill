using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Ant_Hill
{
 
    class QueenCreator : ICreator
    {
        public Insenct Create() => new Queen();
    }

    // королева муравьев, рожает новых
    class Queen : Insenct
    {
        public Queen() : base(2) 
        {
            antHill.NumberOfQueens++;
            thread.Start();
        }

        protected override void RunThread()
        {
            BehaveAsBaby(70);
            Point Location = ant.Location;
            field.Remove(ant);
            ant = DefaultView.GetPbxQueen();
            ant.Location = Location;
            field.Add(ant);
            field.BringToFront(ant);
            field.BringToFront(healthBar.Bar);

            antHill.Food.AddSubscriber(this);
            antHill.Food.NotifySubscriber(this);

            int i = 0;
            while (health > 0) // основной цикл жизни
            {
                Move();
                if (i % 500 == 0)
                    Work();
                i++;
                Eat();
                Thread.Sleep(10);
            }
            Disappear();
            antHill.NumberOfQueens--;
        }

        // рождение муравья
        private void Work()
        {
            ICreator[] creators = new ICreator[]{ new WorkerCreator(),    new PestCreator(),
                                                  new PolicemanCreator(), new SolderCreator()};
            creators[rnd.Next(4)].Create();
        }


        override public void Update(PictureBox item, bool isAvailable) { }
    }

   
}

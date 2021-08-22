using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Ant_Hill
{
    abstract class Subscriber
    {
        // сообщает об изменении состояния объекта
        abstract public void Update(PictureBox item, bool isAvailable);
    }

    interface ICreator
    {
        Insenct Create();
    }

    // абстрактный класс насекомого
    // Insenct -> Pest
    //         -> Ant -> AntQueen
    //                -> AntSolder
    //                -> AntWorker
    //                -> AntPoliceman
    abstract class Insenct: Subscriber
    {
        protected static object locker = new object();
        protected static Random rnd;

        // форма, к которой прикреплено насекомое
        protected FrmAntHill field; 
        // муравейник, с которым в данный момент работает насекомое
        protected AntHill antHill;  
        protected Thread thread;
        protected HealthBar healthBar;

        protected PictureBox ant;
        protected int health;
        protected int hunger;
        private int DX, DY; // координаты
        int maxHealth = 1000;

        /// <summary>
        /// конструктор класса
        /// </summary>
        /// <param name="Hunger"> кол-во потребляемой еды /// </param>
        public Insenct(int Hunger)
        {
            if (rnd == null)
                rnd = new Random();
          
            field = FrmAntHill.GetInstance();
            antHill = AntHill.GetInstance();
            ant = DefaultView.GetPbxBaby();

            health = maxHealth;
            hunger = Hunger;
            healthBar = new HealthBar(health, ant.Width);

            DX = 1 - rnd.Next(2) * 2;
            DY = 1 - rnd.Next(2) * 2;

            ant.Location = field.GetRndLocation(ant.Width, ant.Height);
            healthBar.Bar.Location = new Point(ant.Location.X, ant.Location.Y - 5);

            field.Add(ant);
            field.Add(healthBar.Bar);
            field.BringToFront(ant);
            field.BringToFront(healthBar.Bar);

            thread = new Thread(new ThreadStart(RunThread));
        }

        abstract protected void RunThread();

        // переход в новую позицию
        virtual protected void Move()
        {
            Point nextLocation = GetNextLocation();
            field.ChangeLocation(ant, nextLocation);
            field.ChangeLocation(healthBar.Bar, new Point(nextLocation.X, nextLocation.Y - 5));
        }

        // кормление насекомого
        public void Eat()
        {
            health -= Math.Min(health, hunger);
            int takenFood = Math.Min(Math.Min(maxHealth - health, hunger * 2), antHill.AmountOfFood);
            health += takenFood;
            antHill.ChangeAmountOfFood(-takenFood);
            healthBar.SetValue(health);
        }

        // получение следующей координаты, когда нет цели
        // (насекомое доходит до стены и отскакивает от нее)
        protected Point GetNextLocation()
        {
            if (ant.Location.X + DX <= 0 || ant.Location.X + DX > field.FieldWidth - ant.Width)
            {
                DX *= -1;
            }
            if (ant.Location.Y + DY <= 0 || ant.Location.Y + DY > field.FieldHeight - ant.Height - 30)
            {
                DY *= -1;
            }
            return new Point(ant.Location.X + DX, ant.Location.Y + DY);
        }

        // получение следующей координаты, когда есть цель
        protected Point GetNextLocation(Point destination)
        {
            int differenceX = destination.X - ant.Location.X;
            int differenceY = destination.Y - ant.Location.Y;
            int length = Math.Max(1, Math.Min(Math.Abs(differenceX), Math.Abs(differenceY)));
            int dx = differenceX / length;
            int dy = differenceY / length;

            if (Math.Abs(dx) > 2)
                dx = dx / Math.Abs(dx) * 2;
            if (Math.Abs(dy) > 2)
                dy = dy / Math.Abs(dy) * 2;

            return new Point(ant.Location.X + dx, ant.Location.Y + dy);
        }

        // вычисление расстояния между координатами
        protected int Distance(Point p1, Point p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }

        // исчезновение насекомого 
        protected void Disappear()
        {
            field.Remove(ant);
            field.Remove(healthBar.Bar);
        }

        // первый цикл жизни насекомого, бесцельно перемещается и ест
        protected void BehaveAsBaby(int numberOfIterations)
        {
            for(int i = 0; i < numberOfIterations && hunger > 0; i++)
            {
                Move();
                Eat();
                Thread.Sleep(50);
            }
        }
    }
}

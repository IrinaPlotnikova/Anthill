using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Ant_Hill
{

    // муравейник
    class AntHill
    {
        private static object locker = new object();
        //private static AntHill instance;
        public Fruits Food { get; private set; }
        public BuildingMaterials Materials { get; private set; }

        public int AmountOfFood;
        public int NumberOfQueens { get; set; }
        public int NumberOfWorkers { get; set; }
        public int NumberOfSolders { get; set; }
        public int NumberOfPolicemen { get; set; }

        private List<PictureBox> antHills;

        private static AntHill instanse;
        private int maxAmountOfFood;
        private int numberOfMaterials;
        private int neededNumberOfMaterials;

        private FrmAntHill field;
        private Thread thread;

        // основаная модель МУРАВЕЙНИК
        private AntHill()
        {
            field = FrmAntHill.GetInstance();
            Food = new Fruits();
            Materials = new BuildingMaterials();
            antHills = new List<PictureBox>();

            AmountOfFood = 2000;
            maxAmountOfFood = 3000;

            numberOfMaterials = 0;
            neededNumberOfMaterials = 3;
             
            NumberOfQueens = 0;
            NumberOfWorkers = 0;
            NumberOfSolders = 0;
            NumberOfPolicemen = 0;

            AddAntHill();
                 
            thread = new Thread(new ThreadStart(RunThread));
            thread.Start();
        }


        // муравейник реализован через паттерн "Одиночник"
        public static AntHill GetInstance()
        {
            lock (locker)
            {
                if (instanse == null)
                    instanse = new AntHill();
            }
            return instanse;
        }


        // запуск сценария
        protected void RunThread()
        {
            new QueenCreator().Create();
            new WorkerCreator().Create();
            new SolderCreator().Create();
        }


        // добавляем муравейник
        private void AddAntHill()
        {
            PictureBox pbx = DefaultView.GetPbxAntHill();
            pbx.Location = field.GetRndLocation(pbx.Image.Width, pbx.Image.Height);
            antHills.Add(pbx);
            pbx.Cursor = Cursors.Hand;
            pbx.MouseClick += AntHillClick;
            maxAmountOfFood += 1000;
            field.Add(pbx);
        }
        

        // клик по муравейнику добавляет еду // аналогично приносу еды в муравейник
        private void AntHillClick(object sender, EventArgs e)
        {
            AddFood();
        }


        // возвращает координаты ближайшего муравейника относительно точки pnt 
        public Point AntHillCentre(Point pnt)
        {
            Point result = antHills[0].Location;
            lock (locker)
            {
                for (int i = 1; i < antHills.Count; i++)
                    if (Distance(pnt, antHills[i].Location) < Distance(pnt, result))
                        result = antHills[i].Location;
            }
            return result;
        }


        // возвращаете расстояние между двумя точками
        private int Distance(Point p1, Point p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }


        // изменение еды в муравейнике на delta
        public void ChangeAmountOfFood(int delta)
        {
            lock (locker)
            {
                AmountOfFood += delta;
                if (AmountOfFood > maxAmountOfFood)
                    AmountOfFood = maxAmountOfFood;
            }
        }


        // добавление еды в муравейник
        public void AddFood()
        {
            ChangeAmountOfFood(1000 + NumberOfPolicemen * 2000);
        }


        // добавление материалов в муравейник
        public void AddMaterials()
        {
            lock (locker)
            {
                numberOfMaterials++;
                // если достаточно материалов для строительства 
                // дополнительного муравейника — строим его 
                if (numberOfMaterials == neededNumberOfMaterials)
                {
                    numberOfMaterials = 0;
                    maxAmountOfFood += 2000;
                    neededNumberOfMaterials++;
                    AddAntHill();
                }
            }
        }

    }
}

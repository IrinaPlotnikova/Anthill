using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Ant_Hill
{
    static class DefaultView
    {
        private static readonly Image[] foodItemsImages;
        private static readonly Region[] foodItemsRegions;
        private static readonly Image[] materialsImages;
        private static readonly Region[] materialsRegions;
        private static readonly Image[] antsImages;
        private static readonly Region[] antsRegions;
        private static readonly Image antHillImage;
        private static readonly Region antHillRegion;
        private static readonly Random rnd;

        // класс с картинками
        static DefaultView()
        {
            rnd = new Random();

            int numberOfFoodItems = 63;
            foodItemsImages = new Image[numberOfFoodItems];
            foodItemsRegions = new Region[numberOfFoodItems];
            for (int i = 0; i < numberOfFoodItems; i++)
            {
                foodItemsImages[i] = (Image)Properties.Resources.ResourceManager.GetObject("Food" + i.ToString());
                foodItemsRegions[i] = GetRegion(foodItemsImages[i]);
            }

            int numberOfMaterials = 39;
            materialsImages = new Image[numberOfMaterials];
            materialsRegions = new Region[numberOfMaterials];
            for (int i = 0; i < numberOfMaterials; i++)
            {
                materialsImages[i] = (Image)Properties.Resources.ResourceManager.GetObject("Materials" + i.ToString());
                materialsRegions[i] = GetRegion(materialsImages[i]);
            }

            int numberOfAnts = 6;
            antsImages = new Image[numberOfAnts];
            antsRegions = new Region[numberOfAnts];
            for (int i = 0; i < numberOfAnts; i++)
            {
                antsImages[i] = (Image)Properties.Resources.ResourceManager.GetObject("Ant" + i.ToString());
                antsRegions[i] = GetRegion(antsImages[i]);
            }

            antHillImage = (Image)Properties.Resources.ResourceManager.GetObject("ahill");
            antHillRegion = GetRegion(antHillImage);
            
        }

        
        public static PictureBox GetPbxFood()
        {
            int index = rnd.Next(foodItemsImages.Length);
            PictureBox pbx = new PictureBox();

            pbx.Image = foodItemsImages[index];
            pbx.Region = foodItemsRegions[index];
            pbx.Width = 50;
            pbx.Height = 50;
          
            return pbx;
        }


        public static PictureBox GetPbxMaterial()
        {
            int index = rnd.Next(materialsImages.Length);
            PictureBox pbx = new PictureBox();

            pbx.Image = materialsImages[index];
            pbx.Region = materialsRegions[index];
            pbx.Width = 50;
            pbx.Height = 50;

            return pbx;
        }
        public static PictureBox GetPbxQueen()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antsImages[0];
            pbx.Region = antsRegions[0];
            pbx.Width = 50;
            pbx.Height = 50;
            return pbx;
        }

        public static PictureBox GetPbxBaby()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antsImages[1];
            pbx.Region = antsRegions[1];
            pbx.Width = 50;
            pbx.Height = 50;
            return pbx;
        }

        public static PictureBox GetPbxWorker()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antsImages[2];
            pbx.Region = antsRegions[2];
            pbx.Width = 50;
            pbx.Height = 50;
            return pbx;
        }

        public static PictureBox GetPbxPoliceman()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antsImages[3];
            pbx.Region = antsRegions[3];
            pbx.Width = 50;
            pbx.Height = 50;
            return pbx;
        }

        public static PictureBox GetPbxSolder()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antsImages[4];
            pbx.Region = antsRegions[4];
            pbx.Width = 50;
            pbx.Height = 50;
            return pbx;
        }

        public static PictureBox GetPbxPest()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antsImages[5];
            pbx.Region = antsRegions[5];
            pbx.Width = 50;
            pbx.Height = 50;
            return pbx;
        }

        public static PictureBox GetPbxAntHill()
        {
            PictureBox pbx = new PictureBox();
            pbx.Image = antHillImage;
            pbx.Region = antHillRegion;
            pbx.Width = 100;
            pbx.Height = 67;
            return pbx;
        }
     

        private static Region GetRegion(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            GraphicsPath path = new GraphicsPath();
            Color mask = bmp.GetPixel(0, 0);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (!bmp.GetPixel(x, y).Equals(mask))
                    {
                        path.AddRectangle(new Rectangle(x, y, 1, 1));
                    }
                }
            }

            return new Region(path);
        }
    }
}

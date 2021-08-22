using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace Ant_Hill
{
    // класс-издатель
    abstract class FieldPublisher
    {
        private static object locker = new object();

        protected FrmAntHill field;
        protected HashSet<Insenct> subscribers;
        protected HashSet<PictureBox> items;
        protected Thread thread;

        public FieldPublisher()
        {
            field = FrmAntHill.GetInstance();
            subscribers = new HashSet<Insenct>();
            items = new HashSet<PictureBox>();
            thread = new Thread(new ThreadStart(RunThread));
        }

        abstract protected void RunThread();

        // добавление подписчика
        public void AddSubscriber(Insenct subscriber)
        {
            lock (locker)
            {
                subscribers.Add(subscriber);
            }
        }

        // удаление подписчика
        public void RemoveSubscriber(Insenct subscriber)
        {
            lock (locker)
            {
                subscribers.Remove(subscriber);
            }
        }

        // оповещение подписчиков
        protected void NotifySubscribers(PictureBox item, bool isAvailable)
        {
            lock (locker)
            {
                foreach (Insenct subscriber in subscribers)
                {
                    subscriber.Update(item, isAvailable);
                }
                    
            }
        }

        // оповещение подписчика
        public void NotifySubscriber(Insenct subscriber)
        {
            lock (locker)
            {
                foreach (PictureBox item in items)
                    subscriber.Update(item, true);
            }
        }

        // обновление состояния предмета 
        public void UpdateStateOfItem(PictureBox item, bool isAvailable)
        {
            lock (locker)
            {
                if (isAvailable)
                {
                    items.Add(item);
                    item.MouseClick += ItemClick;
                }
                else
                {
                    items.Remove(item);
                    item.MouseClick -= ItemClick;
                }
                NotifySubscribers(item, isAvailable);
            }
        }

        // создание элемента
        protected void CreateItem(Point Location, PictureBox pbx)
        {
            PictureBox newItem = pbx;
            newItem.MouseClick += ItemClick;
            newItem.Cursor = Cursors.Hand;
            newItem.Location = Location;
            field.Add(newItem);
            field.SendToBack(newItem);
            items.Add(newItem);
            NotifySubscribers(newItem, true);
        }


        // обработка нажатия на предмет
        protected void ItemClick(object sender, EventArgs e)
        {
            PictureBox unavailableItem = (PictureBox)(sender);
            UpdateStateOfItem(unavailableItem, false);
            field.Remove(unavailableItem);
        }
    }
}

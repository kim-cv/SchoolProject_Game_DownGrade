﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DownGrade.Interfaces;
using Microsoft.Xna.Framework;

namespace DownGrade
{
    class CollisionHandler : IUpdateable, ISubject
    {
        //Eager instanciation
        private static readonly CollisionHandler _instance = new CollisionHandler();

        public static CollisionHandler Instance
        {
            get { return _instance; }
        }


        // A Collection to keep track of all Registered Observers
        List<Sprite> observers = new List<Sprite>();


        public CollisionHandler()
        {
        }

        public void Update(GameTime gameTime)
        {
            List<Sprite> tempList = observers.ToList();

            foreach (Sprite sprite in tempList)
            {
                foreach (Sprite sprite1 in tempList)
                {
                    if (!sprite.Equals(sprite1) && sprite.BoundingBox.Intersects(sprite1.BoundingBox))
                    {
                        notify(sprite, sprite1);
                    }
                }
            }
        }

        public bool Enabled { get; private set; }
        public int UpdateOrder { get; private set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public void register(Sprite o)
        {
            observers.Add(o);
        }

        public void unregister(Sprite o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.RemoveAt(i);
            }
        }

        public void notify(Sprite sp1, Sprite sp2)
        {
            sp1.Collide(sp2);
        }

        public void UnloadContent()
        {
            observers.Clear();
        }
    }
}

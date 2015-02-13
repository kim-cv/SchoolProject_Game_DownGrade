using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DownGrade
{
    public class Spawner
    {
        private float xPosition;
        private float yPosition;
        Random rnd = new Random();

        public void Spawn(Sprite sprite, List<Sprite> liste)
        {
            xPosition = rnd.Next(100, 500);
            yPosition = 0;

            liste.Add(sprite);
            sprite.Position = new Vector2(xPosition, yPosition);

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DownGrade
{
    class AudioHandler
    {
        public enum TypeOfSound
        {
            Explosion,
            Laser_Shoot2,
            Laser_Shoot4,
            Game_Music,
            Title_Music,
            GameOver_Music
        }

        private Game GameReference = null;

        private static readonly AudioHandler _instance = new AudioHandler();
        public static AudioHandler Instance
        {
            get { return _instance; }
        }

        private AudioHandler(){}

        public void SetGameReference(Game reference)
        {
            GameReference = reference;
        }


        public SoundEffect LoadSoundEffect(TypeOfSound typeOfSound)
        {
            switch (typeOfSound)
            {
                case TypeOfSound.Explosion:
                    {
                        return GameReference.Content.Load<SoundEffect>("explosion");
                    }
                case TypeOfSound.Laser_Shoot2:
                    {
                        return GameReference.Content.Load<SoundEffect>("Laser_Shoot2");
                    }
                case TypeOfSound.Laser_Shoot4:
                    {
                        return GameReference.Content.Load<SoundEffect>("Laser_Shoot4");
                    }
                case TypeOfSound.Game_Music:
                    {
                        return GameReference.Content.Load<SoundEffect>("Game_Music");
                    }
                case TypeOfSound.Title_Music:
                    {
                        return GameReference.Content.Load<SoundEffect>("Title_Music");
                    }
                case TypeOfSound.GameOver_Music:
                    {
                        return GameReference.Content.Load<SoundEffect>("GameOver_Music");
                    }
                default:
                    {
                        return null;
                    }
            }    
        }
    }
}

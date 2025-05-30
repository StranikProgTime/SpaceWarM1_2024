﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceWar.Classes.SaveData
{
    public class PlayerData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int Score { get; set; }
        public int Timer { get; set; }
        public List<BulletData> Bullets { get; set; }
    }
}

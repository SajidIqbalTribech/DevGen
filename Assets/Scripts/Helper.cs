using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    [Serializable]
    public class Powerup
    {
        public string name;
        public string description;
        public Sprite logo;
        public float price = 3;
        public bool isPurchase = false;
        public bool isActive = false;
        public float timeInHours = 259200;
        public bool isExpired = false;
    }
}

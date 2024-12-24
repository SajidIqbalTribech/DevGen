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
        public string ClaimedDescriptionBefore;
        public string ClaimedDescriptionAfter;
        public Sprite logo;
        public int CardInventory;
        public float price = 3;
        public bool isPurchase = false;
        public bool isActive = false;
        //public float timeInHours = 259200;
        public bool isExpired = false;
        public bool isDiscard = false;
    }
}

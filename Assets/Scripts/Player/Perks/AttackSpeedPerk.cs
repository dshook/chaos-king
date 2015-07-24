﻿using UnityEngine;
using System;

namespace Player
{
    public class AttackSpeedPerk : IPerk
    {
        PlayerShooting shooting;

        public int level { get; set; }
        public float amount { get; set; }
        public Sprite icon {
            get
            {
                return Resources.Load<Sprite>("bullets");
            }
        }
        public Color iconColor
        {
            get
            {
                return new Color(61, 23, 3);
            }
        }

        public AttackSpeedPerk(GameObject player) {
            shooting = player.GetComponentInChildren<PlayerShooting>();
        }

        public string GetDescription(int level) {
            return string.Format("{0} +{1}% Attack Speed", level, NextAmount(level) );
        }

        public IPerk ApplyPerk(int level) {
            this.level = level;
            amount = NextAmount(level);

            shooting.attackSpeedMultiplier -= 0.01f * amount;

            return this;
        }

        float NextAmount(int level) {
            return 5f;
        }
    }
}

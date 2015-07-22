using UnityEngine;
using System;

namespace Player
{
    public class DamagePerk : IPerk
    {
        //PlayerShooting shooting;

        public int level { get; set; }
        public decimal amount { get; set; }
        public Sprite icon {
            get
            {
                return Resources.Load<Sprite>("gunshot");
            }
        }
        public Color iconColor
        {
            get
            {
                return new Color(61, 23, 3);
            }
        }

        public DamagePerk(GameObject player) {
            //shooting = player.GetComponentInChildren<PlayerShooting>();
        }

        public string GetDescription(int level) {
            return string.Format("{0} +{1}% Damage", level, NextAmount(level) );
        }

        public IPerk ApplyPerk(int level) {
            this.level = level;
            amount = NextAmount(level);

            //shooting.damagePerShot *= (int)Math.Round(1 + (amount / 100m), 0, MidpointRounding.AwayFromZero);

            return this;
        }

        decimal NextAmount(int level) {
            return level;
        }
    }
}

using UnityEngine;
using System;

namespace Player
{
    [Id(2)]
    public class AmmoPerk : IPerk
    {
        PlayerShooting shooting;

        public int level { get; set; }
        public float amount { get; set; }
        public Sprite icon {
            get
            {
                return Resources.Load<Sprite>("ammo-box");
            }
        }
        public Color iconColor
        {
            get
            {
                return new Color(61, 23, 3);
            }
        }

        public AmmoPerk(GameObject player) {
            shooting = player.GetComponentInChildren<PlayerShooting>();
        }

        public string GetDescription(int level) {
            return string.Format("{0} {1}% More Bullets", level, NextAmount(level) );
        }

        public IPerk ApplyPerk(int level) {
            this.level = level;
            amount = NextAmount(level);

            shooting.ammoBoost += (int)amount;

            return this;
        }

        int NextAmount(int level) {
            return 5;
        }
    }
}

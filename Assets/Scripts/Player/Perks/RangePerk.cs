using UnityEngine;
using System;

namespace Player
{
    [Id(6)]
    public class RangePerk : IPerk
    {
        PlayerShooting shooting;

        public int level { get; set; }
        public float amount { get; set; }
        public Sprite icon {
            get
            {
                return Resources.Load<Sprite>("lob-arrow");
            }
        }
        public Color iconColor
        {
            get
            {
                return new Color(61, 23, 3);
            }
        }

        public RangePerk(GameObject player) {
            shooting = player.GetComponentInChildren<PlayerShooting>();
        }

        public string GetDescription(int level) {
            return string.Format("{0} +{1}% Range", level, NextAmount(level) );
        }

        public IPerk ApplyPerk(int level) {
            this.level = level;
            amount = NextAmount(level);

            shooting.rangeMultiplier += 0.01f * amount;

            return this;
        }

        float NextAmount(int level) {
            return 5f;
        }
    }
}

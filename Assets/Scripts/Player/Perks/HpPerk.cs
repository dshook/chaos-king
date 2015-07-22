using UnityEngine;
using System;

namespace Player
{
    public class HpPerk : IPerk
    {
        PlayerHealth health;

        public int level { get; set; }
        public decimal amount { get; set; }
        public Sprite icon {
            get
            {
                return Resources.Load<Sprite>("health-increase");
            }
        }
        public Color iconColor
        {
            get
            {
                return new Color(255, 22, 50);
            }
        }

        public HpPerk(GameObject player) {
            health = player.GetComponent<PlayerHealth>();
        }

        public string GetDescription(int level) {
            return string.Format("{0} +{1} HP", level, NextAmount(level) );
        }

        public IPerk ApplyPerk(int level) {
            this.level = level;
            health.IncreaseHealth((int)NextAmount(level));

            return this;
        }

        decimal NextAmount(int level) {
            return Math.Round((level + 10) / 2m, 0, MidpointRounding.AwayFromZero);
        }
    }
}

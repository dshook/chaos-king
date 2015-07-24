using UnityEngine;
using System;

namespace Player
{
    public class MoveSpeedPerk : IPerk
    {
        PlayerMovement movement;

        public int level { get; set; }
        public float amount { get; set; }
        public Sprite icon {
            get
            {
                return Resources.Load<Sprite>("run");
            }
        }
        public Color iconColor
        {
            get
            {
                return new Color(61, 23, 3);
            }
        }

        public MoveSpeedPerk(GameObject player) {
            movement = player.GetComponentInChildren<PlayerMovement>();
        }

        public string GetDescription(int level) {
            return string.Format("{0} +{1}% Move Speed", level, NextAmount(level) );
        }

        public IPerk ApplyPerk(int level) {
            this.level = level;
            amount = NextAmount(level);

            movement.speed += movement.speed * ((float)amount / 100f);

            return this;
        }

        float NextAmount(int level) {
            return 5f;
        }
    }
}

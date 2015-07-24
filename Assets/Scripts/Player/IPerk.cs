using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    public interface IPerk
    {
        Sprite icon { get; }
        Color iconColor { get; }
        int level { get; set; }
        float amount { get; set; }
        string GetDescription(int level);
        IPerk ApplyPerk(int level);
    }
}

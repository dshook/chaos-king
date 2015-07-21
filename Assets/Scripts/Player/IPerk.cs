﻿using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    public interface IPerk
    {
        Sprite icon { get; }
        int level { get; set; }
        decimal amount { get; set; }
        string GetDescription(int level);
        IPerk ApplyPerk(int level);
    }
}

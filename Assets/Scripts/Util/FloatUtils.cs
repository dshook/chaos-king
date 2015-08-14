using UnityEngine;

namespace Util
{
    public class FloatUtils
    {
        public static bool CloseEnough(float a, float b, float threshold = 0.001f)
        {
            if (Mathf.Abs(a - b) > threshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

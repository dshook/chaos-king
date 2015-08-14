using UnityEngine;

namespace Util
{
    public class FloatUtils
    {
        public static bool CloseEnough(float a, float b, float epsilon = 0.001f)
        {
            float absA = Mathf.Abs(a);
            float absB = Mathf.Abs(b);
            float diff = Mathf.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || diff < float.MinValue)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * float.MinValue);
            }
            else
            { // use relative error
                return diff / Mathf.Min((absA + absB), float.MaxValue) < epsilon;
            }
        }
    }
}

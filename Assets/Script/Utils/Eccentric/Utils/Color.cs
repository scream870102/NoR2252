using UnityEngine;
namespace Eccentric.Utils {
    public class Color {
        public static Vector3 RGB2HSV (UnityEngine.Color rgb) {
            Vector3 o = new Vector3 ( );
            UnityEngine.Color.RGBToHSV (rgb, out o.x, out o.y, out o.z);
            return o;
        }
        public static UnityEngine.Color HSV2RGB (Vector3 hsv) {
            return UnityEngine.Color.HSVToRGB (hsv.x, hsv.y, hsv.z);
        }
        public static UnityEngine.Color SetAlpha (UnityEngine.Color rgb, float alpha) {
            float a = alpha / 255f;
            rgb.a = a;
            return rgb;
        }
        public static UnityEngine.Color SetAlpha01 (UnityEngine.Color rgb, float alpha) {
            rgb.a = alpha;
            return rgb;
        }
        public static UnityEngine.Color SetAlphaHalf (UnityEngine.Color rgb) {
            rgb.a = rgb.a / 2f;
            return rgb;
        }

    }
}

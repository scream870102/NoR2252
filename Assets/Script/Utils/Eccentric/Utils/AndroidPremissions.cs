using arp = AndroidRuntimePermissions;
namespace Eccentric.Utils {
    public class AndroidAskRuntimePermission {
        /// <summary>This is all permission string <see href="https://developer.android.com/reference/android/Manifest.permission.html">HERE</see></summary>
        /// <remarks>This is the asset link <a href="https://github.com/yasirkula/UnityAndroidRuntimePermissions">HERE</a></remarks>
        public void GetPermission (string permission) {
            arp.Permission result = AndroidRuntimePermissions.RequestPermission (permission);
        }
    }
}

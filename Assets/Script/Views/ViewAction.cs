using UnityEngine;
namespace NoR2252.View.Action {
    public class RotationAction {
        bool bClockWise;
        float velocity;
        Transform transform;
        public RotationAction (bool IsClockWise, float Velocity, ref Transform Transform) {
            this.bClockWise = IsClockWise;
            this.velocity = Velocity;
            this.transform = Transform;
        }
        public void Tick ( ) {
            Vector3 rotation = transform.rotation.eulerAngles;
            if (bClockWise) rotation.z += velocity * Time.deltaTime;
            else rotation.z -= velocity * Time.deltaTime;
            transform.rotation = Quaternion.Euler (rotation);
        }
    }

    public class PingPongScale {
        Transform transform;
        float transVelo;
        float biggest;
        Vector3 init;
        public PingPongScale (float TransitionVelocity, float BiggestScale, ref Transform Transform) {
            this.transform = Transform;
            this.transVelo = TransitionVelocity;
            this.biggest = BiggestScale;
            init = Transform.localScale;
        }

        public void Tick ( ) {
            Vector3 newScale = init;
            newScale.x = Mathf.PingPong (Time.time * transVelo, biggest) + init.x;
            newScale.y = Mathf.PingPong (Time.time * transVelo, biggest) + init.y;
            transform.localScale = newScale;
        }
    }
}

using UnityEngine;
namespace NoR2252.View.Action {
    /// <summary>keep rotate the transform due to velocity</summary>
    /// <remarks>call TickX/Y/Z to rotate in different coordinate</remarks>
    public class RotationAction {
        bool bClockWise;
        float velocity;
        Transform transform;
        public RotationAction (bool IsClockWise, float Velocity, ref Transform Transform) {
            this.bClockWise = IsClockWise;
            this.velocity = Velocity;
            this.transform = Transform;
        }
        public void TickZ ( ) {
            Vector3 rotation = transform.rotation.eulerAngles;
            if (bClockWise) rotation.z += velocity * Time.deltaTime;
            else rotation.z -= velocity * Time.deltaTime;
            transform.rotation = Quaternion.Euler (rotation);
        }
        public void TickY ( ) {
            Vector3 rotation = transform.rotation.eulerAngles;
            if (bClockWise) rotation.y += velocity * Time.deltaTime;
            else rotation.y -= velocity * Time.deltaTime;
            transform.rotation = Quaternion.Euler (rotation);
        }
        public void TickX ( ) {
            Vector3 rotation = transform.rotation.eulerAngles;
            if (bClockWise) rotation.x += velocity * Time.deltaTime;
            else rotation.x -= velocity * Time.deltaTime;
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

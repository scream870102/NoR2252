namespace Eccentric.Utils {
    /// <summary>a countdown timer easy to use</summary>
    /// <remarks>call method Reset to reset timer and call property IsFinshed to check if countdown finished</remarks>
    public class CountdownTimer {
        float timeSection;
        float timer;
        /// <summary>remaining time until the countdown end</summary>
        public float Remain {
            get {
                float offset = timer - UnityEngine.Time.time;
                if (offset >= 0f)
                    return offset;
                else return 0f;
            }
        }
        /// <summary>if this countdown finished or not</summary>
        public bool IsFinished {
            get {
                if (timer <= UnityEngine.Time.time) return true;
                else return false;
            }
        }

        public CountdownTimer (float timeSection=0f) {
            this.timeSection = timeSection;
            Reset ( );
        }
        /// <summary>Reset countdown timer with default setting</summary>
        public void Reset ( ) {
            timer = UnityEngine.Time.time + timeSection;
        }
        /// <summary>reset countdown timer with new timeSection</summary>
        public void Reset (float timeSection) {
            timer = UnityEngine.Time.time + timeSection;
        }

    }
}

using System;

namespace Paps
{
    public class SyncTimer
    {
        public float Interval { get; set; }
        public bool Loop { get; set; }

        public float CurrentTime { get; private set; }
        public float TimeLeft
        {
            get
            {
                return Interval - CurrentTime;
            }
        }

        private bool active;

        public bool Active
        {
            get
            {
                return active && !Paused;
            }
        }

        public bool Paused { get; private set; }
        
        public event Action<SyncTimer> onTick;

        public SyncTimer()
        {

        }

        public void Start()
        {
            CurrentTime = 0;
            active = true;
            Paused = false;
        }

        public void Update(float elapsed)
        {
            if(Active)
            {
                CurrentTime += elapsed;

                if (CurrentTime >= Interval)
                {
                    if (!Loop)
                    {
                        Stop();
                    }
                    else
                    {
                        CurrentTime = 0;
                    }

                    if (onTick != null)
                    {
                        onTick(this);
                    }
                }
            }
        }

        public void Stop()
        {
            active = false;
            UnPause();
        }

        public void Pause()
        {
            Paused = true;
        }

        public void UnPause()
        {
            Paused = false;
        }
    }
}

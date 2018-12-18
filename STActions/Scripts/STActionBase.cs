namespace STToolBox.ActionKit
{
    using UnityEngine;
    public abstract class STActionBase
    {
        public Transform transform;

        public string key;
        public bool isActive = true;
        public bool actionComplete = false;
        public STActionType type;

        public float elapsedTime { get; protected set; }
        protected float _runTime;
        public float runTime { get { return GetRuntime(); } set { _runTime = value; } }
        public float percentComplete { get { return Mathf.Clamp01(elapsedTime / runTime); } }
        // -1 to repeat forever
        public int iterations = 1;

        protected Vector3 startVector;
        protected Vector3 endVector;

        public delegate void ActionCallback();
        public event ActionCallback ActionCanceled;
        public event ActionCallback ActionComplete;

        public virtual float UpdateAction(float deltaTime)
        {
            float runTime = this.runTime;
            elapsedTime += deltaTime;

            // This is where the actions actually take place.
            UpdateValues();

            if (elapsedTime >= runTime)
            {
                Debug.Log(elapsedTime + " " + runTime);
                if (iterations >= 0)
                {
                    iterations--;
                    if (iterations == 0)
                    {
                        // Action is done, this is how much time is left
                        InvokeActionComplete();
                        return elapsedTime - runTime;
                    }
                }

                // Action has to reset and run again
                ResetAction(elapsedTime - runTime);
                if (elapsedTime > 0)
                {
                    // The remainder of time that was not used goes towards starting the action again a second time
                    UpdateValues();
                }
            }
            // Action is not done, no time remains
            return 0;
        }

        protected abstract void UpdateValues();
        public virtual void ResetAction(float timeLeftInFrame = 0)
        {
            elapsedTime = timeLeftInFrame;
            actionComplete = false;
        }

        protected virtual float GetRuntime()
        {
            return _runTime;
        }

        protected virtual void InvokeActionCanceled()
        {
            if (ActionCanceled != null)
            {
                ActionCanceled();
            }
        }

        protected virtual void InvokeActionComplete()
        {
            if (ActionComplete != null)
            {
                actionComplete = true;
                ActionComplete();
            }
        }
    }

    public enum STActionType
    {
        Transform,
        Block,
        Wait,
        Group
    }
}

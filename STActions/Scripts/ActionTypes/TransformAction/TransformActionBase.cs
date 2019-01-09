namespace STToolBox.ActionKit
{
    using UnityEngine;
    public abstract class TransformActionBase : STActionBase
    {
        public bool endIsStatic = false;
        protected Vector3 startVector;
        protected Vector3 endVector;
        protected Vector3 lastValue;

        public TransformActionBase(Vector3 start, Vector3 end, float seconds)
        {
            lastValue = startVector = start;
            endVector = end;

            elapsedTime = 0;
            _runTime = seconds;
            type = STActionType.Transform;
        }

        protected override void UpdateValues()
        {
            Vector3 newValue = Vector3.Lerp(startVector, endVector, CustomLerpTime(elapsedTime / _runTime));
            UpdateActiveVector(newValue);
            lastValue = newValue;
        }

        protected abstract void UpdateActiveVector(Vector3 vector);
        protected virtual float CustomLerpTime(float value)
        {
            return Mathf.Clamp01(value);
        }
    }
}

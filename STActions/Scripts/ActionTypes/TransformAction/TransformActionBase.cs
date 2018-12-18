namespace STToolBox.ActionKit
{
    using UnityEngine;
    public abstract class TransformActionBase : STActionBase
    {
        public TransformActionBase(Vector3 start, Vector3 end, float seconds)
        {
            startVector = start;
            endVector = end;

            elapsedTime = 0;
            _runTime = seconds;
            type = STActionType.Transform;
        }

        protected override void UpdateValues()
        {
            UpdateActiveVector(Vector3.Lerp(startVector, endVector, CustomLerp(elapsedTime / _runTime)));
        }

        protected abstract void UpdateActiveVector(Vector3 vector);
        protected virtual float CustomLerp(float value)
        {
            return Mathf.Clamp01(value);
        }
    }
}

namespace STToolBox.ActionKit
{
    using UnityEngine;
    public class RotateAction : TransformActionBase
    {
        public const string defaultKey = "rotate_action";
        public Vector3 startRotation { get { return startVector; } set { startVector = value; } }
        public Vector3 endRotation { get { return endVector; } set { endVector = value; } }
        public bool useWorldRotation;

        public RotateAction(Vector3 from, Vector3 to, float seconds, bool worldRotation = false, bool endIsStatic = false, string key = defaultKey) : base(from, to, seconds)
        {
            useWorldRotation = worldRotation;
            this.endIsStatic = endIsStatic;
            this.key = key;
        }

        protected override void UpdateActiveVector(Vector3 vector)
        {
            if (useWorldRotation)
            {
                transform.rotation = Quaternion.Euler(endIsStatic ? vector : transform.rotation.eulerAngles + vector - lastValue);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(endIsStatic ? vector : transform.localRotation.eulerAngles + vector - lastValue);
            }
        }
    }
}

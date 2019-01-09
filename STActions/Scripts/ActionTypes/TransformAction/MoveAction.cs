namespace STToolBox.ActionKit
{
    using UnityEngine;
    public class MoveAction : TransformActionBase
    {
        public const string defaultKey = "move_action";
        public Vector3 startPosition { get { return startVector; } set { startVector = value; } }
        public Vector3 endPosition { get { return endVector; } set { endVector = value; } }
        public bool useWorldPosition;

        public MoveAction(Vector3 from, Vector3 to, float seconds, bool worldPosition = false, bool endIsStatic = false, string key = defaultKey) : base(from, to, seconds)
        {
            useWorldPosition = worldPosition;
            this.endIsStatic = endIsStatic;
            this.key = key;
        }

        protected override void UpdateActiveVector(Vector3 vector)
        {
            if (useWorldPosition)
            {
                transform.position = endIsStatic ? vector : transform.position + vector - lastValue;
            }
            else
            {
                transform.localPosition = endIsStatic ? vector : transform.localPosition + vector - lastValue;
            }
        }
    }
}

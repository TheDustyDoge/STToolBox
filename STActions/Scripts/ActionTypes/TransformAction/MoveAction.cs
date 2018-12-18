namespace STToolBox.ActionKit
{
    using UnityEngine;
    public class MoveAction : TransformActionBase
    {
        public const string defaultKey = "move_action";
        public Vector3 startPosition { get { return startVector; } set { startVector = value; } }
        public Vector3 endPosition { get { return endVector; } set { endVector = value; } }
        public bool use2DMovement = false;
        public bool useWorldPosition;

        public MoveAction(Vector3 from, Vector3 to, float seconds, bool worldPosition = false, string key = defaultKey) : base(from, to, seconds)
        {
            useWorldPosition = worldPosition;
            this.key = key;
        }

        protected override void UpdateActiveVector(Vector3 vector)
        {
            if (useWorldPosition)
            {
                if (use2DMovement)
                {
                    vector.z = transform.position.z;
                }
                transform.position = vector;
            }
            else
            {
                if (use2DMovement)
                {
                    vector.z = transform.localPosition.z;
                }
                transform.localPosition = vector;
            }
        }
    }
}

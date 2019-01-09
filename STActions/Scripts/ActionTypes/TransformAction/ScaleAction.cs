using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STToolBox.ActionKit
{
    public class ScaleAction : TransformActionBase
    {
        public const string defaultKey = "scale_action";
        public Vector3 startScale { get { return startVector; } set { startVector = value; } }
        public Vector3 endScale { get { return endVector; } set { endVector = value; } }
        //public bool useWorldScale;

        public ScaleAction(Vector3 from, Vector3 to, float seconds, bool endIsStatic = false, string key = defaultKey) : base(from, to, seconds)
        {
            this.endIsStatic = endIsStatic;
            this.key = key;
        }

        protected override void UpdateActiveVector(Vector3 vector)
        {
            transform.localScale = endIsStatic ? vector : transform.localScale + vector - lastValue;
        }
    }
}

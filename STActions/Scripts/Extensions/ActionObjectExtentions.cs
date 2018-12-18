using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STToolBox.ActionKit
{
    // =================== \\
    // = Action Creation = \\
    // =================== \\

    public static class STActions
    {
        // ============= \\
        // = Transform = \\
        // ============= \\

        public static void MoveBy(this Transform transform, Vector3 offset, float seconds, string key = MoveAction.defaultKey)
        {
            transform.RunAction(new MoveAction(transform.position, transform.position + offset, seconds, key: key));
        }
        public static void MoveTo(this Transform transform, Vector3 offset, float seconds, string key = MoveAction.defaultKey)
        {
            transform.RunAction(new MoveAction(transform.position, offset, seconds, true, key));
        }
        public static void MoveToLocal(this Transform transform, Vector3 offset, float seconds, string key = MoveAction.defaultKey)
        {
            transform.RunAction(new MoveAction(transform.localPosition, offset, seconds, key: key));
        }

        public static void RotateBy(this Transform transform, Vector3 offset, float seconds, string key = RotateAction.defaultKey)
        {
            transform.RunAction(new RotateAction(transform.rotation.eulerAngles, transform.rotation.eulerAngles + offset, seconds, key: key));
        }
        public static void RotateTo(this Transform transform, Vector3 offset, float seconds, string key = RotateAction.defaultKey)
        {
            transform.RunAction(new RotateAction(transform.rotation.eulerAngles, offset, seconds, true, key));
        }
        public static void RotateToLocal(this Transform transform, Vector3 offset, float seconds, string key = RotateAction.defaultKey)
        {
            transform.RunAction(new RotateAction(transform.localRotation.eulerAngles, offset, seconds, key: key));
        }

        public static void ScaleBy(this Transform transform, Vector3 offset, float seconds, string key = ScaleAction.defaultKey)
        {
            transform.RunAction(new ScaleAction(transform.localScale, Mathmatics.Vector3Extentions.Multiply(transform.localScale, offset), seconds, key));
        }
        public static void ScaleTo(this Transform transform, Vector3 offset, float seconds, string key = ScaleAction.defaultKey)
        {
            transform.RunAction(new ScaleAction(transform.localScale, offset, seconds, key));
        }

        // =============== \\
        // = Game Object = \\
        // =============== \\

        public static void MoveBy(this GameObject gameObject, Vector3 offset, float seconds, string key = MoveAction.defaultKey)
        {
            gameObject.transform.MoveBy(offset, seconds, key);
        }
        public static void MoveTo(this GameObject gameObject, Vector3 offset, float seconds, string key = MoveAction.defaultKey)
        {
            gameObject.transform.MoveTo(offset, seconds, key);
        }
        public static void MoveToLocal(this GameObject gameObject, Vector3 offset, float seconds, string key = MoveAction.defaultKey)
        {
            gameObject.transform.MoveToLocal(offset, seconds, key);
        }

        public static void RotateBy(this GameObject gameObject, Vector3 offset, float seconds, string key = RotateAction.defaultKey)
        {
            gameObject.transform.RotateBy(offset, seconds, key);
        }
        public static void RotateTo(this GameObject gameObject, Vector3 offset, float seconds, string key = RotateAction.defaultKey)
        {
            gameObject.transform.RotateTo(offset, seconds, key);
        }
        public static void RotateToLocal(this GameObject gameObject, Vector3 offset, float seconds, string key = RotateAction.defaultKey)
        {
            gameObject.transform.RotateToLocal(offset, seconds, key);
        }

        public static void ScaleBy(this GameObject gameObject, Vector3 offset, float seconds, string key = ScaleAction.defaultKey)
        {
            gameObject.transform.ScaleBy(offset, seconds, key);
        }
        public static void ScaleTo(this GameObject gameObject, Vector3 offset, float seconds, string key = ScaleAction.defaultKey)
        {
            gameObject.transform.ScaleTo(offset, seconds, key);
        }



    }

    // =========================== \\
    // = Unity Object Extentions = \\
    // =========================== \\

    public static class ActionObjectExtentions
    {
        // ============= \\
        // = Transform = \\
        // ============= \\

        public static void RunAction(this Transform transform, STActionBase action)
        {
            transform.gameObject.RunAction(action);
        }

        // =============== \\
        // = Game Object = \\
        // =============== \\

        public static void RunAction(this GameObject gameObject, STActionBase action)
        {
            gameObject.GetActionObject().AddAction(action);
        }

        public static ActionObject GetActionObject(this GameObject gameObject)
        {
            ActionObject ao = gameObject.GetComponent<ActionObject>();
            if (ao == null)
            {
                ao = gameObject.AddComponent<ActionObject>();
            }
            return ao;
        }



    }
}

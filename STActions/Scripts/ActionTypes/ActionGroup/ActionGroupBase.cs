namespace STToolBox.ActionKit
{
    using System.Collections.Generic;

    public class ActionGroupBase : STActionBase
    {
        public List<STActionBase> subActions;

        public ActionGroupBase(STActionBase[] subActions)
        {
            type = STActionType.Group;
            for (int i = 0; i < subActions.Length; i++)
            {
                AddAction(subActions[i]);
            }
        }

        protected override void UpdateValues() { }
        public virtual void AddAction(STActionBase action)
        {
            subActions.Add(action);
        }

        public override void ResetAction(float timeLeftInFrame = 0)
        {
            for (int i = 0; i < subActions.Count; i++)
            {
                subActions[i].ResetAction(timeLeftInFrame);
            }

            base.ResetAction(timeLeftInFrame);
        }
    }
}
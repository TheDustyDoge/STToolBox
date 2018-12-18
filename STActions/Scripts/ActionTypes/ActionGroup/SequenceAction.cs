namespace STToolBox.ActionKit
{
    public class SequenceAction : ActionGroupBase
    {
        public SequenceAction(STActionBase[] subActions) : base(subActions) { }

        protected override float GetRuntime()
        {
            float totalTime = 0;
            for (int i = 0; i < subActions.Count; i++)
            {
                totalTime += subActions[i].runTime;
            }
            return totalTime;
        }

        public override float UpdateAction(float deltaTime)
        {
            float ret = base.UpdateAction(deltaTime);
            for (int i = 0; i < subActions.Count; i++)
            {
                if (!subActions[i].actionComplete)
                {
                    float remainder = subActions[i].UpdateAction(deltaTime);
                    if (remainder > 0 && i + 1 < subActions.Count)
                    {
                        subActions[i + 1].UpdateAction(remainder);
                    }
                    return ret;
                }
            }
            return ret;
        }
    }
}

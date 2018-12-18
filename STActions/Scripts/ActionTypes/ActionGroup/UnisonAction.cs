namespace STToolBox.ActionKit
{
    public class UnisonAction : ActionGroupBase
    {
        public UnisonAction(STActionBase[] subActions) : base(subActions) { }

        protected override float GetRuntime()
        {
            float longestTime = 0;
            for (int i = 0; i < subActions.Count; i++)
            {
                if (subActions[i].runTime > longestTime)
                {
                    longestTime = subActions[i].runTime;
                }
            }
            return longestTime;
        }

        public override float UpdateAction(float deltaTime)
        {
            float ret = base.UpdateAction(deltaTime);
            for (int i = 0; i < subActions.Count; i++)
            {
                if (!subActions[i].actionComplete)
                {
                    subActions[i].UpdateAction(deltaTime);
                }
            }
            return ret;
        }
    }
}

namespace STToolBox.ActionKit
{
    public class WaitAction : STActionBase
    {
        public const string defaultKey = "wait_action";
        public WaitAction(float seconds, string key = defaultKey)
        {
            elapsedTime = 0;
            _runTime = seconds;
            this.key = key;
            type = STActionType.Wait;
        }

        protected override void UpdateValues()
        {
            // Wait action already works as is
        }
    }
}

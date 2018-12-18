namespace STToolBox.ActionKit
{
    public class BlockAction : STActionBase
    {
        public const string defaultKey = "block_action";
        public event ActionCallback RunBlock;

        public BlockAction(ActionCallback block, string key = defaultKey)
        {
            RunBlock += block;
            this.key = key;
            type = STActionType.Block;
        }

        protected override void UpdateValues()
        {
            if (RunBlock != null)
            {
                RunBlock();
            }
        }
    }
}

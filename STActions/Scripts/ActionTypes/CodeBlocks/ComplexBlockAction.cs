namespace STToolBox.ActionKit
{
    using System.Collections.Generic;
    public class ComplexBlockAction<K, V> : STActionBase
    {
        public const string defaultKey = "block_action";
        public delegate void ComplexActionCallback(Dictionary<K, V> callbackData);
        public event ComplexActionCallback RunComplexBlock;
        public Dictionary<K, V> callbackData;

        public ComplexBlockAction(ComplexActionCallback block, Dictionary<K, V> callbackData, string key = defaultKey)
        {
            RunComplexBlock += block;
            this.callbackData = callbackData;
            this.key = key;
            type = STActionType.Block;
        }

        protected override void UpdateValues()
        {
            if (RunComplexBlock != null)
            {
                RunComplexBlock(callbackData);
            }
        }
    }
}

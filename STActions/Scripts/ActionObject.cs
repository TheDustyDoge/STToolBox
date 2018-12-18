namespace STToolBox.ActionKit
{
    using UnityEngine;
    using System.Collections.Generic;
    public class ActionObject : MonoBehaviour
    {
        public List<STActionBase> actions;
        public List<STActionBase> completedActions;

        private void Awake()
        {
            actions = new List<STActionBase>();
            completedActions = new List<STActionBase>();
        }

        // ==================== \\
        // = Public Functions = \\
        // ==================== \\

        public void AddAction(STActionBase action)
        {
            action.transform = transform;
            actions.Add(action);
        }

        public void RemoveAction(STActionBase action)
        {
            if (actions.Contains(action))
            {
                action.transform = null;
                actions.Remove(action);
            }
        }

        public STActionBase GetActionWithKey(string key)
        {
            for (int i = 0; i < actions.Count;)
            {
                if (actions[i].key == key)
                {
                    return actions[i];
                }
            }
            return null;
        }

        // ==================== \\
        // = Update Functions = \\
        // ==================== \\

        void Update()
        {
            UpdateActions(Time.deltaTime);
            RemoveCompletedActions();
        }

        void UpdateActions(float deltaTime)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                float timeRemaining = actions[i].UpdateAction(deltaTime);
                if (timeRemaining > 0)
                {
                    // Time remaining after action completed
                    completedActions.Add(actions[i]);
                }
            }
        }

        void RemoveCompletedActions()
        {
            for (int i = 0; i < completedActions.Count; i++)
            {
                actions.Remove(completedActions[i]);
            }
            completedActions.Clear();
        }
    }
}
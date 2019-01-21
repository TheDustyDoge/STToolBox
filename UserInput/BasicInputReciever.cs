using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicInputReciever : MonoBehaviour
{
    public bool debugMode = false;
    public int totalInputs = 0;
    public InputSystem.InputAction[] inputActions;

    public UnityEvent[] OnInputActivated;
    public UnityEvent[] OnInputDeactivated;

    private void Update()
    {
        for (int i = 0; i < inputActions.Length; i++)
        {
            if (inputActions[i] == InputSystem.InputAction.None)
                continue;
            
            if (InputSystem.GetInputStateDown(inputActions[i]))
                InvokeActivated(i);

            if (InputSystem.GetInputStateUp(inputActions[i]))
                InvokeDeactivated(i);
        }
    }

    // ==================== \\

    void InvokeActivated(int i)
    {
        if (OnInputActivated[i] != null)
            OnInputActivated[i].Invoke();
        if (debugMode)
            Debug.Log(gameObject.name + " recieved activation input " + inputActions[i]);
    }

    void InvokeDeactivated(int i)
    {
        if (OnInputDeactivated[i] != null)
            OnInputDeactivated[i].Invoke();
        if (debugMode)
            Debug.Log(gameObject.name + " recieved deactivation input " + inputActions[i]);
    }

}

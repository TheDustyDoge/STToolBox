using UnityEngine;

public static class InputSystem
{
    public const string MOVE_HORIZONTAL = "Horizontal";
    public const string MOVE_VERTICAL = "Vertical";
    
    public enum InputAction
    {
        None,
        MoveHorizontal,
        MoveVertical
    }

    // =========== \\
    // = Getters = \\
    // =========== \\

    public static bool GetInputStateUp(InputAction input)
    {
        return Input.GetButtonUp(GetStringFromAction(input));
    }

    public static bool GetInputStateDown(InputAction input)
    {
        return Input.GetButtonDown(GetStringFromAction(input));
    }

    public static bool GetInputState(InputAction input)
    {
        return Input.GetButton(GetStringFromAction(input));
    }

    public static float GetInputAxis(InputAction input)
    {
        return Input.GetAxis(GetStringFromAction(input));
    }

    public static string GetStringFromAction(InputAction action)
    {
        switch (action)
        {
            case InputAction.MoveHorizontal:
                return MOVE_HORIZONTAL;
            case InputAction.MoveVertical:
                return MOVE_VERTICAL;
        }

        Debug.LogError("No string associated with action " + action);
        return "";
    }
}

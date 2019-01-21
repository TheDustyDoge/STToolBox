using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAction
{
    public enum Type
    {
        Button,
        Axis
    }

    public string name;
    public Type type;

    public InputAction(string name)
    {
        this.name = name;
        type = Type.Button;
    }
}

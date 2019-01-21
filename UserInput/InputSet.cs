using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSet
{
    public string name;
    public List<InputAction> inputActions;

    public InputSet(string name)
    {
        this.name = name;
        inputActions = new List<InputAction>();
    }
}

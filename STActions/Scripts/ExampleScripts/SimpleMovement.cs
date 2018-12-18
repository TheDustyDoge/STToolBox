using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STToolBox.ActionKit;

public class SimpleMovement : MonoBehaviour
{
    public float moveTime = 0.25f;

    public void MoveUp()
    {
        gameObject.MoveBy(Vector3.forward, moveTime);
    }

    public void MoveDown()
    {
        gameObject.MoveBy(Vector3.back, moveTime);
    }

    public void MoveLeft()
    {
        gameObject.MoveBy(Vector3.left, moveTime);
    }

    public void MoveRight()
    {
        gameObject.MoveBy(Vector3.right, moveTime);
    }

    public void MoveCenter()
    {
        gameObject.MoveTo(Vector3.zero, moveTime);
    }

}

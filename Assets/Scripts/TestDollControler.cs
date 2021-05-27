using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDollControler : MonoBehaviour
{
    [SerializeField] DollManager dm;
    [SerializeField] bool d = false;
    [SerializeField] float leftPos = 0.8f;
    [SerializeField] float leftRot;
    [SerializeField] float rightPos = 0.8f;
    [SerializeField] float rightRot;
    [SerializeField] float left;
    [SerializeField] float right;

    public void RightPos(float newPos)
    {
        rightPos = newPos;
        dm.SetRightPalmPos(rightPos, rightRot);
    }

    public void RightRot(float newRot)
    {
        rightRot = newRot;
        dm.SetRightPalmPos(rightPos, rightRot);
    }

    public void LeftPos(float newPos)
    {
        leftPos = newPos;
        dm.SetLeftPalmPos(leftPos, leftRot);
    }

    public void LeftRot(float newRot)
    {
        leftRot = newRot;
        dm.SetLeftPalmPos(leftPos, leftRot);
    }

    public void RightFootRot(float newRot)
    {
        right = newRot;
        dm.SetRightFootRot(right);
    }

    public void LeftFootRot(float newRot)
    {
        left = newRot;
        dm.SetLeftFootRot(left);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) d = !d;
        if (d)
        {
            dm.SetRightFootRot(right);
            dm.SetLeftFootRot(left);
        }
    }
}

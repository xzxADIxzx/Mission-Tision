using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerControler : MonoBehaviour
{
    [SerializeField] private DollManager dm;

    void Update()
    {
        Vector3 cursorLocalPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorLocalPosition);
        Vector3 direction = (cursorPosition - transform.position - new Vector3(0, 0.6f, 0)).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        dm.SetLeftPalmPos(0.86f, angle);
        dm.SetRightPalmPos(0.56f, angle);
        direction = (cursorPosition - transform.position - new Vector3(0, 1.65f, 0)).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        dm.SetHeadRot(angle);
    }
}

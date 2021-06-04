using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollManager : MonoBehaviour
{
    [SerializeField] private Transform head;
    [Space(10)]
    [SerializeField] private Transform body;
    [Space(10)]
    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform[] leftArm;
    [SerializeField] private Transform leftPalm;
    [Space(10)]
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private Transform[] rightArm;
    [SerializeField] private Transform rightPalm;
    [Space(10)]
    [SerializeField] private Transform leftHip;
    [SerializeField] private Transform[] leftLeg;
    [SerializeField] private Transform leftFoot;
    [Space(10)]
    [SerializeField] private Transform rightHip;
    [SerializeField] private Transform[] rightLeg;
    [SerializeField] private Transform rightFoot;

    public void SetHeadRot(float rot)
    {
        rot = Mathf.Clamp(rot, -15, 15);
        head.rotation = Quaternion.Euler(0, 0, rot);
    }

    public void SetLeftPalmPos(float pos, float rot)
    {
        leftShoulder.localRotation = Quaternion.Euler(0, 0, rot);
        leftPalm.localPosition = new Vector3(pos, 0, 0);
        float a = pos;
        float b = 0.43f;
        float c = 0.43f;
        float angleA = Mathf.Acos((b * b + c * c - a * a) / (2 * b * c)) * 180 / Mathf.PI;
        float angleB = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * 180 / Mathf.PI;
        // float angleC = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * 180 / Mathf.PI;
        leftArm[0].localRotation = Quaternion.Euler(0, 0, -90 - angleB);
        leftArm[1].localRotation = Quaternion.Euler(0, 0, 180 - angleA);
    }

    public void SetRightPalmPos(float pos, float rot)
    {
        rightShoulder.localRotation = Quaternion.Euler(0, 0, rot);
        rightPalm.localPosition = new Vector3(pos, 0, 0);
        float a = pos;
        float b = 0.43f;
        float c = 0.43f;
        float angleA = Mathf.Acos((b * b + c * c - a * a) / (2 * b * c)) * 180 / Mathf.PI;
        float angleB = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * 180 / Mathf.PI;
        // float angleC = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * 180 / Mathf.PI;
        rightArm[0].localRotation = Quaternion.Euler(0, 0, -90 - angleB);
        rightArm[1].localRotation = Quaternion.Euler(0, 0, 180 - angleA);
    }

    public void SetLeftFootRot(float rot)
    {
        float pos = 2.6f;
        RaycastHit2D hit;
        Vector3 origin = transform.position + new Vector3(-0.1f * 3, -0.35f * 3, 0);
        Vector3 direction = leftFoot.position - origin;
        hit = Physics2D.Raycast(origin, direction, 2.6f);
        if (hit.collider != null) pos = hit.distance;

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction, new Color(1, 0.5f, 0));
#endif

        leftHip.localRotation = Quaternion.Euler(0, 0, rot);
        leftFoot.localPosition = new Vector3(pos / 3, 0, 0);
        float a = pos;
        float b = 1.3f;
        float c = 1.3f;
        float angleA = Mathf.Acos((b * b + c * c - a * a) / (2 * b * c)) * 180 / Mathf.PI;
        float angleB = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * 180 / Mathf.PI;
        // float angleC = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * 180 / Mathf.PI;
        leftLeg[0].localRotation = Quaternion.Euler(0, 0, -90 - angleB);
        leftLeg[1].localRotation = Quaternion.Euler(0, 0, 180 - angleA);
    }

    public void SetRightFootRot(float rot)
    {
        float pos = 2.6f;
        RaycastHit2D hit;
        Vector3 origin = transform.position + new Vector3(0.1f * 3, -0.35f * 3, 0);
        Vector3 direction = rightFoot.position - origin;
        hit = Physics2D.Raycast(origin, direction, 2.6f);
        if (hit.collider != null) pos = hit.distance;

#if UNITY_EDITOR
        Debug.DrawRay(origin, direction, new Color(1, 0.5f, 0));
#endif

        rightHip.localRotation = Quaternion.Euler(0, 0, rot);
        rightFoot.localPosition = new Vector3(pos / 3, 0, 0);
        float a = pos;
        float b = 1.3f;
        float c = 1.3f;
        float angleA = Mathf.Acos((b * b + c * c - a * a) / (2 * b * c)) * 180 / Mathf.PI;
        float angleB = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * 180 / Mathf.PI;
        // float angleC = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * 180 / Mathf.PI;
        rightLeg[0].localRotation = Quaternion.Euler(0, 0, -90 - angleB);
        rightLeg[1].localRotation = Quaternion.Euler(0, 0, 180 - angleA);
    }
}

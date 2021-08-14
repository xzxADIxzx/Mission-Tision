using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Transform obj;
    [SerializeField] private float speed = 0.42f;
    [Header("Smooth")]
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float xVel;
    [SerializeField] private float yVel;

    public void Follow(Transform obj)
    {
        this.obj = obj;
    }

    void Update()
    {
        if (obj == null) return;
        x = Mathf.SmoothDamp(x, obj.position.x, ref xVel, speed);
        y = Mathf.SmoothDamp(y, obj.position.y, ref yVel, speed);
        transform.position = new Vector3(x, y, -10f);
    }

    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCameraFollow : MonoBehaviour
{

    public Transform[] targets;

    public Vector3 offset;

    public float smoothTime;
    public float maxSpeed;

    public float minZoom;
    public float maxZoom;

    Vector3 vel;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPos = GetCenterPoint() + offset;

        transform.position = Vector3.SmoothDamp(transform.position, tempPos, ref vel, smoothTime, maxSpeed);

        cam.orthographicSize = GetSize();
    }



    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center; ;
    }

    float GetSize()
    {

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        float boundSize = bounds.size.x / 2;
        boundSize = Mathf.Clamp(boundSize, minZoom, maxZoom);
        

        return boundSize;
    }
}

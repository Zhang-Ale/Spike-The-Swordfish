using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlongPathMove : MonoBehaviour
{
    private List<Vector3> path = new List<Vector3>();
    public float v; //km/h   1000m/3600s = 10m/36s 
    public Transform point0, point1, point2, point3;
    private float totalLength; //total road's length
    private float currentS; //current walked length
    private int index = 0;
    public LineRenderer lineRenderer;
    public bool start;

    void Start()
    {
        path = BezierUtility.BezierInterpolate4List(point0.position, point1.position, point2.position, point3.position, 50);

        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());
    }
    Vector3 dir;
    Vector3 pos;

    private void Update()
    {
        if (start)
        {
            for (int i = 1; i < path.Count; i++)
            {
                totalLength += (path[i] - path[i - 1]).magnitude;
            }
        }
        else
        {
            transform.position = point0.position; 
        }
    }

    private void FixedUpdate()
    {
        float s = (v * 10 / 36) * Time.time;

        if (currentS < totalLength )
        {
            for(int i = index; i < path.Count-1; i++)
            {
                currentS += (path[i + 1] - path[i]).magnitude;
                if (currentS>s)
                {
                    index = i; 
                    currentS -= (path[i+1] - path[i]).magnitude;
                    dir = (path[i + 1] - path[i]).normalized;
                    pos = path[i] + dir * (s - currentS);
                    break;                
                }
            }
            transform.position = pos;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, transform.up), Time.deltaTime * 5);
        }
        else
        {
            Debug.Log("Destination reached"); 
        }
    }
}

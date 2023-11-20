using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class WaterFloatForVolume : MonoBehaviour
{
    private int sampleCount = 100;
    public float density = 1f;
    public float volume;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float waterHeight = 0f;

    Rigidbody rb;
    bool underWater;
    MeshRenderer mesh; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        volume = CalculateVolume(); 
    }

    void FixedUpdate()
    {
        rb.mass = volume + Mathf.Clamp(density, 0, density);
        var difference = transform.position.y - mesh.bounds.size.y / 2 - waterHeight;
        if(difference < 0)
        {
            rb.AddForceAtPosition(Vector3.up * volume * 9.81f * Mathf.Clamp(Mathf.Abs(difference),
                0, mesh.bounds.size.y / 2), transform.position, ForceMode.Impulse);
            if (!underWater)
            {
                underWater = true;
                SwitchState(true);
            }else if (underWater)
            {
                underWater = false;
                SwitchState(false);
            }
        }
    }

    void SwitchState(bool isUnderWater)
    {
        if (!isUnderWater)
        {
            rb.drag = underWaterAngularDrag;
            rb.angularDrag = underWaterAngularDrag;
        }
        else
        {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }

    bool isInCollider(MeshCollider other, Vector3 center, Vector3 point)
    {
        Vector3 direction = center - point;
        RaycastHit[] hits = Physics.RaycastAll(point, direction);
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider == other)
            {
                return false;
            }
        }

        return true; 
    }

    float CalculateVolume()
    {
        GameObject object1 = this.gameObject;
        MeshCollider object1_mc = object1.GetComponent<MeshCollider>();
        Vector3 object1_center = object1_mc.bounds.center;
        Matrix4x4 localToWorld_object1 = object1.transform.localToWorldMatrix;
        Vector3[] vertices_object1 = object1.GetComponent<MeshFilter>().mesh.vertices;
        float[] x = new float[vertices_object1.Length];
        float[] y = new float[vertices_object1.Length];
        float[] z = new float[vertices_object1.Length];

        for(int i = 0; i < vertices_object1.Length; i++)
        {
            Vector3 world_v = localToWorld_object1.MultiplyPoint3x4(vertices_object1[i]);
            x[i] = world_v.x;
            y[i] = world_v.y;
            z[i] = world_v.z;
        }

        Array.Sort(x);
        Array.Sort(y);
        Array.Sort(z);

        float x_length = x[x.Length - 1] - x[0];
        float y_length = y[y.Length - 1] - y[0];
        float z_length = z[z.Length - 1] - z[0];

        float lerp_x = x_length / sampleCount;
        float lerp_y = y_length / sampleCount;
        float lerp_z = z_length / sampleCount;
        int pointInside = 0; 
        for(int i = 0; i<sampleCount; i++)
        {
            for(int j = 0; j<sampleCount; j++)
            {
                for(int k = 0; k<sampleCount; k++)
                {
                    Vector3 sampleDot = new Vector4(x[0] + i * lerp_x, y[0] + j * lerp_y, z[0] + k * lerp_z);
                    if(isInCollider(object1_mc, object1_center, sampleDot))
                    {
                        pointInside++;
                    }
                }
            }
        }

        float volume = (float)pointInside / (sampleCount * sampleCount * sampleCount) * (x_length * y_length * z_length);
        Debug.Log("Volume: " + volume);
        return volume; 
    }
}

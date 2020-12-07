using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AvoidMesh : MonoBehaviour
{

    public GameObject tip;
    public Collider[] avoidMesh;

    public GameObject l_real;
    public GameObject l_display;
    public GameObject r_real;
    public GameObject r_display;

    public float wrappingOriginDist = 0.05f;
    public float maxRotationOffset = 90;
    void Start()
    {
        
    }

    void Update()
    {
        float dist = distanceToMesh();
        if (dist < wrappingOriginDist)
        {
            float ratio = maxRotationOffset * dist / wrappingOriginDist;

            //float l_x = l_real.transform.rotation.x;
            //float r_y = l_real.transform.rotation.z;

            //l_display.transform.Rotate(-ratio / 2, 0, 0);
            //r_display.transform.Rotate(0, 0, -ratio / 2);

            l_display.transform.rotation = Quaternion.Euler( l_real.transform.rotation.eulerAngles + new Vector3(90,0,0));
            r_display.transform.Rotate(0, 0, 90);
        }
        
    }

    float distanceToMesh()
    {
        float minDist = float.MaxValue;

        foreach(Collider m in avoidMesh)
        {
            Vector3 nearest = m.ClosestPointOnBounds(tip.transform.position);
            float dist = Vector3.Distance(nearest, tip.transform.position);
            if (dist < minDist);

        }

        return minDist;
    }
}

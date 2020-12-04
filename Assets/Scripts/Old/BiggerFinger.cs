using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BiggerFinger : MonoBehaviour
{
    public enum Finger { pinky, ring, middle, index };

    [Serializable]
    public struct TipPos
    {
        public Finger name;
        public GameObject obj;
    }
    public TipPos[] real;
    public TipPos[] display;

    Dictionary<Finger, float> distance = new Dictionary<Finger, float>();
    public Dictionary<Finger, GameObject> real_d = new Dictionary<Finger, GameObject>();
    public Dictionary<Finger, GameObject> display_d = new Dictionary<Finger, GameObject>();

    public GameObject r_tip;

    public GameObject r_displayed;
    public GameObject r_real;

    private float retargetingStart = 0.2f;

    private void Start()
    {
        foreach (TipPos tip in real) real_d.Add(tip.name, tip.obj);
        foreach (TipPos tip in display) display_d.Add(tip.name, tip.obj);
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Finger finger in Enum.GetValues(typeof(Finger)))
        {
            distance[finger] = Vector3.Distance(r_tip.transform.position, real_d[finger].transform.position);
        }

        float smallestDist = float.MaxValue;
        Finger smallestFinger = Finger.index;

        foreach (KeyValuePair<Finger, float> dist in distance)
        {
            if(dist.Value < smallestDist)
            {
                smallestDist = dist.Value; 
                smallestFinger = dist.Key; 
            }
        }

        float alpha = 1 - Mathf.Min(1, smallestDist / retargetingStart);

        Vector3 targetDistDiff = real_d[smallestFinger].transform.position - display_d[smallestFinger].transform.position;
        r_displayed.transform.position = r_real.transform.position - targetDistDiff * alpha;

    }
}

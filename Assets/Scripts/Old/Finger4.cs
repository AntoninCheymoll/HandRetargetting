using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Finger4 : MonoBehaviour
{
    public enum Finger { ring, middle, index };

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
            Debug.Log(finger);
             distance[finger] = Vector3.Distance(r_tip.transform.position, real_d[finger].transform.position);
        }

        float smallestDist = float.MaxValue;
        Finger smallestFinger = Finger.index;

        float smallestDist2 = float.MaxValue;
        Finger smallestFinger2 = Finger.index;


        foreach (KeyValuePair<Finger, float> dist in distance)
        {
            if(dist.Value < smallestDist)
            {
                smallestDist2 = smallestDist;
                smallestFinger2 = smallestFinger;

                smallestDist = dist.Value; 
                smallestFinger = dist.Key; 

            }else if (dist.Value < smallestDist2)
            {
                smallestDist2 = dist.Value;
                smallestFinger2 = dist.Key;
            }
        }

        float alpha = 1 - Mathf.Min(1, (smallestDist + smallestDist2)/2 / retargetingStart);
        float distDiff = smallestDist2 - smallestDist;
        float prop2 = Mathf.Max(0,(smallestDist - distDiff)/(smallestDist*2));
        float prop1 = 1 - prop2;

        Vector3 targetDistDiff = real_d[smallestFinger].transform.position - display_d[smallestFinger].transform.position;
        Vector3 targetDistDiff2 = real_d[smallestFinger2].transform.position - display_d[smallestFinger2].transform.position;

        r_displayed.transform.position = r_real.transform.position - (targetDistDiff * prop1 + targetDistDiff2 * prop2) * alpha;

    }

}

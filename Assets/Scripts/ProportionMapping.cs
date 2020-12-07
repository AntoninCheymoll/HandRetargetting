using OVRTouchSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using UnityEngine;

public class ProportionMapping : MonoBehaviour
{
    enum FingerName { index, middle, ring, pinky };

    public struct MapPoint
    {
        public GameObject realPoint;
        public GameObject displayPoint;

        public MapPoint(GameObject realPoint, GameObject displayPoint)
        {
            this.realPoint = realPoint;
            this.displayPoint = displayPoint;
        }

        public Vector3 getVectDiff()
        {
            return displayPoint.transform.position - realPoint.transform.position;
        }

        public float getDist(SphereCollider tip)
        {
            return Mathf.Max(0,Vector3.Distance(tip.transform.position, realPoint.transform.position) - tip.radius);
        }
    }

    struct Finger
    {
        public FingerName fingerName;
        public List<MapPoint> points;
        public List<Collider> collider;

        public Finger(FingerName f)
        {
            this.fingerName = f;
            this.points = new List<MapPoint>();
            this.collider = new List<Collider>();
        }
    }

    struct Hand
    {
        public List<Finger> fingers;
    }

    public SphereCollider tip;

    public GameObject r_displayed_hand;
    public GameObject r_real_hand;

    public GameObject l_displayed_hand;
    public GameObject l_real_hand;

    public float wrapOrigin = 0.07f;
    public float withinFingerPow;
    public float betweenFingerPow;

    Hand hand = new Hand();


    void Start()
    {

        hand.fingers = new List<Finger>();

        foreach (FingerName finger in (FingerName[])Enum.GetValues(typeof(FingerName)))
        {
            hand.fingers.Add(new Finger(finger));
            FindObjectsInFinger(finger.ToString());

            Finger fp = hand.fingers.Find(x => x.fingerName.Equals(finger));
            int fingerIndex = hand.fingers.IndexOf(fp);

        }
    }

    // Update is called once per frame
    void Update()
    {

        /*float totalDist = finger.points.Sum(p => p.getDist(tip));

        float invertPower(MapPoint x) { return Mathf.Pow(totalDist - x.getDist(tip), withinFingerPow); }

        float totalInvertedDist = finger.points.Sum(invertPower);

        float distToFinger = getFingerDist(finger);

        Vector3 offset = new Vector3(0, 0, 0);

        foreach (MapPoint mp in finger.points)
        {
            offset += mp.getVectDiff() * (invertPower(mp)) / totalInvertedDist1;
        }

        offset *= Mathf.Max(0, wrapOrigin - distToFinger) / wrapOrigin;
        */

        float totalDist = hand.fingers.Sum(f => getFingerDist(f));

        float invertPower(Finger f) { return Mathf.Pow(totalDist - getFingerDist(f), betweenFingerPow); }

        float totalInvertedDist = hand.fingers.Sum(invertPower);

        Vector3 offset = Vector3.zero;
        
        foreach (Finger finger in hand.fingers)
        {
            offset += getFingerOffset(finger) * (invertPower(finger)) / totalInvertedDist;
        }

        r_displayed_hand.transform.position = r_real_hand.transform.position + offset;
    }

    public int SortByDistance(MapPoint p1, MapPoint p2)
    {
        return (p1.getDist(tip) > p2.getDist(tip)) ?1:-1;
    }

    private void FindObjectsInFinger(string fingerName)
    {

        Finger fp = hand.fingers.Find(x => x.fingerName.ToString().Equals(fingerName));
        int fingerIndex = hand.fingers.IndexOf(fp);

        string[] fingerPartNames = new string[] { fingerName + 1, fingerName + 2, fingerName + 3, fingerName + "_finger_tip_marker", fingerName + "_palm_knuckle_marker" };

        foreach (string fingerPartName in fingerPartNames)
        {
            MapPoint mp = new MapPoint(FindDeepChild(l_real_hand.transform, fingerPartName).gameObject,
                                FindDeepChild(l_displayed_hand.transform, fingerPartName).gameObject);

            hand.fingers[fingerIndex].points.Add(mp);

            if(mp.realPoint.GetComponent<Collider>() != null)
                hand.fingers[fingerIndex].collider.Add(mp.realPoint.GetComponent<Collider>());

        }
    }


    public Transform FindDeepChild(Transform aParent, string aName)
    {

        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name.Contains(aName))
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }

        return null;
    }

     (Finger,Finger) getClosestFingers()
    {
        Finger closestFinger = new Finger();
        Finger secondClosestFinger = new Finger();

        float closestDist = float.MaxValue;
        float secondClosestDist = float.MaxValue;

        foreach (Finger finger in hand.fingers)
        {
            float dist = getFingerDist(finger);

            if(dist < closestDist)
            {
                secondClosestFinger = closestFinger;
                secondClosestDist = closestDist;

                closestFinger = finger;
                closestDist = dist;
            }
            else if (dist < secondClosestDist)
            {
                secondClosestDist = dist;
                secondClosestFinger = finger;
            }
        }

        return (closestFinger, secondClosestFinger);
    }

     Vector3 getFingerOffset(Finger finger)
    {

        float totalDist = finger.points.Sum(p => p.getDist(tip));

        float invertPower(MapPoint x) { return Mathf.Pow(totalDist - x.getDist(tip), withinFingerPow); }

        float totalInvertedDist = finger.points.Sum(invertPower);

        float distToFinger = getFingerDist(finger);

        Vector3 offset = new Vector3(0, 0, 0);

        foreach (MapPoint mp in finger.points)
        {
            offset += mp.getVectDiff() * (invertPower(mp)) / totalInvertedDist;
        }

        //offset *= Mathf.Max(0, wrapOrigin - distToFinger) / wrapOrigin;

        return offset;
    }

     float getFingerDist(Finger f)
     {
        float minDist = float.MaxValue;
        foreach(CapsuleCollider col in f.collider)
        {
            float dist = Vector3.Distance(tip.transform.position, col.ClosestPoint (tip.transform.position)) - col.radius;
            minDist = Mathf.Min(minDist, dist);
        }
        return Math.Max(0,minDist);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RematchFingers : MonoBehaviour
{

    public enum FingerName { index, middle, ring, pinky };

    public struct FingerCorrespondance
    {
        public FingerName originalFinger;
        public bool isBetween;

        public FingerName correspondingFinger1;
        public FingerName correspondingFinger2;

        public FingerCorrespondance(FingerName original, FingerName n1)
        {
            this.originalFinger = original;

            this.isBetween = false;

            this.correspondingFinger1 = n1;
            this.correspondingFinger2 = n1;
        }

        public FingerCorrespondance(FingerName original, FingerName n1, FingerName n2)
        {
            this.originalFinger = original;

            this.isBetween = true;

            this.correspondingFinger1 = n1;
            this.correspondingFinger2 = n2;
        }
    }

    public struct FingerPoints{

        public FingerName name;
        public List<GameObject> reals;
        public List<GameObject> displays;
        public List<Collider> collider;


        public FingerPoints(FingerName name, List<GameObject> real, List<GameObject> display, List<Collider> colliders)
        {
            this.name = name;

            this.reals = real;
            this.displays = display;

            this.collider = colliders;
        }

    }
    
    public List<FingerCorrespondance> correspondances = new List<FingerCorrespondance>();
    public List<FingerPoints> points = new List<FingerPoints>();

    public SphereCollider tip;

    public GameObject r_displayed_hand;
    public GameObject r_real_hand;

    public GameObject l_displayed_hand;
    public GameObject l_real_hand;

    public float wrapOrigin = 0.07f;
    public float withinFingerPow;
    public float betweenFingerPow;

    // Start is called before the first frame update
    void Start()
    {
        initiateCorrespondance();
        findPoints();

    }

    // Update is called once per frame
    void Update()
    {
        FingerPoints closest = getClosestFinger();
        FingerCorrespondance correspondance = getCorrespondance(closest.name);

        float totalDist = points.Sum(f => getFingerDist(f));

        float invertPower(FingerPoints f) { return Mathf.Pow(totalDist - getFingerDist(f), betweenFingerPow); }

        float totalInvertedDist = points.Sum(invertPower);

        Vector3 offset = Vector3.zero;

        foreach (FingerPoints finger in points)
        {
            offset += getFingerOffset(getCorrespondance(finger.name)) * (invertPower(finger)) / totalInvertedDist;
        }

        r_displayed_hand.transform.position = r_real_hand.transform.position + offset;


    }

    void initiateCorrespondance()
    {
        correspondances.Add(new FingerCorrespondance(FingerName.index, FingerName.index));
        correspondances.Add(new FingerCorrespondance(FingerName.middle, FingerName.middle));
        correspondances.Add(new FingerCorrespondance(FingerName.ring, FingerName.middle));
        correspondances.Add(new FingerCorrespondance(FingerName.pinky, FingerName.ring));
    }

    void findPoints()
    {
        foreach (FingerName finger in (FingerName[])Enum.GetValues(typeof(FingerName)))
        {
            string[] fingerPartNames = new string[] { finger.ToString() + 1, finger.ToString() + 2, finger.ToString() + 3, finger.ToString() + "_finger_tip_marker", finger.ToString() + "_palm_knuckle_marker" };

            List<GameObject> real = new List<GameObject>();
            List<GameObject> display = new List<GameObject>();

            List<Collider> colliders = new List<Collider>();

            for (int i = 0; i < fingerPartNames.Length; i++)
            {
                string name = fingerPartNames[i];

                real.Add(FindDeepChild(l_real_hand.transform, name).gameObject);
                display.Add(FindDeepChild(l_displayed_hand.transform, name).gameObject);

                if (real[i].GetComponent<Collider>() != null) colliders.Add(real[i].GetComponent<Collider>());
            }

            points.Add(new FingerPoints(finger, real, display, colliders));
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

    FingerPoints getClosestFinger()
    {
        FingerPoints closestFinger = new FingerPoints();

        float closestDist = float.MaxValue;

        foreach (FingerPoints finger in points)
        {
            float dist = getFingerDist(finger);
            if (dist < closestDist)
            {

                closestFinger = finger;
                closestDist = dist;
            }
        }

        return (closestFinger);
    }

    float getFingerDist(FingerPoints f)
    {
        float minDist = float.MaxValue;
        foreach (CapsuleCollider col in f.collider)
        {
            float dist = Vector3.Distance(tip.transform.position, col.ClosestPoint(tip.transform.position)) - col.radius;
            minDist = Mathf.Min(minDist, dist);
        }
        return Math.Max(0, minDist);
    }

    public FingerCorrespondance getCorrespondance(FingerName name)
    {
        foreach(FingerCorrespondance c in correspondances)
        {
            if (c.originalFinger == name) return c;
        }
        return new FingerCorrespondance();
    }

    public FingerPoints GetFingerPoints(FingerName name)
    {
        foreach (FingerPoints p in points)
        {
            if (p.name == name) return p;
        }
        return new FingerPoints();
    }

    public float getPointDist(GameObject point)
    {
        return Mathf.Max(0, Vector3.Distance(tip.transform.position, point.transform.position) - tip.radius);
    }

    public float getTotalDist( FingerPoints finger)
    {
        float total = 0;
        foreach (GameObject p in finger.reals) total += getPointDist(p);
        return total;
    }


    Vector3 getFingerOffset(FingerCorrespondance correspondance)
    {

        FingerPoints original = GetFingerPoints(correspondance.originalFinger);

        FingerPoints display1 = GetFingerPoints(correspondance.correspondingFinger1);
        FingerPoints display2 = GetFingerPoints(correspondance.correspondingFinger2);

        List<GameObject> realPoints = original.reals;
        List<Vector3> displayedPoints = new List<Vector3>();

        for (int i = 0; i < realPoints.Count; i++)
        {

            if (!correspondance.isBetween)
            {
                displayedPoints.Add(display1.displays[i].transform.position);
            }
            else
            {
                displayedPoints.Add(display1.displays[i].transform.position / 2 + display2.displays[i].transform.position / 2);
            }
        }

        float totalDist = getTotalDist(original);
        float invertPower(GameObject x) { return Mathf.Pow(totalDist - getPointDist(x), withinFingerPow); }

        float totalInvertedDist = 0;
        foreach (GameObject p in realPoints) totalInvertedDist += invertPower(p);

        Vector3 offset = new Vector3(0, 0, 0);

        for (int i = 0; i < realPoints.Count; i++)
        {
            Vector3 vectDiff = displayedPoints[i] - realPoints[i].transform.position;
            offset += vectDiff * (invertPower(realPoints[i])) / totalInvertedDist;

        }

        return offset;
    }
}

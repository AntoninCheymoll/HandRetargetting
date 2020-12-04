using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour
{
    private OVRHand rightHand;
    private OVRHand leftHand;

    public GameObject pot;
    public GameObject potCenter;

    public GameObject leftRealHand;
    public GameObject rightRealHand;

    public GameObject leftDisplayHand;
    public GameObject rightDisplayHand;

    public GameObject leftHandle;
    public GameObject rightHandle;

    public GameObject leftMesh;
    public GameObject rightMesh;

    public Text debugtext;

    private Vector3 differenceToCenter;
    private float maxDistance = 0.10f;
    private float minDistance = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = rightRealHand.GetComponent<OVRHand>();
        leftHand = leftRealHand.GetComponent<OVRHand>();

        differenceToCenter = pot.transform.position - potCenter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        placeBetweenBothHand();
    }

    void placeBetweenBothHand()
    {
        Vector3 handMiddle = (leftHand.transform.position + rightHand.transform.position)/2;
        pot.transform.position = handMiddle + differenceToCenter;
        
        Vector3 orientation = rightRealHand.transform.position - leftRealHand.transform.position;
        pot.transform.rotation = Quaternion.FromToRotation(Vector3.up, orientation);
        pot.transform.Rotate(new Vector3(90,0,90));

        rightDisplayHand.transform.position = rightHandle.transform.position;
        leftDisplayHand.transform.position = leftHandle.transform.position;

        float dist = Vector3.Distance(leftRealHand.transform.position, leftHandle.transform.position);
        dist -= minDistance; 
        //updateHandColor(new Color(dist/(maxDistance-minDistance),0,0));
        debugtext.text = dist + "";
    }

    void updateHandColor(Color c)
    {
        rightMesh.GetComponent<Renderer>().material.color = c;
        leftMesh.GetComponent<Renderer>().material.color = c;
    }
}

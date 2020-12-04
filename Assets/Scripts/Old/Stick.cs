using OVRTouchSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Stick : MonoBehaviour
{
    private OVRHand rightHand;
    private OVRHand leftHand;
    
    private OVRSkeleton rightSkeleton;
    private OVRSkeleton leftSkeleton;


    private bool rightHandGraspClosed = false;
    private bool leftHandGraspClosed = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public GameObject stick;
    public GameObject leftHandObject;
    public GameObject rightHandObject;

    public GameObject leftHandCenter;
    public GameObject rightHandCenter;

    public GameObject leftHandPart1;
    public GameObject leftHandPart2;

    // Start is called before the first frame update
    void Start()
    {

        rightHand = rightHandObject.GetComponent<OVRHand>();
        leftHand = leftHandObject.GetComponent<OVRHand>();

        rightSkeleton = rightHandObject.GetComponent<OVRSkeleton>();
        leftSkeleton = leftHandObject.GetComponent<OVRSkeleton>();

        initialPosition = stick.transform.position;
        initialRotation = stick.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        rightHandGraspClosed = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        leftHandGraspClosed = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        //if (rightHandGraspClosed && leftHandGraspClosed)
        //{
            Vector3 rightHandPos = rightHandCenter.transform.position;
            Vector3 leftHandPos = leftHandCenter.transform.position;

            Vector3 leftHandPart1Pos = leftHandPart1.transform.position;
            Vector3 leftHandPart2Pos = leftHandPart2.transform.position;

            placeBetweenBothHand(rightHandPos, leftHandPos);
            //placeUsingOneHand(rightHandPos, leftHandPos, leftHandPart1Pos, leftHandPart2Pos);
        //}
        //else
        //{
            //rightHand.transform.position = initialPosition;
            //rightHand.transform.rotation = initialRotation;
        //}
    }

    void placeBetweenBothHand(Vector3 hand1, Vector3 hand2)
    {
        Vector3 dir = hand1 - hand2;

        stick.transform.position = (hand1 + hand2) / 2;
        stick.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //leftHandObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }

    void placeUsingOneHand(Vector3 hand1, Vector3 hand2, Vector3 handPart1, Vector3 handPart2)
    {
        Vector3 dir = handPart1 - handPart2;

        stick.transform.position = (hand2);
        stick.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
}

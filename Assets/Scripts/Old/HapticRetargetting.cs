using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticRetargetting : MonoBehaviour
{ 
    public GameObject l_displayed;
    public GameObject l_real;

    public GameObject r_displayed;
    public GameObject r_real;

    public GameObject r_tip;
    public GameObject l_tip;

    public GameObject displayedTarget;
    public GameObject realTarget;

    public Text txt;

    private float retargetingStart = 0.2f;
    private void Start()
    {
        displayedTarget.transform.parent = null;
        realTarget.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        float distToTarget = Vector3.Distance(r_tip.transform.position, realTarget.transform.position);
        float alpha = 1-Mathf.Min(1,distToTarget/retargetingStart);
        txt.text = alpha.ToString() + "";

        Vector3 targetDistDiff = realTarget.transform.position - displayedTarget.transform.position;
        r_displayed.transform.position = r_real.transform.position - targetDistDiff * alpha;
    }
}

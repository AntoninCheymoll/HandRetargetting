using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Offset : MonoBehaviour
{
    public GameObject l_displayed;
    public GameObject l_real;

    public GameObject r_displayed;
    public GameObject r_real;

    public float l_translateX;
    public float l_translateY;
    public float l_translateZ;

    public float l_rotateX;
    public float l_rotateY;
    public float l_rotateZ;

    public float r_translateX;
    public float r_translateY;
    public float r_translateZ;

    public float r_rotateX;
    public float r_rotateY;
    public float r_rotateZ;

    public Text t;
    private bool notUpdated;

    private float countDown = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countDown += Time.deltaTime;
        t.text = Mathf.Max(0, Mathf.Floor(countDown))+ "";
        if (countDown > 6) t.text = "";

        stretchUpdate();

        //l_displayed.transform.position = l_real.transform.position + new Vector3(translateX, translateY, translateZ);
        //l_displayed.transform.localRotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
    }

    void stretchUpdate()
    {
        if (countDown > 6 && countDown < 8)
        {
            float time = countDown - 6;
            float prop = time / 30;

            t.text = time + "";

            l_displayed.transform.position = l_real.transform.position + new Vector3(-prop, 0, 0);
            r_displayed.transform.position = r_real.transform.position + new Vector3(prop, 0, 0);

        }
    }
}

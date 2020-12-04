using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    List<int> list = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        list.Add(1);
        list.Add(3);
        list.Add(2);
        list.Add(5);
        list.Add(6);
        list.Add(2);
        list.Add(5);

        list.Sort(SortByDistance);

        foreach(int i in list)
        {
            Debug.Log(i);
        }
        
    }

    static int SortByDistance(int p1, int p2)
    {
        return (p1 > p2) ? 1 : -1;
    }
}

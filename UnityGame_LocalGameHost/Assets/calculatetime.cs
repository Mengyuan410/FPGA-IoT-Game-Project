using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calculatetime : MonoBehaviour
{
    int index = 0;
    public GameObject UDPNetwork;
    public UDPconnection UDPscript;
    float tot_time;
    float avg_time;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        tot_time = 0;
        avg_time = 0;
        //UDPNetwork = GameObject.Find("UDPconnection");
        //UDPscript = UDPNetwork.GetComponent<UDPconnection>();
}

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("The time difference is " + result.ToString());
        float result = Time.time - UDPscript.result_time - Time.deltaTime ;

        if(index < 100)
        {
            index += 1;
            tot_time += result;
            
        }
        else if (index == 100)
        {
            avg_time = tot_time / 100;
            Debug.Log("The avg time difference is " + avg_time.ToString());
            index += 1;
        }
        else
        {
            index = 0;
            tot_time = 0;
            avg_time = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform car;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(car.position.x, car.position.y, -10f);
    }
    
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class changeposition : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public GameObject car2;
//    GameObject opponent_car;
//    MyNetworkManger opponent_carscript;

//    void Start()
//    {
//        opponent_car = GameObject.Find("ServerConnection");
//        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        car2.transform.position = new Vector3(opponent_carscript.x_axis, opponent_carscript.y_axis, 0);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initiate : MonoBehaviour
{

    GameObject ClientPrefabs;
    public GameObject bombAPrefab, bombBPrefab;
    GameObject ServerMessaageObject;
    MyNetworkManger NetworkMessage;


    void Start()
    {
        //Src
        ServerMessaageObject = GameObject.Find("MyNetworkManger");
        NetworkMessage = ServerMessaageObject.GetComponent<MyNetworkManger>();


    }


    //Start is called before the first frame update

    //initiate player
    //public void ini_player()
    //{

    //    if (NetworkMessage.ini_player == true)
    //    {
    //        GameObject ClientPlayer = Instantiate(ClientPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
    //        ClientPlayer.name = "ClientPlayer";
    //        NetworkMessage.ini_player = false;
    //    }

    public void Destroy_Diamond()
    {
        if (NetworkMessage.Destroy_Diamond)
        {
            Destroy(GameObject.Find(NetworkMessage.Destroy_Diamond_Name));
            
        }
    }

    public void ini_bomb(string Team, ref bool renew_bomb_, float bomb_x_axis_, float bomb_y_axis_)
    {
        //initiate bomb
        Debug.Log("tried relesing bomb");
        if (Team != null)
        {
            if (Team == "A")
            {
                Debug.Log("A bomb");
                GameObject bomb = Instantiate(bombAPrefab, new Vector3(bomb_x_axis_, bomb_y_axis_), Quaternion.identity);
                bomb.name = "bombA";
                Debug.Log("A bomb done");
                renew_bomb_ = false;
            }
            else
            {
                Debug.Log("B bomb");
                GameObject bomb = Instantiate(bombBPrefab, new Vector3(bomb_x_axis_, bomb_y_axis_), Quaternion.identity);
                bomb.name = "bombB";
                Debug.Log("B bomb done");
                renew_bomb_ = false;
            }


        }
        else
        {
            Debug.Log("No team");
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;

public class WaitingMenu : MonoBehaviour
{

    //assuming input A 
    EventSystem system;
    public GameObject A3, A1, A2, B1, B2, B3;
    GameObject opponent_car;
    MyNetworkManger opponent_carscript;
    //public Selectable firstInput;
    string localPlayer_team, localPlayerID;
    string LocalID, userA1, userA2, userA3, userB1, userB2, userB3;
    public LoginMenu Login;
    // Start is called before the first frame update

    void Start()
    {
        opponent_car = GameObject.Find("MyNetworkManger");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        Login.system = EventSystem.current;

    }

    // Update is called once per frame
    void Update()
    {
        
       
        if ( A1.activeSelf == false && opponent_carscript.CarTeam_a1 != "")
        {

            Debug.Log("A1 userID is null? " + (A1.transform.Find("userID") == null), gameObject);
            Debug.Log("A1 Text is null? " + (A1.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text == null), gameObject);
            A1.SetActive(true);
            A1.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text = opponent_carscript.CarTeam_a1;
            Debug.Log("A1 name assigned");
        }
        if (A2.activeSelf == false && opponent_carscript.CarTeam_a2 != "")
        {
            Debug.Log("A2 userID is null? " + (A2.transform.Find("userID") == null), gameObject);
            Debug.Log("A2 Text is null? " + (A2.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text == null), gameObject);
            A2.SetActive(true);
            A2.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text = opponent_carscript.CarTeam_a2;
        }
        if (A3.activeSelf == false && opponent_carscript.CarTeam_a3 != "")
        {
            Debug.Log("A3 userID is null? " + (A3.transform.Find("userID") == null), gameObject);
            Debug.Log("A3 Text is null? " + (A3.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text == null), gameObject);
            A3.SetActive(true);
            A3.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text = opponent_carscript.CarTeam_a3;
        }
            
        if (B1.activeSelf == false && opponent_carscript.CarTeam_b1 != "")
        {
            Debug.Log("B1 userID is null? " + (B1.transform.Find("userID") == null), gameObject);
            Debug.Log("B1 Text is null? " + (B1.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text == null), gameObject);
            B1.SetActive(true);
            B1.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text = opponent_carscript.CarTeam_b1;
        }
        if (B2.activeSelf == false && opponent_carscript.CarTeam_b2 != "")
        {
            Debug.Log("B2 userID is null? " + (B2.transform.Find("userID") == null), gameObject);
            Debug.Log("B2 Text is null? " + (B2.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text == null), gameObject);
            B2.SetActive(true);
            B2.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text = opponent_carscript.CarTeam_b2;
        }
        if (B3.activeSelf == false && opponent_carscript.CarTeam_b3 != "")
        {
            Debug.Log("B3 userID is null? " + (B3.transform.Find("userID") == null), gameObject);
            Debug.Log("B3 Text is null? " + (B3.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text == null), gameObject);
            B3.SetActive(true);
            B3.transform.Find("userID").GetComponent<TMPro.TextMeshProUGUI>().text = opponent_carscript.CarTeam_b3;
        }
        
        if(A1.activeSelf && A2.activeSelf && A3.activeSelf && B1.activeSelf && B2.activeSelf && B3.activeSelf)
        {
            Debug.Log("Starting");
            Invoke("LaunchGame", 3.0f);
        }

    }
    public void LaunchGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

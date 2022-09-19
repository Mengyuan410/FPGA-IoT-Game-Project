using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TeamSelectionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    int countA = 0, countB = 0;
    public GameObject numberA, numberB;
    public MyNetworkManger server_input;
    public LoginMenu Login;
    public Selectable firstInput;
    GameObject opponent_car;
    MyNetworkManger opponent_carscript;
    void Start()
    {
        opponent_car = GameObject.Find("MyNetworkManger");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        Login.system = EventSystem.current;
        firstInput.Select();
    }

    void Update()
    {
        numberA.GetComponent<TMPro.TextMeshProUGUI>().text = countTeam(opponent_carscript.CarTeam_a1, opponent_carscript.CarTeam_a2, opponent_carscript.CarTeam_a3).ToString();
        numberB.GetComponent<TMPro.TextMeshProUGUI>().text = countTeam(opponent_carscript.CarTeam_b1, opponent_carscript.CarTeam_b2, opponent_carscript.CarTeam_b3).ToString();
    }
    int countTeam(string input_1, string input_2, string input_3){
        int count = 0;
        if(input_1 !=  ""){
            count += 1;
        }
        if(input_2 !=  ""){
            count += 1;
        }
        if(input_3 !=  ""){
            count += 1;
        }
        return count;

    }
}

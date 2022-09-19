using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class Carsmotion3 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject car3;

    public float prev_x_axis;
    public float prev_y_axis;
    public float prev_z_rotation;

    GameObject opponent_car;
    public float rotationSpeed;
    MyNetworkManger opponent_carscript;
    public float diff_x;
    public float diff_y;
    public string UserID;
    public string Team;
    HealthBar clientHealth_Script;
    GameObject clientCDBar, clientHealthBar;
    public Initiate ini;
    void Start()
    {
        opponent_car = GameObject.Find("ServerConnection");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        UserID = opponent_carscript.car_username[2];
        clientHealthBar = car3.transform.Find("ClientCar/BarCanvas/clientHealthBar").gameObject;
        clientHealth_Script = clientHealthBar.GetComponent<HealthBar>();
        clientCDBar = car3.transform.Find("ClientCar/BarCanvas/clientCDBar").gameObject;
        string [] teamA = {opponent_carscript.CarTeam_a1,opponent_carscript.CarTeam_a2,opponent_carscript.CarTeam_a3};
        string [] teamB = {opponent_carscript.CarTeam_b1,opponent_carscript.CarTeam_b2,opponent_carscript.CarTeam_b3};
        Find_team(teamA, teamB);
    }



    // Update is called once per frame

    void Update()
    {



        //movement
        car3.transform.position = new Vector3(opponent_carscript.x_axis_car3, opponent_carscript.y_axis_car3, 0);
        car3.transform.eulerAngles = new Vector3(
            car3.transform.eulerAngles.x,
            car3.transform.eulerAngles.y,
            opponent_carscript.z_rotation_car3);
        SynHealth_CD();
        if (opponent_carscript.renew_bomb_car3)
        {
            ini.ini_bomb(Team, ref opponent_carscript.renew_bomb_car3, opponent_carscript.bomb_x_axis_car3, opponent_carscript.bomb_x_axis_car3);
        }

    }
    public void SynHealth_CD()
    {
        clientHealth_Script.SetMaxHealth(opponent_carscript.maxHealth_car3);
        clientHealth_Script.SetHealth(opponent_carscript.health_car3);
        clientCDBar.GetComponent<Slider>().value = opponent_carscript.cd_car3;
    }
    void Find_team(string[] teamA, string[] teamB)
    {
        
        for(int i = 0; i < teamA.Length; i++)
        {
            if(UserID == teamA[i])
            {
                Team = "A";
            }
        }
        for (int g = 0; g < teamB.Length; g++)
        {
            if (UserID == teamB[g])
            {
                Team = "B";
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class Carsmotion4 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject car4;

    public float prev_x_axis;
    public float prev_y_axis;
    public float prev_z_rotation;
    public string UserID;
    GameObject opponent_car;
    public float rotationSpeed;
    MyNetworkManger opponent_carscript;
    public float diff_x;
    public float diff_y;
    public string Team;
    HealthBar clientHealth_Script;
    GameObject clientCDBar, clientHealthBar;
    public Initiate ini;
    void Start()
    {
        opponent_car = GameObject.Find("ServerConnection");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        UserID = opponent_carscript.car_username[3];
        clientHealthBar = car4.transform.Find("ClientCar/BarCanvas/clientHealthBar").gameObject;
        clientHealth_Script = clientHealthBar.GetComponent<HealthBar>();
        clientCDBar = car4.transform.Find("ClientCar/BarCanvas/clientCDBar").gameObject;
        string [] teamA = {opponent_carscript.CarTeam_a1,opponent_carscript.CarTeam_a2,opponent_carscript.CarTeam_a3};
        string [] teamB = {opponent_carscript.CarTeam_b1,opponent_carscript.CarTeam_b2,opponent_carscript.CarTeam_b3};
        Find_team(teamA, teamB);
    }



    // Update is called once per frame

    void Update()
    {



        //movement
        car4.transform.position = new Vector3(opponent_carscript.x_axis_car4, opponent_carscript.y_axis_car4, 0);
        car4.transform.eulerAngles = new Vector3(
            car4.transform.eulerAngles.x,
            car4.transform.eulerAngles.y,
            opponent_carscript.z_rotation_car4);
        SynHealth_CD();
        if (opponent_carscript.renew_bomb_car4)
        {
            ini.ini_bomb(Team, ref opponent_carscript.renew_bomb_car4, opponent_carscript.bomb_x_axis_car4, opponent_carscript.bomb_x_axis_car4);
        }

    }
    public void SynHealth_CD()
    {
        clientHealth_Script.SetMaxHealth(opponent_carscript.maxHealth_car4);
        clientHealth_Script.SetHealth(opponent_carscript.health_car4);
        clientCDBar.GetComponent<Slider>().value = opponent_carscript.cd_car4;
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
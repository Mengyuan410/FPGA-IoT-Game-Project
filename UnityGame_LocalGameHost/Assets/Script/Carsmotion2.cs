using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class Carsmotion2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject car2;

    public float prev_x_axis;
    public float prev_y_axis;
    public float prev_z_rotation;
    public string UserID;
    public string Team;
    GameObject opponent_car;
    public float rotationSpeed;
    MyNetworkManger opponent_carscript;
    public float diff_x;
    public float diff_y;
    HealthBar clientHealth_Script;
    GameObject clientCDBar, clientHealthBar;
    public Initiate ini;
    void Start()
    {
        opponent_car = GameObject.Find("MyNetworkManger");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        UserID = opponent_carscript.car_username[4];
        clientHealthBar = car2.transform.Find("ClientCar/BarCanvas/clientHealthBar").gameObject;
        clientHealth_Script = clientHealthBar.GetComponent<HealthBar>();
        clientCDBar = car2.transform.Find("ClientCar/BarCanvas/clientCDBar").gameObject;
        string [] teamA = {opponent_carscript.CarTeam_a1,opponent_carscript.CarTeam_a2,opponent_carscript.CarTeam_a3};
        string [] teamB = {opponent_carscript.CarTeam_b1,opponent_carscript.CarTeam_b2,opponent_carscript.CarTeam_b3};
        Find_team(teamA, teamB);
    }



    // Update is called once per frame

    void Update()
    {



        //movement
        car2.transform.position = new Vector3(opponent_carscript.x_axis_car2, opponent_carscript.y_axis_car2, 0);
        car2.transform.eulerAngles = new Vector3(
            car2.transform.eulerAngles.x,
            car2.transform.eulerAngles.y,
            opponent_carscript.z_rotation_car2);
        SynHealth_CD();
        if (opponent_carscript.renew_bomb_car2)
        {
            ini.ini_bomb(Team, ref opponent_carscript.renew_bomb_car2, opponent_carscript.bomb_x_axis_car2, opponent_carscript.bomb_x_axis_car2);
        }


    }
    public void SynHealth_CD()
    {
        clientHealth_Script.SetMaxHealth(opponent_carscript.maxHealth_car2);
        clientHealth_Script.SetHealth(opponent_carscript.health_car2);
        clientCDBar.GetComponent<Slider>().value = opponent_carscript.cd_car2;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class Carsmotion5 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject car5;

    public float prev_x_axis;
    public float prev_y_axis;
    public float prev_z_rotation;
    public string Team;
    GameObject opponent_car;
    public float rotationSpeed;
    MyNetworkManger opponent_carscript;
    public float diff_x;
    public float diff_y;
    public string UserID;
    HealthBar clientHealth_Script;
    GameObject clientCDBar, clientHealthBar;
    //from networkManger
    public bool placed_bomb_flag;
    public float _bomb_x_axis, _bomb_y_axis;
    //use ini_bomb to ini
    public Initiate ini;
    
    
    void Start()
    {
        opponent_car = GameObject.Find("ServerConnection");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        clientHealthBar = car5.transform.Find("ClientCar/BarCanvas/clientHealthBar").gameObject;
        clientHealth_Script = clientHealthBar.GetComponent<HealthBar>();
        clientCDBar = car5.transform.Find("ClientCar/BarCanvas/clientCDBar").gameObject;
        UserID = opponent_carscript.car_username[4];
        string [] teamA = {opponent_carscript.CarTeam_a1,opponent_carscript.CarTeam_a2,opponent_carscript.CarTeam_a3};
        string [] teamB = {opponent_carscript.CarTeam_b1,opponent_carscript.CarTeam_b2,opponent_carscript.CarTeam_b3};

        Find_team(teamA,teamB);
    }



    // Update is called once per frame

    void Update()
    {



        //movement
        car5.transform.position = new Vector3(opponent_carscript.bomb_x_axis_car5, opponent_carscript.bomb_x_axis_car5, 0);
        car5.transform.eulerAngles = new Vector3(
            car5.transform.eulerAngles.x,
            car5.transform.eulerAngles.y,
            opponent_carscript.z_rotation_car5);
        SynHealth_CD();
        if (opponent_carscript.renew_bomb_car1)
        {
            ini.ini_bomb(Team, ref opponent_carscript.renew_bomb_car5, opponent_carscript.bomb_x_axis_car5, opponent_carscript.bomb_x_axis_car5);
        }


    }
    public void SynHealth_CD()
    {
        clientHealth_Script.SetMaxHealth(opponent_carscript.maxHealth_car5);
        clientHealth_Script.SetHealth(opponent_carscript.health_car5);
        clientCDBar.GetComponent<Slider>().value = opponent_carscript.cd_car5;
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
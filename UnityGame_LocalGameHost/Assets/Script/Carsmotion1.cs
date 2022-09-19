using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class Carsmotion1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject car1;
    public float prev_x_axis;
    public float prev_y_axis;
    public float prev_z_rotation;

    GameObject opponent_car;
    public float rotationSpeed;
    MyNetworkManger opponent_carscript;
    //public string CarTeam_a1_, CarTeam_a2_, CarTeam_a3_;
   //public string CarTeam_b1_, CarTeam_b2_, CarTeam_b3_;

    public float diff_x;
    public float diff_y;
    public string UserID;
    public string Team;
    
    public Initiate ini;
    public int count;
    public int pre_health;

    HealthBar clientHealth_Script;
    GameObject clientCDBar, clientHealthBar;
    void Start()
    {


        opponent_car = GameObject.Find("MyNetworkManger");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();
        // CarTeam_a1_ = opponent_carscript.CarTeam_a1;
        // CarTeam_a2_ = opponent_carscript.CarTeam_a2;
        // CarTeam_a3_ = opponent_carscript.CarTeam_a3;
        // CarTeam_b1_ = opponent_carscript.CarTeam_a1;
        // CarTeam_b2_ = opponent_carscript.CarTeam_a2;
        // CarTeam_b3_ = opponent_carscript.CarTeam_a3;

        string [] teamA = {opponent_carscript.CarTeam_a1,opponent_carscript.CarTeam_a2,opponent_carscript.CarTeam_a3};
        string [] teamB = {opponent_carscript.CarTeam_b1,opponent_carscript.CarTeam_b2,opponent_carscript.CarTeam_b3};



        clientHealthBar = car1.transform.Find("ClientCar/BarCanvas/clientHealthBar").gameObject;
        clientHealth_Script = clientHealthBar.GetComponent<HealthBar>();
        clientCDBar = car1.transform.Find("ClientCar/BarCanvas/clientCDBar").gameObject;
        UserID = opponent_carscript.car_username[0];
        Debug.Log("teamA size " + teamA.Length);
        Debug.Log("teamB size " + teamB.Length);
        Find_team(teamA, teamB);
    }





    // Update is called once per frame

    void Update()
    {

        //movement
        car1.transform.position = new Vector3(opponent_carscript.x_axis_car1, opponent_carscript.y_axis_car1, 0);
        car1.transform.eulerAngles = new Vector3(
            car1.transform.eulerAngles.x,
            car1.transform.eulerAngles.y,
            opponent_carscript.z_rotation_car1);
        
        SynHealth_CD();
        if (opponent_carscript.renew_bomb_car1)
        {
            Debug.Log("Releaasing Bomb");
            ini.ini_bomb(Team, ref opponent_carscript.renew_bomb_car1, opponent_carscript.bomb_x_axis_car1, opponent_carscript.bomb_y_axis_car1);
        }
    }

    public void SynHealth_CD()
    { 

        if(opponent_carscript.health_car1 != pre_health){
            count = 0;   
        }

        if (opponent_carscript.health_car1  == 0)
        {
            count += 1;
            if (count >= 50)
            {
                gameObject.SetActive(false);
                
            }
        }
        else{
        clientHealth_Script.SetMaxHealth(opponent_carscript.maxHealth_car1);
        clientHealth_Script.SetHealth(opponent_carscript.health_car1);

        clientCDBar.GetComponent<Slider>().value = opponent_carscript.cd_car1;
        }
        pre_health = opponent_carscript.health_car1;
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

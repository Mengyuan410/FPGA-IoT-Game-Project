using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Bomb : MonoBehaviour
{
    public Slider timerSlider;
    public float CD_time;
    public Text bomb_cd;
    public float bomb_x_axis;
    public float bomb_y_axis;
    public bool bomb_placed = false;
    private float time_gap, t1, t2;
    public Transform firePoint;
    public GameObject bombAPrefab, bombBPrefab;
    GameObject FPGAinput;
    inputfpga FPGAinputscript;

    public cars localCar;
    public string Team;

    void Start()
    {
        timerSlider.maxValue = CD_time;
        timerSlider.value = CD_time;
        FPGAinput = GameObject.Find("InputFPGA");
        FPGAinputscript = FPGAinput.GetComponent<inputfpga>();
       
    }

    void Update()

    {
        if (Team != "A" && Team != "B")
        {
            Debug.Log("assign");
            Team = localCar.Team;

        }
        float cd_bar;
        t2 = Time.time;
        time_gap = t2 - t1;
        int display_num;
        if (FPGAinputscript.bomb == 1 && time_gap >= CD_time)
        //if (Input.GetKeyDown("space") && time_gap >= CD_time)
        {
            Bombing();
            t1 = Time.time;
            bomb_placed = true;
            bomb_x_axis = firePoint.position.x;
            bomb_y_axis = firePoint.position.y;
        }

        cd_bar = t1 + CD_time - t2;
        if (cd_bar >= 0)
        {
            timerSlider.value = cd_bar;
            display_num = ((int)cd_bar);
            bomb_cd.text = display_num.ToString();

        }


    }

    void Bombing()
    {
        if (Team != null)
        {
            Debug.Log("my_team "+Team);
            if (Team == "A")
            {
                
                GameObject bomb = Instantiate(bombAPrefab, firePoint.position, firePoint.rotation);
                bomb.name = "bombA";
                Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
                bomb.tag = "My_Bomb";
            }
            else
            {
                GameObject bomb = Instantiate(bombBPrefab, firePoint.position, firePoint.rotation);
                bomb.name = "bombB";
                Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
                bomb.tag = "My_Bomb";
            }
        }
        else
        {
            Debug.Log("Team NULL");
        }
            
    }
}

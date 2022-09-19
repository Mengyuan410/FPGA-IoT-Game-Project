using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BombAction : MonoBehaviour
{

    public GameObject hitEffect;
    public HealthBar healthBar;
    //public int health;

    public int count = 0;

    GameObject my_car, my_health;
    GameObject kill_stored;
    inputfpga kill_storedscript;
    cars mycarscript;
    HealthBar my_health_script;
    //private static int count_dia = cars.count_dia;
    void Start()
    {
        my_car = GameObject.Find("Cars");
        my_health = GameObject.Find("Health bar");
        mycarscript = my_car.GetComponent<cars>();
        my_health_script = my_health.GetComponent<HealthBar>();
        kill_stored = GameObject.Find("InputFPGA");
        kill_storedscript = kill_stored.GetComponent<inputfpga>();

    }
    void OnCollisionEnter2D(Collision2D collision)
    { 
        Debug.Log("gameObject.tag is null? " + (gameObject.tag == null), gameObject);
        Debug.Log(gameObject.tag);
        if (gameObject.tag == "My_Bomb" && collision.gameObject.tag != "LocalPlayer")
        {

            Debug.Log("name is null? " + (gameObject.name == null), gameObject);    
            Debug.Log("gameObject is null? " + (collision.gameObject == null), gameObject);
            Debug.Log("bombname " + gameObject.name);
     

            if (gameObject.name == "bombA" && collision.gameObject.GetComponent<Carsmotion1>().Team != "A")
            {               
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 5f);
                Destroy(gameObject);
                kill_storedscript.cur_kill_num += 1;                             
            }else 
            if(gameObject.name == "bombB" && collision.gameObject.GetComponent<Carsmotion1>().Team != "B")
            {                               
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 5f);
                Destroy(gameObject);
                kill_storedscript.cur_kill_num += 1;                              
            }          
        }


        else if(gameObject.tag != "My_Bomb" && collision.gameObject.tag == "LocalPlayer")
        {
            // other bomb
            

            if(gameObject.name == "bombA" && collision.gameObject.GetComponent<cars>().Team != "A" ){
                Debug.Log( collision.gameObject.GetComponent<cars>().Team);
                //this bomb is from bombA
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 5f);
                Destroy(gameObject);
            }else 
            if(gameObject.name == "bombB" && collision.gameObject.GetComponent<cars>().Team != "B"){
                Debug.Log( collision.gameObject.GetComponent<cars>().Team);
                //this bomb is from bombB
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 5f);
                Destroy(gameObject);   
            }

            mycarscript.count_dia -= 1;
            mycarscript.DiamondNum.text = mycarscript.count_dia.ToString();
            my_health_script.SetHealth(mycarscript.count_dia);
            Debug.Log("Bomb-1");
            if (mycarscript.count_dia < 0)
            {
            
                Application.Quit();
            }
            

            // if (collision.gameObject.tag == "LocalPlayer")
            // {
            //     mycarscript.count_dia -= 1;
            //     mycarscript.DiamondNum.text = mycarscript.count_dia.ToString();

            //     my_health_script.SetHealth(mycarscript.count_dia);
            //     Debug.Log("Bomb-1");

            //     //count_dia -= 1;   
            //     //i hit others bomb
            //     GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //     Destroy(effect, 5f);
            //     Destroy(gameObject);

            //     kill_storedscript.cur_kill_num += 1;

                
            // }
        }


    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MockCarControl : MonoBehaviour
{

    public Rigidbody2D rb;
    public float speed;
    public float rotationSpeed;
    public int count_dia = 3;
    private int maxHealth;
    //public Text DiamondNum;
    public GameObject bar, my_health;
    public int time;
    public float horizontalmove;
    public float verticalmove;
    //float scaledspeed;
    public float scaledspeed;
    int pre_mov;
    int pre_index;
    float increase_val;
    //GameObject FPGAinput;
    //inputfpga FPGAinputscript;
    HealthBar healthBar;
    // Better :// now extra my team information from server
    //public GameObject server;
    //public MyNetworkManger server_information;

    //public GameObject ClientCar1;
    //public string UserID;
    // SOS default teamB
    //public string Team;
    //public bool Diamond_Distory;
    //public string Diamond_ID;


    //health Bar



    // Start is called before the first frame update
    void Start()
    {
        scaledspeed = 2;


        //count_dia = int.Parse(DiamondNum.text);
        maxHealth = count_dia;
        my_health = GameObject.Find("Health bar");
        healthBar = my_health.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(3);


        // Better :// now extra my team information from server

        // server = GameObject.Find("MyNetworkManger");
        // server_information = server.GetComponent<MyNetworkManger>();
        //UserID = FPGAinputscript.username;


        //string[] teamA = { server_information.CarTeam_a1, server_information.CarTeam_a2, server_information.CarTeam_a3 };
      


        //string[] teamB = { server_information.CarTeam_b1, server_information.CarTeam_b2, server_information.CarTeam_b3 };

        //SOS
        //Find_team(teamA, teamB);
        //ClientCar1 = GameObject.Find("ClientCar1");
    }

    // k = 0.8, m = 0.4611 0.2 to 0.98 model 1-kexp(mt)
    //float[] coefficient = { 0.2f, 0.49553f, 0.68189f, 0.7994f, 0.8735f, 0.92023f, 0.9497f, 0.968281f, 0.980f };

    void Update()
    {

        /// moving without FPGA
        horizontalmove = Input.GetAxisRaw("Horizontal");
        verticalmove = Input.GetAxisRaw("Vertical");
        Vector2 movementDirection = new Vector2(horizontalmove, verticalmove);
               
        transform.Translate(movementDirection * scaledspeed * Time.deltaTime, Space.World);
        movementDirection.Normalize();

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, -movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        }
    }

    }

  //   void Find_team(string[] teamA, string[] teamB)
  //   {
  //       Debug.Log("teamA is null? " + (teamA == null), gameObject);
  //       for (int i = 0; i < teamA.Length; i++)
  //       {

  //           if (UserID == teamA[i])
  //           {
  //               Team = "A";
  //           }
  //       }
  //       for (int g = 0; g < teamB.Length; g++)
  //       {
  //           if (UserID == teamB[g])
  //           {
  //               Team = "B";
  //           }
  //       }

  //   }



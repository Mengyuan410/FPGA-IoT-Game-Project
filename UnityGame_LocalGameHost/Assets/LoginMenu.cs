using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LoginMenu : MonoBehaviour
{



    public EventSystem system;
    public Selectable firstInput;
    public GameObject SystemReturnMVP;
    public Button summitButton;
    public string userID, password;
    public fakeDataBase fakeDataBase;
    GameObject SystemReturnMessage;
   

    GameObject FPGAinput;
    inputfpga FPGAinputscript;
    MyNetworkManger DataBaseMessage;
    public DataBase dataBase_message;
    public GameObject LoginMenu1;
    int previousInput;
    int previousButton0Pressed;
    float t1 = 0;
    float t2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        //input
        FPGAinput = GameObject.Find("InputFPGA");
        FPGAinputscript = FPGAinput.GetComponent<inputfpga>();
        DataBaseMessage = GameObject.Find("MyNetworkManger").GetComponent<MyNetworkManger>();

        //SystemBotton Setting
        system = EventSystem.current;
        firstInput.Select();

        //output
        SystemReturnMessage = GameObject.Find("SystemReturnMessage");

        //SystemReturnMVP = GameObject.Find("SystemReturnMVP");
    }

    // Update is called once per frame
    void Update()
    {
        //moving up
        if (LoginMenu1.activeSelf)
        {
            userID = FPGAinputscript.username;
            GameObject.Find("nameInputField").gameObject.GetComponent<InputField>().text = userID;
        }
        if (FPGAinputscript.moving_direction == 1)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                    if (previous != null)
                    {
                        previous.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }

        }
        //moving down
        else if (FPGAinputscript.moving_direction == 2)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                    if (next != null)
                    {
                        next.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
        }
        //moving left
        else if (FPGAinputscript.moving_direction == 3)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable left = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
                    if (left != null)
                    {
                        left.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
        }
        //moving right
        else if (FPGAinputscript.moving_direction == 4)
        {
            t1 = Time.time;
            if (t1 - t2 > 1)
            {
                try
                {
                    Selectable right = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
                    if (right != null)
                    {
                        right.Select();
                    }
                }
                catch
                {
                    Debug.Log("NoButton");
                }
                t2 = Time.time;
            }
        }

        if (FPGAinputscript.Button0_Pressed == 1)
        {

            Debug.Log("<----- Button 0 Pressed ----->");
            //while(FPGAinputscript.Button0_Pressed == 1)
            //{
            previousButton0Pressed = 1;
            Debug.Log("inside loop");

        }
        else
        {
            if (previousButton0Pressed == 1)
            {
                Debug.Log("<----- Button 0 Leave ----->");
                system.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke(); //sos
                previousButton0Pressed = 0;
            }
            
        }

        //updating system message
        dataBase_message = DataBaseMessage.dataBase;
        if(dataBase_message.Register == null)
        {
            //SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Loading ... ";
        }
        else
        {
            if(dataBase_message.Register == "YES")
            {
                SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Successfully Login!";
                SystemReturnMVP.SetActive(true);
                SystemReturnMVP.GetComponent<TMPro.TextMeshProUGUI>().text = "MVP:   " + dataBase_message.MVP + "     " + "HighestNum:   " + dataBase_message.HighestKill;
            }
            else 
            {
                SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "No Account detail : Register as a new player";
            }
            
        }
       



    }
    public void Login()
    {
        Debug.Log("onSummit");
        
        //password = GameObject.Find("passwordInputField").gameObject.GetComponent<InputField>().text.ToString();
        //if (fakeDataBase.userVerification(userName, Convert.ToInt32(password)))
        //if(fakeDataBase.userVerification_without_password(Convert.ToInt32(userID)))
        //{
           
            //Debug.Log("fakeDataBase is null? " + (fakeDataBase.systemReturnMessage == null), gameObject);
            //Debug.Log("SystemReturnMessage is null? " + (SystemReturnMessage.GetComponent<Text>().text == null), gameObject);
        SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Loading ... ";
        Debug.Log("onSummit" + userID);
            //GameObject.Find("/LoginMenu").SetActive(false);
            //GameObject.Find("/MainMenu").SetActive(true);

        //}

        //Debug.Log("fakeDataBase is null? " + (fakeDataBase == null), gameObject);

        //SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = fakeDataBase.systemReturnMessage;

    }



}

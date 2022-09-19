using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class fakeDataBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int[] userIDList;

    public Dictionary<string,int> UserInform;
    public string systemReturnMessage;
    void Start()
    {
        UserInform = new Dictionary<string, Int32>();
        UserInform.Add("UserName", 1);  
    }

    public bool CreateItem(string userName, int password)
    {
        UserInform.Add(userName, password);
        systemReturnMessage = "Register Success";
        return true;
    }

    public bool ResetItem(string userName, Int32 old_password, Int32 new_password)
    {
        if (UserInform.ContainsKey(userName))
        {
            if (UserInform[userName] == old_password)
            {
                UserInform[userName] = new_password;
                systemReturnMessage = "Set Password Success";
                return true;
            }
            else
            {
                systemReturnMessage = "Error: Old Password Incorrect";
                return false;
            }
        }
        else
        {
            systemReturnMessage = "Error: UserName Not Registed";
            return false;
        }
        
    }
    public bool userVerification(string userName, int password)
    {
        //Debug.Log("UserInform is null? " + (UserInform == null), gameObject);
        if (UserInform.ContainsKey(userName))
        {
            if (UserInform[userName] == password)
            {
                systemReturnMessage = "Login Success";
                return true;
            }
            else
            {
                systemReturnMessage = "Error: Password Incorrect";
                return false;
            }
        }
        else
        {
            systemReturnMessage = "Error: UserName Not Found";
            return false;
        }
    }

    public bool userVerification_without_password(Int32 userID)
    {
        //Debug.Log("UserInform is null? " + (UserInform == null), gameObject);
        if (Array.Find(userIDList, element => element == userID) != null)
        {
            systemReturnMessage = "Success: Login";
            return true;
        }
        else
        {
            systemReturnMessage = "Success: Created User";
            return true;
        }
       
    }
}

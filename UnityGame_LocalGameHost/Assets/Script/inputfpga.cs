using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Windows;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class inputfpga : MonoBehaviour
{
    static inputfpga instance;
    static Scene m_Scene;
    static GameObject healthBar;
    static GameObject KillScoreDisplay;
    static GameObject SystemReturnMessage;
    static int FinalKillNum;
    static Text kill_num_display;

    public string systemMessage;
    Thread mThread;
    //public string connectionIP = "127.0.0.1";
    private int connectionPort = 8052;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos = Vector3.zero;
    public int moving_direction;
    public int speed;
    public int eat;
    public int bomb;
    public string username;
    public int Button0_Pressed;
    public int Button1_Pressed;
    bool running;

    public int prev_kill_num = 0;
    
    public int cur_kill_num = 0;

    public string pre_health;
    public string current_health;
    
    public string scene_name;
    public int i = 0;
    GameObject opponent_car; 
    MyNetworkManger opponent_carscript;



void Awake()
    {
        
        m_Scene = SceneManager.GetActiveScene();
        Debug.Log("FPGA: " + m_Scene.name);
        if (m_Scene.name == "SampleScene")
        {

            Debug.Log("&&&inside SampleScene");
            healthBar = GameObject.Find("Health bar");
            kill_num_display = GameObject.Find("KillNum").GetComponent<Text>();

            prev_kill_num = 0;
            cur_kill_num = 0;
        }
        else if(m_Scene.name == "OverScene")
        {
            Debug.Log("inside overscene");
            //systemMessage = "Pending";
            KillScoreDisplay = GameObject.Find("ScoreKillNumber");
            SystemReturnMessage = GameObject.Find("SystemReturnMessage");
            
            Debug.Log("overscene done");

        }
        else if (m_Scene.name == "WinScene")
        {
            Debug.Log("inside winscene");
            systemMessage = "WIN";
            KillScoreDisplay = GameObject.Find("ScoreKillNumber");
            SystemReturnMessage = GameObject.Find("SystemReturnMessage");

        }
        //else if(m_Scene.name == "UI")
        //{

        //}
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    private void Update()
    {
        //transform.position = receivedPos; //assigning receivedPos in SendAndReceiveData()
        scene_name = m_Scene.name;
        Debug.Log("FGPA/UPDATE:  name" + m_Scene.name);
        if (m_Scene.name == "SampleScene")
        {
            try
            {
                SendData();
                kill_num_display.text = cur_kill_num.ToString();
            }
            catch
            {
                Debug.Log("Sending Fail");
            }
        }
        else if (m_Scene.name == "OverScene")
        {
            systemMessage = "LOSE";
            Debug.Log("FGPA/UPDATE: " + m_Scene.name);
            KillScoreDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = cur_kill_num.ToString();
            SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: " + systemMessage;
            try
            {
                SendData();
                kill_num_display.text = cur_kill_num.ToString();
            }
            catch
            {
                Debug.Log("Sending Fail");
            }
        }
        else if (m_Scene.name == "WinScene")
        {
            Debug.Log("FGPA/UPDATE: " + m_Scene.name);
            systemMessage = "WIN";
            KillScoreDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = cur_kill_num.ToString();
            SystemReturnMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "Status: " + systemMessage;
            try
            {
                SendData();
                kill_num_display.text = cur_kill_num.ToString();
            }
            catch
            {
                Debug.Log("Sending Fail");
            }
        }

        //FinalKillNum = cur_kill_num;

    }

    private void Start()
    {
        i = i + 1;
        try{
        connectionPort = 8052;
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.IsBackground = true;
        mThread.Start();
        }catch (SocketException socketException)
		{
			Debug.Log("Socket exception aba: " + socketException);
		}
         
        //healthBarScript = healthBar.GetComponent<HealthBar>();
        opponent_car = GameObject.Find("MyNetworkManger");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();

    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse("127.0.0.1");
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        Debug.Log("socket built");
        client = listener.AcceptTcpClient();
        Debug.Log("connection built");
        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string


        if (dataReceived != null)
        {
            Debug.Log(dataReceived);
            string[] lines = dataReceived.Split(',');
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string[] result = lines[i].Split('/');
                if(result[0] == "Initial")
                {
                    username = result[1];
                }
                else
                {
                    Debug.Log(result.Length);
                    moving_direction = int.Parse(result[0]);
                    speed = int.Parse(result[1]);
                    eat = int.Parse(result[2]);
                    bomb = int.Parse(result[3]);
                    Button1_Pressed =  int.Parse(result[3]);
                    Button0_Pressed = int.Parse(result[4]);
                }               
            }
        }
    }
    

    private void SendData()
	{

		if (client == null)
		{
            Debug.Log("inside this branch");
			return;
		}
		try
		{
			NetworkStream stream = client.GetStream();
            Debug.Log("Got Stream");
            Debug.Log("Check if :" + stream.CanWrite );
            if (stream.CanWrite)
            {
                if (scene_name == "SampleScene" )
                {
                    try{
                        Debug.Log("healthBar is null? " + (healthBar == null), gameObject);
                        Debug.Log("slide is null? " + (healthBar.GetComponent<Slider>() == null), gameObject);
                        Debug.Log("value is null? " + (healthBar.GetComponent<Slider>().value == null), gameObject);
                        current_health = healthBar.GetComponent<Slider>().value.ToString();
                       
                    }
                    catch{
                        Debug.Log("gg");
                    }
                    
                    if(current_health != pre_health || prev_kill_num != cur_kill_num){
                        Debug.Log("inside 168");
                        pre_health = current_health;
                        prev_kill_num = cur_kill_num;
                        string clientMessage = 
                            "" + current_health + "/" +
                            prev_kill_num.ToString() + ",";             
                        byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                        // Write byte array to socketConnection stream.                 
                        stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                        Debug.Log("Client sent his message - should be received by server" + clientMessage);
                    }
                }
                else if(scene_name == "OverScene")
                {
                    Debug.Log("OverScene%%" + systemMessage);
                    //if (systemMessage != "Pending")
                    //{

                    //   string clientMessage = systemMessage;
                    //  Debug.Log("%%" + clientMessage);
                    //  byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);            
                    //  stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    //  Debug.Log("Client sent his message - should be received by server" + clientMessage);
                    //}
                    if (systemMessage=="LOSE")
                    {

                        string clientMessage = "LOSE";
                        byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                        stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                        Debug.Log("Client sent his message - should be received by server" + clientMessage);
                    }
                }
                else if(scene_name == "WinScene")
                {
                    string clientMessage = "WIN";  
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                    // Write byte array to socketConnection stream.                 
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    Debug.Log("Client sent his message - should be received by server" + clientMessage);
                    
                }
               //if(scene_name == "OverScene")
               // {
               //     try
               //     {
                       // 
               //     }
               // }
            }		
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
    }

}
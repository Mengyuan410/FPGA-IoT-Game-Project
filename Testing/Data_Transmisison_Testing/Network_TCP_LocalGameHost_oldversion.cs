using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MyNetworkManger : MonoBehaviour
{
	public int count = 0;
	
	#region private members
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	public Text connection_state;
	public float x_axis;
	public float y_axis;
	public float z_rotation;
	public float bomb_x_axis;
	public float bomb_y_axis;
	string test;
	public bool starts;
	public bool renew_bomb = false, ini_player = false;
	public int maxHealth, health;
	public float cd;
	public Initiate ini;
	
       

	#endregion

	GameObject car,healthBar,cdBar;
	Bomb bomb_placed_position;

	


	// Use this for initialization 	
	void Start()
	{
		ConnectToTcpServer();
		test = "fail";
		starts = false;
		car = GameObject.Find("Cars");
		healthBar = GameObject.Find("Health bar");
		cdBar = GameObject.Find("CDbar");
		bomb_placed_position = car.GetComponent<Bomb>();
	}
	// Update is called once per frame
	void Update()
	{
		connection_state.text = test;
        if((count > 50 && starts == true )|| (bomb_placed_position.bomb_placed == true))
        {
			ini.ini_player();
			ini.ini_bomb();
			ini.SynHealth_CD();
			SendMessage();
			count = 0;
        }
		count += 1;
		
		
	}
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData()
	{
		try
		{
			
			socketConnection = new TcpClient("192.168.0.146", 11000);
			test = "success";
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData);
						if(serverMessage == "start")
                        {
							starts = true;
							ini_player = true;
							
						}
                        else
                        {
							string[] MessageContainer = serverMessage.Split('/');
							x_axis = float.Parse(MessageContainer[0]);
							y_axis = float.Parse(MessageContainer[1]);
							z_rotation = float.Parse(MessageContainer[2]);
							maxHealth = int.Parse(MessageContainer[3]);
							health = int.Parse(MessageContainer[4]);
							cd = float.Parse(MessageContainer[5]);


							Debug.Log(serverMessage);
							// Debug.Log("message Length2" + MessageContainer[1]);
							if(MessageContainer.Length > 6)
							{
								renew_bomb = true;
								bomb_x_axis = float.Parse(MessageContainer[6]);
								bomb_y_axis = float.Parse(MessageContainer[7]);
								//Instantiate(bombPrefabs, new Vector3(bomb_x_axis, bomb_y_axis, 0), Quaternion.identity);
								//Debug.Log("bomb_x_axis" + MessageContainer[2]);
								//Debug.Log("bomb_y_axis" + MessageContainer[3]);
								//Debug.Log("server message received as: " + serverMessage);
							}
							else
							{
								Debug.Log("there is no  bomb");
							}
						}	

                }
            }
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	private void SendMessage()
	{
		
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			Vector3 posi = car.transform.position;
			float rotate = car.transform.localRotation.eulerAngles.z;
			
			
			
			if (bomb_placed_position.bomb_placed)
			{ 
				NetworkStream stream = socketConnection.GetStream();
				Debug.Log("With Bomb");
				if (stream.CanWrite)
				{
					Debug.Log("healthBar is null? " + (healthBar == null), gameObject);

					Debug.Log("cdBar is null?2 " + (cdBar == null), gameObject);
					string clientMessage =
						posi[0].ToString() + "/" +								//0 Transition
						posi[1].ToString() + "/" +								//1
						rotate.ToString()  + "/" +								//2
						healthBar.GetComponent<Slider>().maxValue.ToString() + "/" +		//3 Health Bar
						healthBar.GetComponent<Slider>().value.ToString() + "/" +			//4
						cdBar.GetComponent<Slider>().value.ToString() + "/" +				//5 CD Bar
						bomb_placed_position.bomb_x_axis.ToString() + "/" +		//6 Bomb
						bomb_placed_position.bomb_y_axis.ToString();			//7
					// Convert string message to byte array.                 
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
					// Write byte array to socketConnection stream.                 
					stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					Debug.Log("Client sent his message - should be received by server");
					Debug.Log(clientMessage);
					bomb_placed_position.bomb_placed = false;
				}
			}else
			{
				//without bomb
				NetworkStream stream = socketConnection.GetStream();
				Debug.Log("Without Bomlb");
				if (stream.CanWrite)
                {
					Debug.Log("healthBar is null? " + (healthBar == null), gameObject);

					Debug.Log("cdBar is null?2 " + (cdBar == null), gameObject);
					string clientMessage =
						posi[0].ToString() + "/" +                              //0 Transition
						posi[1].ToString() + "/" +                              //1
						rotate.ToString() + "/" +                               //2
						healthBar.GetComponent<Slider>().maxValue.ToString() + "/" +       //3 Health Bar
						healthBar.GetComponent<Slider>().value.ToString() + "/" +          //4
						cdBar.GetComponent<Slider>().value.ToString();		                //5 CD Bar
																				// Convert string message to byte array.                 
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
					// Write byte array to socketConnection stream.                 
					stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					Debug.Log("Client sent his message - should be received by server");
					Debug.Log(clientMessage);
				}
			}
			// Get a stream object for writing. 			
			
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
		
	}
}
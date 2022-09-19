using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Net;


public struct Mvpcount
{
	public string userId;
	public string score;
};
public struct HighestKillNo
{
	public string userId;
	public string score;
};
public struct Ranked
{
	public string gameID,startTime,endTime,MVPUserID,Team;

	public string[] teamAUserID;
	public string[] teamBUserID;
};
public struct DataBase
{
	public string Register;
	public string MVP;
	public string HighestKill;
};
public class MyNetworkManger : MonoBehaviour
{
	static MyNetworkManger instance;
	static Scene m_Scene;
	static Text connection_state;
	static GameObject SystemReturnMessage;
	static GameObject KillNum;
	#region private members
	public int count = 0;
	private UdpClient socketConnection;
	private IPEndPoint remoteEndPoint; 
    private string IP;  
    public int port_send; 
    public int port_receive;
	private Thread clientReceiveThread;
	public ChangeScene change;

	public float x_axis_car1, x_axis_car2, x_axis_car3, x_axis_car4, x_axis_car5;
	public float y_axis_car1, y_axis_car2, y_axis_car3, y_axis_car4, y_axis_car5;
	public float z_rotation_car1, z_rotation_car2, z_rotation_car3, z_rotation_car4, z_rotation_car5;
	public int maxHealth_car1, maxHealth_car2, maxHealth_car3, maxHealth_car4, maxHealth_car5;
	public int health_car1, health_car2, health_car3, health_car4, health_car5;
	public float cd_car1, cd_car2, cd_car3, cd_car4, cd_car5;
	public List<string> car_username;
	public float bomb_x_axis_car1, bomb_x_axis_car2, bomb_x_axis_car3, bomb_x_axis_car4, bomb_x_axis_car5;
	public float bomb_y_axis_car1, bomb_y_axis_car2, bomb_y_axis_car3, bomb_y_axis_car4, bomb_y_axis_car5;
	public bool renew_bomb_car1, renew_bomb_car2, renew_bomb_car3, renew_bomb_car4, renew_bomb_car5 = false;
	public string Destroy_Diamond_Name;

	public string CarTeam_a1 = "", CarTeam_a2 = "", CarTeam_a3 = "";
	public string CarTeam_b1 = "", CarTeam_b2 = "", CarTeam_b3 = "";



	public Button playButton, loginButton;
	public bool game_start = false;
	public string menu;
	public LoginMenu login;
	public string host_health;
	public bool sendUserId, sendTeam = false;
	public string scene_name;
	// represent the user number inside the 1 - 6
	public int user_position = 0;
	public int client_postition = 0;
	
	//local Send
	public Initiate ini;
	GameObject FPGAinput;
	inputfpga FPGAinputscript;
	GameObject TeamM;
	TeamManager TeamMscript;
	static GameObject car, healthBar, cdBar;
	static Bomb bomb_placed_position;
	static cars car_script;

	public bool renew_bomb = false;
	public bool Destroy_Diamond = false;

	string test = "fail";

	//scene over
	public string systemMessage;
	
	public bool request_on;
	public bool win_signal_send;
	public Mvpcount[] mvpcount_table = new Mvpcount[6];
	public HighestKillNo[] highestKillNo_table = new HighestKillNo[6];
	public Ranked[] ranked_table = new Ranked[6];
	bool move_on_to_next = false;

	//Display in UI for LoginMenu
	public DataBase dataBase;




	//public Dictionary<string, string> mvpcount = new Dictionary<string, string>();
	#endregion

	//GameObject changescene;

	//ChangeScene changescenescript;

	void Awake()
	{
		m_Scene = SceneManager.GetActiveScene();
		scene_name = m_Scene.name;
		
		if (m_Scene.name == "SampleScene")
		{
			Debug.Log("loadin");
			car = GameObject.Find("Cars");
			//send
			bomb_placed_position = car.GetComponent<Bomb>();
			car_script = car.GetComponent<cars>();

			healthBar = GameObject.Find("Health bar");
			cdBar = GameObject.Find("CDbar");
			connection_state = GameObject.Find("ConnectionS").GetComponent<Text>();
			//Debug.Log("healthBar is null? " + (connection_state == null), gameObject);
			connection_state.text = test;
			//Debug.Log("text is null? " + (connection_state.text == null), gameObject);

			
		}
		else if (m_Scene.name == "OverScene" || m_Scene.name == "WinScene")
		{
			//for further system is telling
			Debug.Log("My Network now inside" + m_Scene.name);
			SystemReturnMessage = GameObject.Find("SystemReturnMessage");

			KillNum = GameObject.Find("ScoreKillNumber");		
		}
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

	void Start()
	{
		health_car1 = 3;
		health_car2 = 3;
		health_car3 = 3;
		health_car4 = 3;
		health_car5 = 3;
		maxHealth_car1 = 3; 
		maxHealth_car2 = 3;
		maxHealth_car3 = 3;
		maxHealth_car4 = 3;
		maxHealth_car5 = 3;
		win_signal_send = true;
        port_receive = 8080;
		IP= "34.229.159.143";
		port_send = 12000;
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port_send);
		ConnectToTcpServer();
		FPGAinput = GameObject.Find("InputFPGA");
		FPGAinputscript = FPGAinput.GetComponent<inputfpga>();
		TeamM = GameObject.Find("TeamManager");
		TeamMscript = TeamM.GetComponent<TeamManager>();
		loginButton.onClick.AddListener(login_button_pressed);
		playButton.onClick.AddListener(play_button_pressed);
		connection_state = GameObject.Find("ConnectionS").GetComponent<Text>();
		connection_state.text = test;



	}


	void Update()
	{
		if (m_Scene.name == "UI")
		{
			try
			{
				if (socketConnection != null)
				{
					//Debug.Log("is socketConnected" + socketConnection.Connected);
					connection_state.text = test;
					//NetworkStream stream = socketConnection.GetStream();
					if (sendUserId)
					{
						string input = "ID" + login.userID;
						Debug.Log(input);
						Debug.Log("sendUserId");
						byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(input);
						socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
						//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
						Debug.Log(input);
						sendUserId = false;
					}
					else if (sendTeam)
					{

						string input = "T" + TeamMscript.Team;
						//Debug.Log(input);
						Debug.Log("sendTeam");
						byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(input);
						socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
						//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
						Debug.Log("gggggggg" + input);
						sendTeam = false;
					}

				}
			}
			catch (SocketException socketException)
			{
				Debug.Log("Socket exception: " + socketException);
			}
		}
		else if (m_Scene.name == "SampleScene")
		{
			try
			{
				connection_state.text = test;
				if (count > 5 || bomb_placed_position.bomb_placed == true)
				{
					count = 0;
					//ini.ini_player();
					ini.Destroy_Diamond();
					Debug.Log("sending message");
					SendMessage_Ingame();
					Debug.Log("sending completed");
				}
				count += 1;

			}
			catch
			{
				Debug.Log("null");
			}

		}
		else if (m_Scene.name == "OverScene")
		{
			//gameOver Scene you r certainly not the MVP
			if (socketConnection != null)
			{
				connection_state.text = test;
				//NetworkStream stream = socketConnection.GetStream();
				if (request_on)
				{

					string input = "E" + KillNum.GetComponent<TMPro.TextMeshProUGUI>().text + "0";
				
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(input);
					socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
					//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					Debug.Log(input);
					
					request_on = false;
				}

			}


		}
		else if (m_Scene.name == "WinScene")
		{
			//gameOver Scene you r certainly not the MVP
			if (socketConnection != null)
			{
				connection_state.text = test;
				//NetworkStream stream = socketConnection.GetStream();
				
				string endSignal = "END";
				
				
				if(win_signal_send)
				{
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(endSignal);
					socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
					//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					Debug.Log(endSignal);
					win_signal_send = false;
				}
				
				if (request_on)
				{

					string input = "E" + KillNum.GetComponent<TMPro.TextMeshProUGUI>().text + "1";
					
				
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(input);
					socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
					//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					Debug.Log(input);
					
					request_on = false;
				}

			}


		}
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
		//Debug.Log("<--scene-->");
		//Debug.Log("scene function" + change.scene_name_change());
		
		try
		{
            //remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
            socketConnection = new UdpClient(port_receive);
			// socketConnection = new TcpClient("54.166.20.236", 12000);
			test = "success";
			Byte[] bytes = new Byte[1024];
			int message_index;

			while (true)
			{

				// Get a stream object for reading 				
				//using (NetworkStream stream = socketConnection.GetStream())
				//{
					int length;
					IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
					//SOS
					byte[] incommingData = socketConnection.Receive(ref anyIP);
					// Read incomming stream into byte arrary. 					
					//while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					//{
						// var incommingData = new byte[length];
						// Array.Copy(bytes, 0, incommingData, 0, length);
						string serverMessage = Encoding.ASCII.GetString(incommingData);//// Convert byte array to string message. 
																					   //Here Start indicate start receving ingame message 
						Debug.Log("<-- message from server -->" + serverMessage);
						//if (serverMessage == "start")
						//{
						//	game_start = true;
						//  ini_player = true;
						//}


						if (change.scene_name == "SampleScene")
						{
							//Debug.Log("enter sample scene");
							string[] CarInforContainer = serverMessage.Split(',');
							//Debug.Log("Car Container Lenght" + CarInforContainer.Length.ToString());
							//if (CarInforContainer.Length > 2)
							//{
							Debug.Log("<--- inside IF -->");
							//if (CarInforContainer.Length > 1)
							//{
							for (int i = 0; i < CarInforContainer.Length; i++)
							{
								//Debug.Log("<--- inside loop --->");
								string[] MessageContainer = CarInforContainer[i].Split('/');
								//Debug.Log("Car Container Lenght" + CarInforContainer.Length.ToString());
								Debug.Log("<-- message from server Splitted Version-->" + CarInforContainer[i]);
								try
								{
									message_index = int.Parse(MessageContainer[0]);
									if (user_position > message_index)
									{
										client_postition = message_index;

									}
									else if (user_position == message_index)
									{
										client_postition = -1;
										//same as user, ignore
									}
									else
									{
										client_postition = message_index - 1;
									}

								}
								catch
								{
									// when parse 
									client_postition = -1;
								}

								Debug.Log("<<<client position" + client_postition.ToString() + ">>>");
								switch (client_postition)
								{
									case 1:
										Debug.Log("<--- Car 1 -->");
										x_axis_car1 = float.Parse(MessageContainer[1]);
										y_axis_car1 = float.Parse(MessageContainer[2]);
										z_rotation_car1 = float.Parse(MessageContainer[3]);
										maxHealth_car1 = int.Parse(MessageContainer[4]);
										health_car1 = int.Parse(MessageContainer[5]);
										cd_car1 = float.Parse(MessageContainer[6]);
										if (MessageContainer.Length > 7)
										{
											if (MessageContainer[7].Substring(0, 2) == "$B")
											{
												renew_bomb_car1 = true;
												bomb_x_axis_car1 = float.Parse(MessageContainer[7].Substring(2, MessageContainer[7].Length - 2));
												bomb_y_axis_car1 = float.Parse(MessageContainer[8]);
											}
											else if (MessageContainer[7].Substring(0, 2) == "$D")
											{
												Destroy_Diamond = true;
												Destroy_Diamond_Name = MessageContainer[7].Substring(1, MessageContainer[7].Length - 1);
											}
										}
										else
										{
											Debug.Log("there is no  bomb");
										}
										break;
									case 2:
										Debug.Log("<--- Car 2 -->");
										x_axis_car2 = float.Parse(MessageContainer[1]);
										y_axis_car2 = float.Parse(MessageContainer[2]);
										z_rotation_car2 = float.Parse(MessageContainer[3]);
										maxHealth_car2 = int.Parse(MessageContainer[4]);
										health_car2 = int.Parse(MessageContainer[5]);
										cd_car2 = float.Parse(MessageContainer[6]);
										if (MessageContainer.Length > 7)
										{
											//if (MessageContainer[7].Substring(0, 1) == "$")
											//{
											//	Debug.Log("there is a bomb");
											//	renew_bomb_car2 = true;
											//	bomb_x_axis_car2 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 1));
											//	bomb_y_axis_car2 = float.Parse(MessageContainer[8]);
											//}
											if (MessageContainer[7].Substring(0, 2) == "$B")
											{
												Debug.Log("there is a bomb");
												renew_bomb_car2 = true;
												bomb_x_axis_car2 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 2));
												bomb_y_axis_car2 = float.Parse(MessageContainer[8]);
											}
											else if (MessageContainer[7].Substring(0, 2) == "$D")
											{
												Destroy_Diamond = true;
												Destroy_Diamond_Name = MessageContainer[7].Substring(1, MessageContainer[7].Length - 1);
											}
										}
										else
										{
											Debug.Log("there is no  bomb");
										}
										break;
									case 3:
										x_axis_car3 = float.Parse(MessageContainer[1]);
										y_axis_car3 = float.Parse(MessageContainer[2]);
										z_rotation_car3 = float.Parse(MessageContainer[3]);
										maxHealth_car3 = int.Parse(MessageContainer[4]);
										health_car3 = int.Parse(MessageContainer[5]);
										cd_car3 = float.Parse(MessageContainer[6]);
										if (MessageContainer.Length > 7)
										{
											//if (MessageContainer[7].Substring(0, 1) == "$")
											//{
											//	renew_bomb_car3 = true;
											//	bomb_x_axis_car3 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 1));
											//	bomb_y_axis_car3 = float.Parse(MessageContainer[8]);

											//}
											if (MessageContainer[7].Substring(0, 2) == "$B")
											{
												Debug.Log("there is a bomb");
												renew_bomb_car2 = true;
												bomb_x_axis_car2 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 2));
												bomb_y_axis_car2 = float.Parse(MessageContainer[8]);
											}
											else if (MessageContainer[7].Substring(0, 2) == "$D")
											{
												Destroy_Diamond = true;
												Destroy_Diamond_Name = MessageContainer[7].Substring(1, MessageContainer[7].Length - 1);
											}
										}
										else
										{
											Debug.Log("there is no  bomb");
										}
										break;

									case 4:
										x_axis_car4 = float.Parse(MessageContainer[1]);
										y_axis_car4 = float.Parse(MessageContainer[2]);
										z_rotation_car4 = float.Parse(MessageContainer[3]);
										maxHealth_car4 = int.Parse(MessageContainer[4]);
										health_car4 = int.Parse(MessageContainer[5]);
										cd_car4 = float.Parse(MessageContainer[6]);
										if (MessageContainer.Length > 7)
										{
											//if (MessageContainer[7].Substring(0, 1) == "$")
											//{
											//	renew_bomb_car4 = true;
											//	bomb_x_axis_car4 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 1));
											//	bomb_y_axis_car4 = float.Parse(MessageContainer[8]);

											//}
											if (MessageContainer[7].Substring(0, 2) == "$B")
											{
												Debug.Log("there is a bomb");
												renew_bomb_car2 = true;
												bomb_x_axis_car2 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 2));
												bomb_y_axis_car2 = float.Parse(MessageContainer[8]);
											}
											else if (MessageContainer[7].Substring(0, 2) == "$D")
											{
												Destroy_Diamond = true;
												Destroy_Diamond_Name = MessageContainer[7].Substring(1, MessageContainer[7].Length - 1);
											}
										}
										else
										{
											Debug.Log("there is no  bomb");
										}
										break;
									case 5:
										x_axis_car5 = float.Parse(MessageContainer[1]);
										y_axis_car5 = float.Parse(MessageContainer[2]);
										z_rotation_car5 = float.Parse(MessageContainer[3]);
										maxHealth_car5 = int.Parse(MessageContainer[4]);
										health_car5 = int.Parse(MessageContainer[5]);
										cd_car5 = float.Parse(MessageContainer[6]);
										if (MessageContainer.Length > 7)
										{
											//if (MessageContainer[7].Substring(0, 1) == "$")
											//{
											//	renew_bomb_car5 = true;
											//	bomb_x_axis_car5 = float.Parse(MessageContainer[7].Substring(1, MessageContainer[7].Length - 1));
											//	bomb_y_axis_car5 = float.Parse(MessageContainer[8]);
											//}
											if (MessageContainer[7].Substring(0, 2) == "$B")
											{
												Debug.Log("there is a bomb");
												renew_bomb_car2 = true;
												bomb_x_axis_car2 = float.Parse(MessageContainer[7].Substring(2, MessageContainer[7].Length - 2));
												bomb_y_axis_car2 = float.Parse(MessageContainer[8]);
											}
											else if (MessageContainer[7].Substring(0, 2) == "$D")
											{
												Destroy_Diamond = true;
												Destroy_Diamond_Name = MessageContainer[7].Substring(1, MessageContainer[7].Length - 1);
											}
										}
										else
										{
											Debug.Log("there is no  bomb");
										}
										break;
									default:
				  						Debug.Log("User");
										break;

								}
								

							}
						}
						else if (change.scene_name == "UI")
						{
							//Debug.Log("<----------------UI----------------->");
							//Debug.Log("scene variable" + change.scene_name);
							string[] MessageType = serverMessage.Split(';');
							//Start,A/1111/3333,B/2222/4444$2222/1111/3333/4444;
							for (int p = 0; p < MessageType.Length; p++)
							{
								if (MessageType[p].Length>5 && MessageType[p].Substring(0, 5) == "start")
								{

									string[] TeamSelection_or_Id = MessageType[p].Split('$');
									//TeamSelection_or_Id[1] = Start,A/1111/3333,B/2222/4444
									string[] UserTeamSelection = TeamSelection_or_Id[0].Split(',');
									//UserTeamSelection = { Start | A/1111/3333 | B/2222/4444 }
									for (int j = 1; j < 3; j++)
									{
										string[] MessageContainer = UserTeamSelection[j].Split('/');
										switch (MessageContainer[0])
										{
											case "A":

												for (int m = 1; m < MessageContainer.Length; m++)
												{
													switch (m)
													{
														case 1: CarTeam_a1 = MessageContainer[1]; break;
														case 2: CarTeam_a2 = MessageContainer[2]; break;
														case 3: CarTeam_a3 = MessageContainer[3]; break;
														default: Debug.Log("Error"); break;
													}

												}
												break;
											case "B":
												for (int m = 1; m < MessageContainer.Length; m++)
												{
													switch (m)
													{
														case 1: CarTeam_b1 = MessageContainer[1]; break;
														case 2: CarTeam_b2 = MessageContainer[2]; break;
														case 3: CarTeam_b3 = MessageContainer[3]; break;
														default: Debug.Log("Error"); break;
													}

												}
												break;
										}
									}

									string IDSelection = TeamSelection_or_Id[1];
									//IDSelection = 2222/1111/3333/4444

									Debug.Log("$$" + IDSelection);

									string[] car_id = IDSelection.Split('/');
									//carID = {2222 | 1111 | 3333 | 4444}

									for (int g = 0; g < car_id.Length; g++)
									{
										if (car_id[g] != FPGAinputscript.username)
										{
											car_username.Add(car_id[g]);
										}
										else
										{
											user_position = g + 1;

										}
									}

								}
								else if (MessageType[p].Length > 2 && MessageType[p].Substring(0, 2) == "$N")
								{
									dataBase.Register = MessageType[p].Substring(3, 4);
								}
								else if (MessageType[p].Length > 2 && MessageType[p].Substring(0, 2) == "$R")
								{
									string[] element = MessageType[p].Split('/');
									dataBase.Register = "YES";
									dataBase.MVP = element[1];
									dataBase.HighestKill = element[2];
								}
                                else if (MessageType[p].Length > 1 && MessageType[p].Substring(0, 1) == "A")
								{
									
									Debug.Log("<else condition>" + serverMessage);
									string[] UserTeamSelection = MessageType[p].Split(',');
									for (int k = 0; k < 2; k++)
									{
										string[] MessageContainer = UserTeamSelection[k].Split('/');
										switch (MessageContainer[0])
										{
											case "A":

												for (int m = 1; m < MessageContainer.Length; m++)
												{
													switch (m)
													{
														case 1: CarTeam_a1 = MessageContainer[1]; break;
														case 2: CarTeam_a2 = MessageContainer[2]; break;
														case 3: CarTeam_a3 = MessageContainer[3]; break;
														default: Debug.Log("Error"); break; ;
													}

												}
												break;
											case "B":
												for (int m = 1; m < MessageContainer.Length; m++)
												{
													switch (m)
													{
														case 1: CarTeam_b1 = MessageContainer[1]; break;
														case 2: CarTeam_b2 = MessageContainer[2]; break;
														case 3: CarTeam_b3 = MessageContainer[3]; break;
														default: Debug.Log("Error"); break;
													}

												}
												break;
										}
									}
								}
							}

						}
						else if (change.scene_name == "OverScene" || change.scene_name == "WinScene")
						{
							if (serverMessage.Substring(0, 3) == "REQ")
							{
								move_on_to_next = true;
								request_on = true;
							}
							else if(move_on_to_next)
							{
								string[] TableList = serverMessage.Split('!');
								for (int i = 0; i < 2; i++)
								{
									Debug.Log("<-- message from server Splitted ! -->" + TableList[i]);
									string[] TableRow = TableList[i].Split(';');
									for (int k = 0; k < TableRow.Length && k <6; k++)
									{
										Debug.Log("<-- message from server Splitted ; -->" + TableRow[k]);
										Debug.Log("K: 732" + k);
										string[] TableElement = TableRow[k].Split('/');
										switch (i+1)
										{
											case 1:
												Mvpcount mvpelement;
												mvpelement.userId = TableElement[0];
												mvpelement.score = TableElement[1];
												Debug.Log("K: " + k);
												mvpcount_table[k] = mvpelement;												
												break;
											case 2:
												HighestKillNo highestKillNo;
												highestKillNo.userId = TableElement[0];
												highestKillNo.score = TableElement[1];
												highestKillNo_table[k] = highestKillNo;
												break;
											case 3:
												Ranked ranked;
												ranked.gameID = TableElement[0];
												ranked.startTime = TableElement[1];
												ranked.endTime = TableElement[2];
												ranked.MVPUserID = TableElement[3];
												ranked.Team = TableElement[4];
												ranked.teamAUserID = TableElement[5].Split(',');
												ranked.teamBUserID = TableElement[6].Split(',');
												ranked_table[k] = ranked;
												break;
											default:
												Debug.Log("Error");
												break;

										}
									}
								}
							}
						}
					//}
				//}
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
	private void SendMessage_Ingame()
	{

		if (socketConnection == null)
		{
			Debug.Log("socket sample scene null");
			return;
		}
		try
		{
			Vector3 posi = car.transform.position;
			float rotate = car.transform.localRotation.eulerAngles.z;
			host_health = healthBar.GetComponent<Slider>().value.ToString();
			if (bomb_placed_position.bomb_placed)
			{
				//NetworkStream stream = socketConnection.GetStream();
				
				Debug.Log("With Bomb");
					//Debug.Log("healthBar is null? " + (healthBar == null), gameObject);

					//Debug.Log("cdBar is null?2 " + (cdBar == null), gameObject);
					string clientMessage =
						"1/" +
						posi[0].ToString() + "/" +                              //0 Transition
						posi[1].ToString() + "/" +                              //1
						rotate.ToString() + "/" +                               //2
						healthBar.GetComponent<Slider>().maxValue.ToString() + "/" +        //3 Health Bar
						healthBar.GetComponent<Slider>().value.ToString() + "/" +           //4
						cdBar.GetComponent<Slider>().value.ToString() + "/" +               //5 CD Bar
						"$B" + bomb_placed_position.bomb_x_axis.ToString() + "/" +     //6 Bomb
						bomb_placed_position.bomb_y_axis.ToString() + ",";            //7
																					   // Convert string message to byte array.                 
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
					// Write byte array to socketConnection stream.                 
					//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
					Debug.Log("Client sent his message - should be received by server");
					Debug.Log(clientMessage);
					bomb_placed_position.bomb_placed = false;
			}
			else
			{
				//without bomb
				//NetworkStream stream = socketConnection.GetStream();
				Debug.Log("Without Bomlb");
				
				//Debug.Log("healthBar is null? " + (healthBar == null), gameObject);
				if (car_script.Diamond_Distory)
				{
					Debug.Log("cdBar is null?2 " + (cdBar == null), gameObject);
					string clientMessage =
						"1/" +
						posi[0].ToString() + "/" +                              //0 Transition
						posi[1].ToString() + "/" +                              //1
						rotate.ToString() + "/" +                               //2
						healthBar.GetComponent<Slider>().maxValue.ToString() + "/" +       //3 Health Bar
						healthBar.GetComponent<Slider>().value.ToString() + "/" +          //4
						cdBar.GetComponent<Slider>().value.ToString() + "/" +
						"$" + car_script.Diamond_ID + ",";                      //5 CD Bar
																				// Convert string message to byte array.                 
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
					// Write byte array to socketConnection stream.                 
					//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
					Debug.Log("Client sent his message - should be received by server");
					Debug.Log(clientMessage);
					
				}
				else
				{
					Debug.Log("cdBar is null?2 " + (cdBar == null), gameObject);
					string clientMessage =
						"1/" +
						posi[0].ToString() + "/" +                              //0 Transition
						posi[1].ToString() + "/" +                              //1
						rotate.ToString() + "/" +                               //2
						healthBar.GetComponent<Slider>().maxValue.ToString() + "/" +       //3 Health Bar
						healthBar.GetComponent<Slider>().value.ToString() + "/" +          //4
						cdBar.GetComponent<Slider>().value.ToString() + ",";                      //5 CD Bar
																									// Convert string message to byte array.                 
					byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
					// Write byte array to socketConnection stream
					//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
					socketConnection.Send(clientMessageAsByteArray, clientMessageAsByteArray.Length, remoteEndPoint);
					car_script.Diamond_Distory = false;
					Debug.Log("Client sent his message - should be received by server");
					Debug.Log(clientMessage);
				}
			}
			if (healthBar.GetComponent<Slider>().value == 0)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}

			// Get a stream object for writing. 			
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}

	}
	private void SendMessage_UI()
	{
		if (socketConnection == null)
		{
			return;
		}
		//send userID when hit login
		//send team selection result realtime


	}
	// private void SendMessage_OverScene()
    // {

    // }
	public void login_button_pressed()
	{
		sendUserId = true;
	}
	public void play_button_pressed()
	{
		sendTeam = true;

	}
	
}

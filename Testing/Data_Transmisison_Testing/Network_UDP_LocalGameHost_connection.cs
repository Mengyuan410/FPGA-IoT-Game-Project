using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class UDPconnection : MonoBehaviour {

    private Thread receiveThread;
    private UdpClient socketConnection; 
    private IPEndPoint remoteEndPoint;
    // infos
    public string lastReceivedUDPPacket="";
    public string allReceivedUDPPackets=""; // clean up this from time to time!

    private string IP;  
    public int port_send; 
    public int port_receive;
    public float result_time;
   
   
    
    public void Start()
    {
        
        //client = new UdpClient();
        //print("Sending to "+IP+" : "+port);
        //print("Testing: nc -lu "+IP+" : "+port);
        //print("UDPSend.init()");
        //print("Sending to 127.0.0.1 : "+port);
        //print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
        receiveThread = new Thread(
        new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start(); 
        
    }
   void Update()
   {
       if (socketConnection == null)
		{
			Debug.Log("socket sample scene null");
			return;
		}
        try{
            sendString();
        }catch (Exception socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
       
   }
    // receive thread
    private void ReceiveData()
    {
 
        //SOS
        try{
            IP= "34.229.159.143";
            port_send = 12000;
            port_receive = 8052;
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port_send);
            socketConnection = new UdpClient(port_receive);
            Debug.Log("GG");
            
            
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            
             Debug.Log("Start testing");
            
        
            while (true)
            {
                try
                {
                    //Creates an IPEndPoint to record the IP Address and port number of the sender.
                    // The IPEndPoint will allow you to read datagrams sent from any source.
                    
                    byte[] data = socketConnection.Receive(ref anyIP);
                    string serverMessage = Encoding.ASCII.GetString(data);
                    Debug.Log("<-- message from server -->" + serverMessage);
                    result_time = float.Parse(serverMessage);
                    //Debug.Log("time difference " + result.ToString());
                    // lastReceivedUDPPacket=text;
                    // allReceivedUDPPackets=allReceivedUDPPackets+text;

                }
                catch (Exception err)
                {
                    Debug.Log("ListenFail1");
                }
            }
        }catch (Exception err)
        {
            Debug.Log("ListenFail2");
        }
    }
    // public string getLatestUDPPacket()
    // {
    //     allReceivedUDPPackets="";
    //     return lastReceivedUDPPacket;
    // }
   
    // getLatestUDPPacket
    // cleans up the rest
    private void sendString()
    {
        if(socketConnection != null){
            try
            {
                string clientMessage = Time.time.ToString();
                byte[] data_sent = Encoding.ASCII.GetBytes(clientMessage);
                socketConnection.Send(data_sent, data_sent.Length, remoteEndPoint);
                Debug.Log("Message sent");  
                
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }else{
            Debug.Log("socketConnection Not Found");
            return;
        }
    }
    //  private void sendEndless(string testStr)
    // {
    //     do
    //     {
    //         sendString(testStr);
    //     }
    //     while(true);
       
    // }
}



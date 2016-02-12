using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkDiscovery : NetworkDiscovery
{
    public static string serverIP="";
    void Start()
    {
//#if UNITY_EDITOR
   //     Initialize();
  //      StartAsServer();
//	NetworkManager.singleton.StartServer();
 //       Debug.Log("Started as server");
//#else 
     //   NetworkManager.singleton.networkAddress = fromAddress;
       
//#endif
      //  Initialize();
     //   StartAsClient();

    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        serverIP = fromAddress;
       Debug.Log("received from  " + serverIP);

        //if(NetworkManager.singleton.numPlayers<=0)
        //{
        //     Initialize();
        //     NetworkManager.singleton.networkAddress = fromAddress;
        //    NetworkManager.singleton.StartClient();
        //    Debug.Log("NADA!!!");

        //}

      //  NetworkManager.singleton.StartClient();
    }
}
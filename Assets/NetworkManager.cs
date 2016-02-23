using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

    private const string typeName = "UniqueGameName";
    private const string gameName = "RoomName";
   // NetworkView networkView;
    [SerializeField]
    private ToggleGroup turbulenceGroup;
    [SerializeField]
    private InputField hourField;
    [SerializeField]
    private InputField minutesField;
    [SerializeField]
    private  InputField LabelText;

    private enum ClimateStates {aberto, nublado, chuvoso, temporal }
    void start()
    {
        RefreshHostList();
       // MasterServer.ipAddress = Network.player.ipAddress;
        //MasterServer.port = 23466;
    }
    private void StartServer()
    {
        MasterServer.ipAddress = LabelText.text;
        MasterServer.port = 23466;
        
       // Network.InitializeServer();
        //Network.
        //MasterServer.RegisterHost(typeName, gameName);
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        
        MasterServer.RegisterHost(typeName, gameName);
    }


    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
    }

    public void startClient()
    {
        RefreshHostList();
        for (int i = 0; i < 2; i++) 
        { 
            if (hostList != null)
            {
                JoinServer(hostList[0]);
            }
            else
                Debug.Log("Erro!");
        }
    }

   /* void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
            {
                //MasterServer.ipAddress = MyNetworkDiscovery.serverIP;
                //MasterServer.port = 23466;
                RefreshHostList();
            }


            


            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }


        }

        if (GUI.Button(new Rect(400, 250, 250, 100), "Teste"))
            GetComponent<NetworkView>().RPC("ReceiveSimpleMessage", RPCMode.Server, "Hello world");
    }*/


    public void setTurbulence()
    {

        List<Toggle> tos = new List<Toggle>(turbulenceGroup.ActiveToggles());
        // tos[0] = null;
        
          GetComponent<NetworkView>().RPC("receiveTurbulenceChange", RPCMode.Server, tos[0].name);
    }


    public void setTime()
    {
        string time= hourField.text + ":" + minutesField.text;
        
        // tos[0] = null;

        GetComponent<NetworkView>().RPC("receiveTimeChange", RPCMode.Server,time);
    }

    public void cleanPressed()
    {
        GetComponent<NetworkView>().RPC("receiveClimateChange", RPCMode.Server, (int)ClimateStates.aberto);
    }

    public void cloudyPressed()
    {
        GetComponent<NetworkView>().RPC("receiveClimateChange", RPCMode.Server, (int)ClimateStates.nublado);
    }

    public void rainyPressed()
    {
        GetComponent<NetworkView>().RPC("receiveClimateChange", RPCMode.Server, (int)ClimateStates.chuvoso);
    }

    public void stormPressed()
    {
        GetComponent<NetworkView>().RPC("receiveClimateChange", RPCMode.Server, (int)ClimateStates.temporal);
    }

    private HostData[] hostList;
   

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }


    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    [RPC]
    void ReceiveSimpleMessage(string teste,NetworkMessageInfo info)
    {
        Debug.Log(teste + " from " + info.sender);

       // if (networkView.isMine)
            
    }


    [RPC]
    void receiveClimateChange(int clima)
    {
        Debug.Log("received >" + clima);

        // if (networkView.isMine)

    }


    [RPC]
    void receiveTurbulenceChange(string toggle)
    {
        Debug.Log("Active turbulence toggle is  " + toggle);
        //send
    }

    [RPC]
    void receiveTimeChange(string time)
    {
        Debug.Log("New time is  " + time);
       // messageManager.handleTime(time);
        //send
    }


    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }
}

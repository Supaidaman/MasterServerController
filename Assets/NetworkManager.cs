using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;

public class NetworkManager : MonoBehaviour {

    private const string typeName = "UniqueGameName";
    private const string gameName = "RoomName";
   // NetworkView networkView;
    [SerializeField]
    private ToggleGroup turbulenceGroup;
    [SerializeField]
    private ToggleGroup stressGroup;
    [SerializeField]
    private InputField hourField;
    [SerializeField]
    private InputField minutesField;
    [SerializeField]
    private  InputField labelText;
    [SerializeField]
    private GameObject  textEditorCanvas;
    [SerializeField]
    private InputField textField;
    [SerializeField]
    private Button connectButton;
    private Text buttonText;

    FileInfo f; 

    private enum ClimateStates {aberto, nublado, chuvoso, temporal }
    void Start()
    {
        buttonText = connectButton.GetComponentInChildren<Text>();
        f = new FileInfo(Application.persistentDataPath + "\\" + "myFile.txt");
       // RefreshHostList();
       // MasterServer.ipAddress = Network.player.ipAddress;
      // MasterServer.port = 23466;
    }
    private void StartServer()
    {
        MasterServer.ipAddress = labelText.text;
        Debug.Log(labelText.text);
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
        MasterServer.ipAddress = labelText.text;
        Debug.Log(labelText.text);
        MasterServer.port = 23466;
        buttonText.text = "Conectando";
        connectButton.interactable = false;
        for (int i = 0; i < 2; i++)
        {
            RefreshHostList();
            StartCoroutine(WaitForSeconds());
        }
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(10);
        if (hostList != null)
            {
                try
                {
                    JoinServer(hostList[0]);
                    connectButton.interactable = false;
                    buttonText.text = "Conectado!";
                }
                catch
                {
                    Debug.Log("Erro!");

                    Network.Disconnect();
                    connectButton.interactable = true;
                   buttonText.text = "Conectar!";
                }
            }
            else
            {
               // try
               // {
                    Debug.Log(labelText.text);
                    Debug.Log("Erro!");
                    Network.Disconnect();
                    connectButton.interactable = true;
                    buttonText.text = "Conectar!";
               // }
               // catch 
               // {

                    //Network.Disconnect();
                    connectButton.interactable = true;
                    buttonText.text = "Conectar!";
                //}
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

    public void setStressLevel()
    {

        List<Toggle> strt = new List<Toggle>(stressGroup.ActiveToggles());
        // tos[0] = null;

        GetComponent<NetworkView>().RPC("receiveStressChange", RPCMode.Server, strt[0].name);
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

    public void notationPressed()
    {
       textEditorCanvas.SetActive(true);
 
    }

    public void endWritePressed()
    {
        if (!f.Exists)
        {
            message = "Creating File";
            Save();
        }
        else
        {
            message = "Saving";
            Save();
        }
        textEditorCanvas.SetActive(false);
    }

    private void Save()
    {
        StreamWriter w;
        if (!f.Exists)
        {
            w = f.CreateText();
        }
        else
        {
            f.Delete();
            w = f.CreateText();
        }
        if(textField.text!=null)
            w.WriteLine(textField.text);
        w.Close();
        Debug.Log("Estou no fim do save");
    }

    public void Load()
    {
        StreamReader r = File.OpenText(Application.persistentDataPath + "\\" + "myFile.txt");
        string info = r.ReadToEnd();
        r.Close();
        textField.text = info;
        //data = info;
    }

    private HostData[] hostList;
    private string message;
   

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
    void receiveStressChange(string toggle)
    {
        Debug.Log("Active stress toggle is  " + toggle);
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

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        connectButton.interactable = true;
        buttonText.text = "Conectar!";
        Debug.Log("Disconnected from server: " + info);
    }
}

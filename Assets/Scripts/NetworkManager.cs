using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string serverGameName = "SurvivalShooter";
	private const string gameName = "RoomName";
	private HostData[] hostList;
	public GameObject playerPrefab;
	
	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(serverGameName, gameName);
	}


	//called when server is initialzied
	void OnServerInitialized()
	{
		SpawnPlayer();
	}


	private void RefreshHostList()
	{
		MasterServer.RequestHostList(serverGameName);
	}


	//fills the hostList array with hosts from the server
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}


	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}


	//called when connected to server
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}


	private void SpawnPlayer()
	{
		Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
	}


	void OnGUI()
	{
		//only shown if server is not initialized or if not into romm already
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

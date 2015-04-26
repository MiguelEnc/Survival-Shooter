using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class networkOnC : MonoBehaviour {
	public string ipAddress = "127.0.0.1";
	//public string ipAddress = "186.6.2.249";
	public int port = 25167;
	public int maxConnections = 10;
	public GameObject player;
	private string playerName = "";

	void OnGUI() {

		if(Network.peerType == NetworkPeerType.Disconnected){

			GUILayout.BeginVertical();

				GUILayout.BeginHorizontal();
					ipAddress = GUILayout.TextField(ipAddress);
					GUILayout.Label("Ip Address");
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
					string tempPort;
					tempPort = GUILayout.TextField(port.ToString());
					port = int.Parse(tempPort);
					GUILayout.Label("Port");
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
					GUILayout.Label(" ");
				GUILayout.EndHorizontal();

				GUILayout.Label(" Character name:");

				GUILayout.BeginHorizontal();
					playerName = GUILayout.TextField(playerName, 20, GUILayout.Width(100));
				GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			if(!playerName.Equals("")){
				if(GUILayout.Button("Connect")){
					print("Connecting...");
					JoinServer();
				}
				if(GUILayout.Button("Start Server")){
					print("Starting server on " + ipAddress + ":" + port);
					StartServer();
				}
			}
		}else {
			if(GUILayout.Button("Disconnect")){
				print("Disconnecting from: " + ipAddress + ":" + port);
				//Network.CloseConnection(Network.connections[0], true);
				OnPlayerDisconnected(Network.player);
				
				Network.Disconnect(200);
			}
		}
	}

	private void StartServer()
	{
		Network.InitializeServer(maxConnections,port);
		//GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		//foreach (GameObject i in gameObjects) {
		//	i.BroadcastMessage("SpawnPlayer", SendMessageOptions.DontRequireReceiver);
		//} 
	}

	private void JoinServer()
	{
		Network.Connect(ipAddress, port);
		//GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		//foreach (GameObject i in gameObjects) {
		//	i.BroadcastMessage("SpawnPlayer", SendMessageOptions.DontRequireReceiver);
		//}
	}

	//called when server is initialzied
	void OnServerInitialized()
	{
		print("Server initialized.");
		SpawnPlayer();
	}
	
	//called when connected to server
	void OnConnectedToServer()
	{
		print("Connected to server");
		SpawnPlayer();
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnPlayerDisconnectedFromServer(){
		Application.LoadLevel(Application.loadedLevel);
	}

	private void SpawnPlayer(){
		GameObject instantiatedPlayer = (GameObject) Network.Instantiate(player, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		//GameObject instantiatedPlayer = (GameObject) Network.Instantiate(player, transform.position, transform.rotation, 0);
		SetPlayerName(instantiatedPlayer);

	}
	void SetPlayerName(GameObject instantiatedPlayer)
	//[RPC] void SetPlayerName(GameObject instantiatedPlayer)
	{
		Canvas canvas = instantiatedPlayer.GetComponentInChildren<Canvas>();
		Image image = canvas.GetComponentInChildren<Image>();
		Text text = image.GetComponentInChildren<Text>();
		text.text = playerName;

		//networkView.RPC("SetPlayerName", RPCMode.All, instantiatedPlayer);
	}
}




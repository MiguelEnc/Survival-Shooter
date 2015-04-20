using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public GameObject player;
	private Vector3 playerPosition;

	// Use this for initialization
	void Start () {
		playerPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(playerPosition.x, playerPosition.y + 3.5f, playerPosition.z - 3.5f);
	}
}

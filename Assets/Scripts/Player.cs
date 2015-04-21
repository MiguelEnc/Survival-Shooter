using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float movementSpeed = 10f;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	//public string Name;

	void Awake(){
		//GetComponent<PlayerName>().nombre = Name;
	}
	
	void Update()
	{
		if (networkView.isMine)
		{
			InputMovement();
			InputColorChange();
		}
		else
		{
			SyncedMovement();
		}
	}


	private void InputColorChange()
	{
		if (Input.GetKeyDown(KeyCode.R))
			ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}

	//changes the player's color on the server
	[RPC] void ChangeColorTo(Vector3 color)
	{
		renderer.material.color = new Color(color.x, color.y, color.z, 1f);
		
		if (networkView.isMine)
			networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
	}

	//syncs the movement on the scene with the server
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	//user movement
	void InputMovement()
	{
		if (Input.GetKey(KeyCode.W))
			rigidbody.MovePosition(rigidbody.position + Vector3.forward * movementSpeed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.S))
			rigidbody.MovePosition(rigidbody.position - Vector3.forward * movementSpeed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.D))
			rigidbody.MovePosition(rigidbody.position + Vector3.right * movementSpeed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.A))
			rigidbody.MovePosition(rigidbody.position - Vector3.right * movementSpeed * Time.deltaTime);
	}

	//function called every time data is sent or received
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncStartPosition = rigidbody.position;
			syncEndPosition = syncPosition;
		}
	}
}
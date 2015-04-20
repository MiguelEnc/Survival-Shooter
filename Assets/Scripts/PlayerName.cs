using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class PlayerName : MonoBehaviour {

	public string nombre = "";
	private Text textComp;


	// Use this for initialization
	void Start () {
		textComp.text = nombre;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake(){
		textComp = GetComponent<Text>();
	}
}

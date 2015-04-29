var log : String = "";
var message : String = "";
var chatting : boolean = false;


function Update () {
	if(Input.GetKeyDown(KeyCode.Space)){
		chatting = true;	
	}
	if(Input.GetKeyDown(KeyCode.Escape)){
		chatting = false;
	}
}

function OnGUI() {
	GUILayout.Label(log);
	
	if(chatting){
		message = GUILayout.TextField(message);
		if(GUILayout.Button("Send Message")){
			networkView.RPC("Chat", RPCMode.All, message);
		}
	}
}

@RPC
function Chat(input: String){
	log += "\n";
	log += input;
}
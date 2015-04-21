#pragma strict
 
private var log:Array = new Array();
var maxLogMessages:int = 50;
var userInput : String = "";
var textFieldSelected : boolean = false;
 
function Start() {
	Input.eatKeyPressOnTextFieldFocus = false;
    log.Add("Press Enter to chat");
}
 
function print(string:String){
	log.push(string);
	
    if(log.length > maxLogMessages)
    	log.RemoveAt(0);
}

private var scrollPos:Vector2 = Vector2(0, 0);
private var lastLogLen:int = 0;
var printGUIStyle:GUIStyle;
var maxLogLabelHeight:float = 100.0f;
 
function OnGUI() {
	GUI.SetNextControlName ("chatWindow");
    userInput = GUI.TextField (Rect (0.0, Screen.height - 50, 200, 20), userInput, 25);
 
    if (textFieldSelected) {
    	GUI.FocusControl ("chatWindow");
    }
       
    var logBoxWidth:float = 180.0;
    var logBoxHeights:float[] = new float[log.length];
    var totalHeight:float = 0.0;
    var i:int = 0;
 
    for(var string:String in log) {
    	var logBoxHeight = Mathf.Min(maxLogLabelHeight, printGUIStyle.CalcHeight(GUIContent(string), logBoxWidth));
        logBoxHeights[i++] = logBoxHeight;
        totalHeight += logBoxHeight+10.0;
    }
 
    var innerScrollHeight:float = totalHeight;
 
    // if there's a new message, automatically scroll to bottom
    if(lastLogLen != log.length) {
    	scrollPos = Vector2(0.0, innerScrollHeight);
        lastLogLen = log.length;
    }
 
    scrollPos = GUI.BeginScrollView(Rect(0.0, Screen.height-150.0-50.0, 200, 150), scrollPos, Rect(0.0, 0.0, 180, innerScrollHeight));
 
    var currY:float = 0.0;
    i = 0;
 
    for(var string:String in log) {
    	logBoxHeight = logBoxHeights[i++];
		GUI.Label(Rect(10, currY, logBoxWidth, logBoxHeight), string, printGUIStyle);
		currY += (logBoxHeight+10.0);
	}
 
	GUI.EndScrollView();
        
}
 
function Update () {
	if(Input.GetKeyDown("return")) {
		textFieldSelected = true;
		
		if(userInput != "") {
			log.Add("" + userInput );
			userInput = "";
		}
	}
	if(Input.GetKeyDown(KeyCode.Escape)) {
		textFieldSelected = false;
	}
        
}


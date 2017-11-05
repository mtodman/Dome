//*** CHECK THIS ProgID ***
var X = new ActiveXObject("ASCOM.Matt's Dome Driver - 20130727.Dome");
WScript.Echo("This is " + X.Name + ")");
// You may want to uncomment this...
// X.Connected = true;
X.SetupDialog();

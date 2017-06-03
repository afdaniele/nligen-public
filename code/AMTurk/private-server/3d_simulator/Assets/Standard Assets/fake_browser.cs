using UnityEngine;
using System.Collections;

public class fake_browser{

	public static void ExternalCall(string function, string argument){
		switch (function) {
		case "refreshPage":
			{
				Debug.Log( "Browser: refreshPage signal received!" );
			}
			break;
		case "playerReady":
			{
				Debug.Log( "Browser: playerReady signal received!" );


				string plainJSONdata = @"{
					'mapName' : 'l',
					'initialX' : 23,
					'initialY' : 18,
					'initialTheta' : 90,
					'tutorial' : false
				}";

				string b64encodedJSONdata = browser_interface.Base64Encode (plainJSONdata);

				intro_control.loadExperimentDetails (b64encodedJSONdata);
			}
			break;
		case "showInstructions":
			{
				Debug.Log( "Browser: showInstructions signal received!" );
			}
			break;
		case "cancelButtonPressed":
			{
				Debug.Log( "Browser: cancelButtonPressed signal received!" );
			}
			break;
		case "finishButtonPressed":
			{
				Debug.Log( string.Format("Browser: finishButtonPressed signal received with arguments: '{0}'", argument ) );
			}
			break;
		}
	}

}



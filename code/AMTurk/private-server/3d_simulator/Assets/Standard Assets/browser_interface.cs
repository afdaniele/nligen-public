using UnityEngine;
using System.Collections;
using System;

public class browser_interface : MonoBehaviour {

	public static void OUT_refreshPage(){
		if (Settings.emulateBrowser) {
			fake_browser.ExternalCall ("refreshPage", "NULL");
		} else {
			Application.ExternalCall ("refreshPage", "NULL");
		}
	}

	public static void OUT_playerReady(){
		if (Settings.emulateBrowser) {
			fake_browser.ExternalCall( "playerReady", "NULL" );
		} else {
			Application.ExternalCall( "playerReady", "NULL" );
		}
	}

	public static void OUT_showInstructions(){
		if (Settings.emulateBrowser) {
			fake_browser.ExternalCall( "showInstructions", "NULL" );
		} else {
			Application.ExternalCall( "showInstructions", "NULL" );
		}
	}

	public static void OUT_cancelButtonPressed(){
		if (Settings.emulateBrowser) {
			fake_browser.ExternalCall( "cancelButtonPressed", "NULL" );
		} else {
			Application.ExternalCall( "cancelButtonPressed", "NULL" );
		}
	}

	public static void OUT_finishButtonPressed( string argument ){
		// encode argument
		string b64encodedArgument = Base64Encode( argument );
		//
		if (Settings.emulateBrowser) {
			fake_browser.ExternalCall( "finishButtonPressed", b64encodedArgument );
		} else {
			Application.ExternalCall( "finishButtonPressed", b64encodedArgument );
		}
	}




	// Utility methods
	public static string Base64Encode(string plainText) {
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		return System.Convert.ToBase64String(plainTextBytes);
	}

	public static string Base64Decode(string base64EncodedData) {
		var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
		return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
	}
		
}

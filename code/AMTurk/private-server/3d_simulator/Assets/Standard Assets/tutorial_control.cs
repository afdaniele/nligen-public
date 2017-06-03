using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;

public class tutorial_control : MonoBehaviour {

	public GameObject mainCharacter;

	public Text remainingTimeField;
	public GameObject tutorialCompletedDialog;
	public GameObject coordinatesPanel;
	public GameObject mapPanel;
	public GameObject tutorialTimePanel;
	public GameObject tutorialIntroDialog;
	public GameObject mapPointer;
	public Button startButton;
	public Button cancelButton;
	public Button finishButton;

	private float remainingTime;

	private const float map_x_offset = 33;
	private const float map_y_offset = -55;
	private const float map_x_width = 66;
	private const float map_y_height = 55;


	// Use this for initialization
	void Start () {
		// pause the game
		Settings.gamePaused = true;
		// set the remaining time
		remainingTime = Settings.tutorial_duration;
		// append listeners
		startButton.onClick.AddListener (delegate() { startButtonPressed(); });
		cancelButton.onClick.AddListener (delegate() { cancelButtonPressed(); });
		finishButton.onClick.AddListener (delegate() { finishButtonPressed(); });
	}

	
	// Update is called once per frame
	void Update () {
		if (Settings.gamePaused)
			return;
		//
		remainingTime -= Time.deltaTime;
		//
		if (remainingTime <= 0.0f) {
			// Tutorial Completed!
			// hide panels
			//TODO: coordinatesPanel.SetActive( false );
			mapPanel.SetActive( false );
			tutorialTimePanel.SetActive( false );

			// open dialog
			tutorialCompletedDialog.SetActive( true );

			// disable the 'walk' command
			Settings.verticalAxisDisabled = true;
		}
		if (remainingTime >= 0.0f) {
			remainingTimeField.text = string.Format ("{0} sec", remainingTime.ToString ("F1"));
		} else {
			remainingTimeField.text = "please wait...";
		}
		//
		if( mainCharacter != null ){
			Vector3 agentPosition = game_control.getAgentPose( ref mainCharacter );
			//
			float x = map_x_offset - agentPosition.x * map_x_width;
			float y = map_y_offset + agentPosition.y * map_y_height;
			float theta = agentPosition.z;
			//
			mapPointer.transform.localPosition = new Vector3( x, y, mapPointer.transform.position.z );
			mapPointer.transform.localRotation = Quaternion.Euler( new Vector3( 0.0f, 180.0f, theta ) );
		}

	}


	void startButtonPressed(){
		// close dialog
		tutorialIntroDialog.SetActive( false );
		// show panels
		//TODO: coordinatesPanel.SetActive( true );
		mapPanel.SetActive( true );
		tutorialTimePanel.SetActive( true );
		// start training
		Settings.mode = "tutorial";
		// resume the game
		Settings.gamePaused = false;
	}


	void cancelButtonPressed(){
		// the user decided to continue training
		// close dialog
		tutorialCompletedDialog.SetActive( false );
		// show panels
		//TODO: coordinatesPanel.SetActive( true );
		mapPanel.SetActive( true );
		tutorialTimePanel.SetActive( true );
		// add 'Settings.tutorial_duration' seconds of training
		remainingTime = Settings.tutorial_duration;
		// restore the value of Settings.verticalAxisDisabled
		Settings.resetCharacterCommands();
	}

	void finishButtonPressed(){
		if (Settings.experiment != null) {
			// experiment ready!
			Settings.mode = "task";
			// ask the browser to show the instructions
			browser_interface.OUT_showInstructions();
			// load scene
			SceneManager.LoadScene( "main" );
		} else {
			// avoid infinite wait
			browser_interface.OUT_refreshPage();
		}
	}

}

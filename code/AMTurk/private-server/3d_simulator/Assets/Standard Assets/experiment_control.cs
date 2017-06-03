using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

public class experiment_control : MonoBehaviour {

	public GameObject mainCharacter;

	public GameObject taskIntroDialog;
	public Button startButton;

	public GameObject taskInfoPanel;
	public Button taskConfirmButton;
	public Button taskRejectButton;

	public GameObject taskConfirmDialog;
	public Button taskConfirmYesButton;
	public Button taskConfirmNoButton;

	public GameObject taskRejectDialog;
	public Button taskRejectYesButton;
	public Button taskRejectNoButton;

	private float lastTheta = -1.0f;

	static float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
		// pause the game
		Settings.gamePaused = true;
		// append listeners
		startButton.onClick.AddListener(delegate() { startButtonPressed(); });
		taskConfirmButton.onClick.AddListener (delegate() { taskConfirm(); });
		taskRejectButton.onClick.AddListener (delegate() { taskReject(); });
		taskConfirmYesButton.onClick.AddListener (delegate() { taskConfirmYes(); });
		taskConfirmNoButton.onClick.AddListener (delegate() { taskConfirmNo(); });
		taskRejectYesButton.onClick.AddListener (delegate() { taskRejectYes(); });
		taskRejectNoButton.onClick.AddListener (delegate() { taskRejectNo(); });
	}
	
	// Update is called once per frame
	void Update () {
		if (Settings.gamePaused)
			return;
		// add checkpoint for re-orientation
		if (!Settings.verticalAxisLocked) { // the agent is not walking
			Vector3 agentPose = game_control.getAgentPose (ref mainCharacter);
			// compute the smallest difference
			float angle_diff = (lastTheta == -1.0f)? 360.0f : Mathf.Abs ((agentPose.z - lastTheta + 180.0f) % 360.0f - 180.0f);
			// add checkpoint if >= threshold
			if (angle_diff >= Settings.add_checkpoint_per_reorientation_each_deg) {
				addCheckpoint (agentPose);
				lastTheta = agentPose.z;
			}
		}
		//
		elapsedTime += Time.deltaTime;
	}


	public static void addCheckpoint( Vector3 agentPose ){
		if (Settings.experiment == null)
			return;

		// get X
		float x = float.Parse( agentPose.x.ToString("F1") );

		// get Y
		float y = float.Parse( agentPose.y.ToString("F1") );

		// get Theta
		float theta = float.Parse( agentPose.z.ToString("F1") );

		// build checkpoint
		Checkpoint cp = new Checkpoint (elapsedTime, x, y, theta);

		// store checkpoint
		Settings.experiment.checkpoints.Add( cp );

		// log
		Debug.Log( string.Format("New checkpoint added! ({0}, {1}, {2})", x.ToString("F1"), y.ToString("F1"), theta.ToString("F1")) );
	}




	void startButtonPressed(){
		// close dialog
		taskIntroDialog.SetActive( false );
		// show panels
		taskInfoPanel.SetActive( true );
		// start
		Settings.mode = "task";
		// resume the game
		Settings.gamePaused = false;
	}


	void taskReject(){
		// pause the game
		Settings.gamePaused = true;
		Settings.controlsDisabled = true;
		// hide the info panel
		taskInfoPanel.SetActive( false );
		// show dialog
		taskRejectDialog.SetActive( true );
	}

	void taskRejectYes(){
		browser_interface.OUT_cancelButtonPressed ();
	}

	void taskRejectNo(){
		// hide dialog
		taskRejectDialog.SetActive( false );
		// show the info panel
		taskInfoPanel.SetActive( true );
		// resume the game
		Settings.gamePaused = false;
		Settings.controlsDisabled = false;
	}




	void taskConfirm(){
		// pause the game
		Settings.gamePaused = true;
		Settings.controlsDisabled = true;
		// hide the info panel
		taskInfoPanel.SetActive( false );
		// show dialog
		taskConfirmDialog.SetActive( true );
	}

	void taskConfirmYes(){
		// pause the game
		Settings.gamePaused = true;
		Settings.controlsDisabled = true;
		Settings.horizontalAxisDisabled = true;
		Settings.verticalAxisDisabled = true;
		// convert the experiment into a JSON string
		string json = JsonConvert.SerializeObject( Settings.experiment );
		//
		browser_interface.OUT_finishButtonPressed ( json );
	}

	void taskConfirmNo(){
		// hide dialog
		taskConfirmDialog.SetActive( false );
		// show the info panel
		taskInfoPanel.SetActive( true );
		// resume the game
		Settings.gamePaused = false;
		Settings.controlsDisabled = false;
	}

}

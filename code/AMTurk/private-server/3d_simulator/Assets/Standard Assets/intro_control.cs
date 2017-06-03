using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class intro_control : MonoBehaviour {

	public static float loading_icon_rotations_per_second = 1.0f;

	public GameObject loadingIcon;



	// Use this for initialization
	void Start () {
		StartCoroutine (ExecuteAfterTime (2.0f));
	}

	
	// Update is called once per frame
	void Update () {
		float rotation_step = - ( 360.0f / loading_icon_rotations_per_second ) * Time.deltaTime; // deg / sec * sec
		//
		loadingIcon.transform.Rotate( 0.0f, 0.0f, rotation_step );
	}


	public static void loadExperimentDetails( string b64encodedJSONdata ){
		// decode data
		string decodedJSONdata = browser_interface.Base64Decode (b64encodedJSONdata);
		// convert the JSON
		Experiment exp = JsonConvert.DeserializeObject<Experiment>(decodedJSONdata);
		// store experiment
		Settings.experiment = exp;
		// create list of checkpoints
		Settings.experiment.checkpoints = new List<Checkpoint>();
		// append initial position as checkpoint
		Settings.experiment.checkpoints.Add( new Checkpoint( 0.0f, (float)exp.initialX, (float)exp.initialY, (float)exp.initialTheta ) );
		// set the presence of the labels
		Settings.drawLabels = Settings.experiment.labels;
		// decide wether to launch the tutorial or not
		if (Settings.experiment.tutorial) {
			// experiment ready!
			Settings.mode = "tutorial";
			// load scene
			SceneManager.LoadScene( "tutorial" );
		} else {
			// experiment ready!
			Settings.mode = "task";
			// ask the browser to show the instructions
			browser_interface.OUT_showInstructions();
			// load scene
			SceneManager.LoadScene( "main" );
		}
	}



	IEnumerator ExecuteAfterTime( float time ){
		yield return new WaitForSeconds( time );
		// tell the web page we are ready to receive the information about the experiment!
		browser_interface.OUT_playerReady ();
	}

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class game_control : MonoBehaviour {

	public Text theta_field;
	public Text x_field;
	public Text y_field;
	public Text fpsCounter;
	public GameObject mainCharacter;

	const float measurePeriod = 0.2f;
	private float nextPeriod = 0;
	private int m_FpsAccumulator = 0;
	private int m_CurrentFps;
	public static float magnetic_point_sensitivity = 0.12f;
	//
	const string theta_field_format = "A: {0}";
	const string x_field_format = "X: {0}";
	const string y_field_format = "Y: {0}";
	const string fps_field_format = "{0} FPS";



	private void Start(){
		nextPeriod = Time.realtimeSinceStartup + measurePeriod;
	}


	public Vector3 getAgentPose(){
		return game_control.getAgentPose ( ref mainCharacter );
	}

	public static Vector3 getAgentPose( ref GameObject agent ){
		// get character position and orientation
		Vector3 position = agent.transform.position;
		Vector3 orientation = agent.transform.rotation.eulerAngles;


		// compute Theta
		float theta = float.Parse( orientation.y.ToString("F1") );
		// compute X
		float x = float.Parse( ( position.x / Settings.dimension_ratio ).ToString("F2") );
		// compute Y
		float y = float.Parse( ( position.z / Settings.dimension_ratio ).ToString("F2") );


		// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
		//
		// compute Theta
		theta = float.Parse( ( ( orientation.y + 180.0f ) % 360.0f ).ToString("F1") );
		// compute X
		x = float.Parse( ( Settings.map_maxX - ( position.x / Settings.dimension_ratio ) ).ToString("F2") );
		// compute Y
		y = float.Parse( ( Settings.map_minY + ( position.z / Settings.dimension_ratio ) ).ToString("F2") );
		//
		// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps


		// magnetic points
		if (Mathf.Abs (x - (float)((int)x)) < magnetic_point_sensitivity) {
			x = (float)((int)x);
		}
		if (Mathf.Abs (x - (float)((int)x)) > 1.0f-magnetic_point_sensitivity) {
			x = (float)((int)x) + 1.0f;
		}
		if (Mathf.Abs (y - (float)((int)y)) < magnetic_point_sensitivity) {
			y = (float)((int)y);
		}
		if (Mathf.Abs (y - (float)((int)y)) > 1.0f-magnetic_point_sensitivity) {
			y = (float)((int)y) + 1.0f;
		}

		// return pose
		return new Vector3 (x, y, theta);
	}


	private void Update(){
		Settings.gameTime += Time.deltaTime;

		if (mainCharacter == null) { return; }

		m_FpsAccumulator++;
		// measure average frames per second
		if (Time.realtimeSinceStartup > nextPeriod){
			
			Vector3 agentPose = getAgentPose ();

			// print Theta
			theta_field.text = string.Format( theta_field_format, agentPose.z.ToString("F1") );

			// print X
			x_field.text = string.Format( x_field_format, agentPose.x.ToString("F1") );

			// print Y
			y_field.text = string.Format( y_field_format, agentPose.y.ToString("F1") );


			// measure average frames per second
			m_CurrentFps = (int) (m_FpsAccumulator/measurePeriod);
			m_FpsAccumulator = 0;
			fpsCounter.text = string.Format(fps_field_format, m_CurrentFps);

			// increase counter
			nextPeriod += measurePeriod;

		}
	}

}

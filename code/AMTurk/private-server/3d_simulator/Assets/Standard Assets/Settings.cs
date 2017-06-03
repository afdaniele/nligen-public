using UnityEngine;
using System.Collections;

public class Settings {

	public static bool emulateBrowser = true;

	public const float tutorial_duration = 30.0f;  // in seconds

	public const float dimension_ratio = 10.0f;
	public const float hallway_width_ratio = 0.3f;
	public const float hallway_height = 4.0f;

	public const float add_checkpoint_per_reorientation_each_deg = 45.0f; // 8 discrete values

	public static string mode = null; // possible values: { 'tutorial', 'task' }

	public static bool drawLabels = false;

	public static Experiment experiment;

	public static Map map;

	public static float map_maxX = 0.0f;
	public static float map_minY = 0.0f;

	public static bool verticalAxisLocked = false;

	public static bool horizontalAxisCorrection = false;

	public static bool verticalAxisDisabled = false;
	public static bool horizontalAxisDisabled = false;

	public static bool controlsDisabled = false;

	public static bool gamePaused = false;

	public static float gameTime = 0.0f;



	public static void resetCharacterCommands(){
		verticalAxisLocked = false;
		verticalAxisDisabled = false;
		horizontalAxisDisabled = false;
		controlsDisabled = false;
	}

}

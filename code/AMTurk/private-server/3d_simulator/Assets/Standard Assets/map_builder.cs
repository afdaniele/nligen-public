using System;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

namespace UnityStandardAssets.Utility{
	
	public class map_builder : MonoBehaviour{

		private float dimension_ratio = Settings.dimension_ratio;
		private float hallway_width_ratio = Settings.hallway_width_ratio;
		private float hallway_height = Settings.hallway_height;

		public GameObject errorPanel;
		public GameObject infoPanel;
		private float refreshPageCounter = 1.0f;

		public GameObject chair;
		public GameObject barstool;
		public GameObject sofa;
		public GameObject hatrack;
		public GameObject easel;
		public GameObject lamp;

		public GameObject hallway_label;

		public GameObject mainCharacter;




		private void Update(){
			if( Settings.experiment == null && Settings.gameTime > refreshPageCounter*10.0f){
				// reload page
				browser_interface.OUT_refreshPage ();
				// increase the counter
				refreshPageCounter += 1.0f;
			}
		}

		private void Start(){
			// show error if the experiment is not available
			if( (Settings.mode == null) || (Settings.mode == "task" && Settings.experiment == null) ){
				// disable the Character controllers
				Settings.controlsDisabled = true;
				// hide info panel
				infoPanel.SetActive (false);
				// show error panel
				errorPanel.SetActive (true);
				// close
				return;
			}

			// get map name
			string mapName = "tutorial";
			if (Settings.mode == "task") {
				mapName = Settings.experiment.mapName;
			}

			// auto-computed properties
			float hallway_length_ratio = 1.0f - hallway_width_ratio;
			float hallway_width = dimension_ratio * hallway_width_ratio;
			float hallway_length = dimension_ratio * hallway_length_ratio;
			float hallway_middle_point = dimension_ratio / 2.0f;
			float wall_y_midpoint = hallway_height / 2.0f;
			float wall_height_ratio = hallway_height / dimension_ratio;
			float hallway_half_width_ratio = (hallway_width_ratio / 2.0f);
			float hallway_half_width = dimension_ratio * hallway_half_width_ratio;

			// load Map
			Map map = Map.loadMap ( mapName );

			// store the map
			Settings.map = map;

			// Compute maxX and minY
			float maxX = -1000.0f;
			float minY = 1000.0f;
			foreach (Intersection n in map.nodes) {
				if (n.x > maxX){
					maxX = (float)n.x;
				}
				if (n.y < minY){
					minY = (float)n.y;
				}
			}

			// make them public
			Settings.map_maxX = maxX;
			Settings.map_minY = minY;

			// draw Intersections
			GameObject plane;
			Renderer renderer;
			foreach (Intersection n in map.nodes) {
				float x = (float)n.x;
				float y = (float)n.y;
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				x = (float)Math.Abs( (float)n.x - maxX );
				y = (float)n.y - minY;
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				// Create planes
				//
				// BOTTOM plane
				plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
				plane.transform.position = new Vector3(x*dimension_ratio, 0.0f, y*dimension_ratio);
				plane.transform.localScale = new Vector3(hallway_width_ratio, 1.0f, hallway_width_ratio);
				// apply Material
				renderer = plane.GetComponent<Renderer> ();
				renderer.material = Resources.Load("default_floor") as Material;
				renderer.material.shader = Shader.Find("Standard");
				// TOP plane
				plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
				plane.transform.position = new Vector3(x*dimension_ratio, hallway_height, y*dimension_ratio);
				plane.transform.localScale = new Vector3(hallway_width_ratio, 1.0f, hallway_width_ratio);
				plane.transform.Rotate (new Vector3 (180.0f, 0.0f, 0.0f));
				// apply Material
				renderer = plane.GetComponent<Renderer> ();
				renderer.material = Resources.Load("default_wall") as Material;
				renderer.material.shader = Shader.Find("Standard");
				// Define which wall to draw
				bool s0_draw = true;
				bool s90_draw = true;
				bool s180_draw = true;
				bool s270_draw = true;
				foreach (Hallway h in map.hallways) {
					Intersection current = n;
					Intersection node1 = h.getNode1 ();
					Intersection node2 = h.getNode2 ();
					// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
					current = new Intersection( (int)Math.Abs( n.x - (int)maxX ), n.y - (int)minY, n.item );
					node1 = new Intersection ( (int)Math.Abs( node1.x - (int)maxX ), node1.y - (int)minY, "" );
					node2 = new Intersection ( (int)Math.Abs( node2.x - (int)maxX ), node2.y - (int)minY, "" );
					// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
					// if the node 'n' is represented by 'node1'
					if ( (current.x == node1.x) && (current.y == node1.y) ) {
						if (current.x != node2.x){
							if (current.x < node2.x) {
								s270_draw = false;
							} else {
								s90_draw = false;
							}
						}
						if (current.y != node2.y){
							if (current.y < node2.y) {
								s180_draw = false;
							} else {
								s0_draw = false;
							}
						} 
					}
					// if the node 'n' is represented by 'node2'
					if ( (current.x == node2.x) && (current.y == node2.y) ) {
						if (current.x != node1.x){
							if (current.x < node1.x) {
								s270_draw = false;
							} else {
								s90_draw = false;
							}
						}
						if (current.y != node1.y){
							if (current.y < node1.y) {
								s180_draw = false;
							} else {
								s0_draw = false;
							}
						} 
					}
				}

				// S0 plane
				if (s0_draw) {
					// draw Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, wall_y_midpoint, y * dimension_ratio - hallway_half_width);
					plane.transform.localScale = new Vector3 (hallway_width_ratio, 1.0f, wall_height_ratio);
					plane.transform.Rotate (new Vector3 (90.0f, 0.0f, 0.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("default_wall") as Material;
					renderer.material.shader = Shader.Find("Standard");
				}
				// S270 plane
				if (s270_draw) {
					plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
					plane.transform.position = new Vector3(x*dimension_ratio + hallway_half_width, wall_y_midpoint, y*dimension_ratio);
					plane.transform.localScale = new Vector3(wall_height_ratio, 1.0f, hallway_width_ratio);
					plane.transform.Rotate (new Vector3 (0.0f, 0.0f, 90.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("default_wall") as Material;
					renderer.material.shader = Shader.Find("Standard");
				}
				// S180 plane
				if (s180_draw) {
					plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
					plane.transform.position = new Vector3(x*dimension_ratio, wall_y_midpoint, y*dimension_ratio + hallway_half_width);
					plane.transform.localScale = new Vector3(hallway_width_ratio, 1.0f, wall_height_ratio);
					plane.transform.Rotate (new Vector3 (-90.0f, 0.0f, 0.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("default_wall") as Material;
					renderer.material.shader = Shader.Find("Standard");
				}
				// S90 plane
				if (s90_draw) {
					plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
					plane.transform.position = new Vector3(x*dimension_ratio - hallway_half_width, wall_y_midpoint, y*dimension_ratio);
					plane.transform.localScale = new Vector3(wall_height_ratio, 1.0f, hallway_width_ratio);
					plane.transform.Rotate (new Vector3 (0.0f, 0.0f, -90.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("default_wall") as Material;
					renderer.material.shader = Shader.Find("Standard");
				}


				if (n.item != "") {
					Vector3 object_position = new Vector3 (x, 0.0f, y);

					GameObject obj = null;
					switch (n.item) {
					case "chair":
						obj = chair;
						break;
					case "easel":
						obj = easel;
						break;
					case "barstool":
						obj = barstool;
						break;
					case "sofa":
						obj = sofa;
						break;
					case "hatrack":
						obj = hatrack;
						break;
					case "lamp":
						obj = lamp;
						break;
					}

					// orient the object according to the presence of the walls
					Vector3 object_rotation = new Vector3();
					if (s0_draw) {
						object_rotation = new Vector3 (0.0f, 0.0f, 0.0f);
					}
					if (s270_draw) {
						object_rotation = new Vector3 (0.0f, -90.0f, 0.0f);
					}
					if (s180_draw) {
						object_rotation = new Vector3 (0.0f, 180.0f, 0.0f);
					}
					if (s90_draw) {
						object_rotation = new Vector3 (0.0f, 90.0f, 0.0f);
					}

					object_position *= dimension_ratio;

					GameObject clone = Instantiate (obj, object_position, new Quaternion()) as GameObject;
					clone.transform.Rotate (object_rotation);

				}

			}


			foreach (Hallway h in map.hallways) {
				Intersection node1 = h.getNode1 ();
				Intersection node2 = h.getNode2 ();
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				node1 = new Intersection ((int)Math.Abs (node1.x - (int)maxX), node1.y - (int)minY, "");
				node2 = new Intersection ((int)Math.Abs (node2.x - (int)maxX), node2.y - (int)minY, "");
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				// compute X and Y
				float x = ( (float)node1.x + (float)node2.x ) / 2.0f;
				float y = ( (float)node1.y + (float)node2.y ) / 2.0f;
				// labels positions / rotations
				Vector3[] labels_positions = new Vector3[2];
				Vector3[] labels_rotations = new Vector3[2];
				// if it is an horizontal hallway (along-X)
				if( node1.x != node2.x ){
					// S0 Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, wall_y_midpoint, y * dimension_ratio - hallway_half_width);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, wall_height_ratio);
					plane.transform.Rotate (new Vector3 (90.0f, 0.0f, 0.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("wall_"+h.wall) as Material;
					renderer.material.shader = Shader.Find("Standard");
					// S180 Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, wall_y_midpoint, y * dimension_ratio + hallway_half_width);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, wall_height_ratio);
					plane.transform.Rotate (new Vector3 (90.0f, 0.0f, 0.0f));
					plane.transform.Rotate (new Vector3 (0.0f, 0.0f, 180.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("wall_"+h.wall) as Material;
					renderer.material.shader = Shader.Find("Standard");
					// TOP Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, hallway_height, y * dimension_ratio);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, hallway_width_ratio);
					plane.transform.Rotate (new Vector3 (180.0f, 0.0f, 0.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("default_wall") as Material;
					renderer.material.shader = Shader.Find("Standard");
					// BOTTOM Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, 0.0f, y * dimension_ratio);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, hallway_width_ratio);
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("floor_"+h.floor) as Material;
					renderer.material.shader = Shader.Find("Standard");
					// label position
					labels_positions[0] = new Vector3 (x * dimension_ratio, 0.0f, y * dimension_ratio - hallway_half_width);
					labels_rotations[0] = new Vector3 (0.0f, 270.0f, 0.0f);
					labels_positions[1] = new Vector3 (x * dimension_ratio, 0.0f, y * dimension_ratio + hallway_half_width);
					labels_rotations[1] = new Vector3 (0.0f, 90.0f, 0.0f);
				}
				// if it is a vertical hallway (along-Z)
				if( node1.y != node2.y ){
					// S270 Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio + hallway_half_width, wall_y_midpoint, y * dimension_ratio);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, wall_height_ratio);
					plane.transform.Rotate (new Vector3 (0.0f, 90.0f, 0.0f));
					plane.transform.Rotate (new Vector3 (90.0f, 0.0f, 0.0f));
					plane.transform.Rotate (new Vector3 (0.0f, 0.0f, 180.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("wall_"+h.wall) as Material;
					renderer.material.shader = Shader.Find("Standard");
					// S90 Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio - hallway_half_width, wall_y_midpoint, y * dimension_ratio);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, wall_height_ratio);
					plane.transform.Rotate (new Vector3 (0.0f, 90.0f, 0.0f));
					plane.transform.Rotate (new Vector3 (90.0f, 0.0f, 0.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("wall_"+h.wall) as Material;
					renderer.material.shader = Shader.Find("Standard");
					// TOP Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, hallway_height, y * dimension_ratio);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, hallway_width_ratio);
					plane.transform.Rotate (new Vector3 (180.0f, 0.0f, 0.0f));
					plane.transform.Rotate (new Vector3 (0.0f, 90.0f, 0.0f));
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("default_wall") as Material;
					renderer.material.shader = Shader.Find("Standard");
					// BOTTOM Plane
					plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
					plane.transform.position = new Vector3 (x * dimension_ratio, 0.0f, y * dimension_ratio);
					plane.transform.localScale = new Vector3 (hallway_length_ratio, 1.0f, hallway_width_ratio);
					// apply Material
					renderer = plane.GetComponent<Renderer> ();
					renderer.material = Resources.Load("floor_"+h.floor) as Material;
					renderer.material.shader = Shader.Find("Standard");
					plane.transform.Rotate (new Vector3 (0.0f, 90.0f, 0.0f));
					// label position
					labels_positions[0] = new Vector3 (x * dimension_ratio - hallway_half_width, 0.0f, y * dimension_ratio);
					labels_rotations[0] = new Vector3 (0.0f, 0.0f, 0.0f);
					labels_positions[1] = new Vector3 (x * dimension_ratio + hallway_half_width, 0.0f, y * dimension_ratio);
					labels_rotations[1] = new Vector3 (0.0f, 180.0f, 0.0f);
				}


				// draw labels (if requested)
				if (Settings.drawLabels) {
					for (int i = 0; i < 2; i++) {
						GameObject clone = Instantiate (hallway_label, labels_positions [i], new Quaternion ()) as GameObject;
						clone.transform.Rotate (labels_rotations [i]);
						// change label
						TextMesh[] children = clone.GetComponentsInChildren<TextMesh> ();
						foreach (TextMesh child in children) {
							child.text = h.floor;
						}
					}
				}

			}


			// default: position/orientation assigned in the Editor
			Vector3 initialPosition = mainCharacter.transform.position;
			Vector3 rotation = new Vector3 (0.0f, 0.0f, 0.0f);

			if (Settings.mode == "task") {
				// read 'initialX'
				float initialX = (float)Settings.experiment.initialX;
				//
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				initialX = (float)Math.Abs (initialX - maxX);
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				//
				initialPosition.x = initialX;

				// read 'initialY'
				float initialY = (float)Settings.experiment.initialY;
				//
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				initialY = initialY - minY;
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				//
				initialPosition.z = initialY;

				// read 'initialTheta'
				float initialTheta = (float)Settings.experiment.initialTheta;
				//
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				initialTheta = (initialTheta - 180.0f) % 360.0f;
				// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
				//
				rotation.y = initialTheta;

				// scale the position according to 'dimension_ratio'
				float mainCharacterHeight = initialPosition.y;
				initialPosition *= Settings.dimension_ratio;
				initialPosition.y = mainCharacterHeight;
			}
				
			// assign initial position / orientation
			mainCharacter.transform.position = initialPosition;
			mainCharacter.transform.eulerAngles = rotation;

			// hide labels (if requested)
			if( !Settings.drawLabels ){
				Camera camera = mainCharacter.transform.FindChild("FPSController").transform.FindChild("FirstPersonCharacter").GetComponent<Camera>();
				camera.cullingMask &= ~(1 << LayerMask.NameToLayer("ObjectLabels"));
			}

			// enable the Character
			Settings.resetCharacterCommands();
		}


	}

}

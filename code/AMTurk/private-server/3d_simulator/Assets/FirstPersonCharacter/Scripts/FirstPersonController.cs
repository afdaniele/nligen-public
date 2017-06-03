using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson{
	
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]

    public class FirstPersonController : MonoBehaviour{

        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
		[SerializeField] GameObject mainCharacter;

        private Camera m_Camera;
		private float m_StepInterval;

        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private AudioSource m_AudioSource;

		private Vector3 previousPosition;
		private Vector3 tempLastPosition;

		private bool horizontalAxisCorrectionEnabled;

        // Use this for initialization
        private void Start(){
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
			m_StepInterval = m_WalkSpeed / 1.2f;

            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);

			previousPosition = mainCharacter.transform.position;

			horizontalAxisCorrectionEnabled = false;
        }


        // Update is called once per frame
        private void Update(){
			if (Settings.gamePaused)
				return;

			if (!Settings.horizontalAxisDisabled && !Settings.controlsDisabled) {
				RotateView ();
			}

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded){
                m_MoveDir.y = 0f;
            }

            if (!m_CharacterController.isGrounded && m_PreviouslyGrounded){
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }

        private void FixedUpdate(){
			if (Settings.gamePaused)
				return;
			
			if (!Settings.verticalAxisLocked && (Settings.verticalAxisDisabled || Settings.controlsDisabled)) {
				return;
			}


            float speed;
            GetInput(out speed);

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


			m_MoveDir.y = -1.0f; // Stick To Ground Force;

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void ProgressStepCycle(float speed){
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0)){
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed))*Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep)){
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio(){
            if (!m_CharacterController.isGrounded){
                return;
            }
            // pick & play a random footstep sound from the array, excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed){
            Vector3 newCameraPosition;

            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded){
                newCameraPosition = m_Camera.transform.localPosition;
            }else{
                newCameraPosition = m_Camera.transform.localPosition;
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed){
			// if the agent is walking
			if (Settings.verticalAxisLocked) {
				speed = m_WalkSpeed;
				// get the agent pose
				Vector3 agentPose = game_control.getAgentPose ( ref mainCharacter );
				// security check, if no movement in one step, stop it
				if ( tempLastPosition != null && (mainCharacter.transform.position - tempLastPosition).sqrMagnitude < 0.01f) {
					previousPosition = new Vector3 ( -1.0f, 0.0f, -1.0f );
				} else {
					tempLastPosition = mainCharacter.transform.position;
				}
				// compute the distance between starting position and current position
				Vector3 diff = mainCharacter.transform.position - previousPosition;
				if (diff.magnitude > 0.9f * Settings.dimension_ratio) {
					if ((agentPose.x + agentPose.y ) % 1.0f == 0.0f) {
						// compute position vector
						float x = agentPose.x;
						float y = agentPose.y;

						// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
						x = (float)Math.Abs( x - Settings.map_maxX );
						y = y - Settings.map_minY;
						// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps

						// compute final position
						Vector3 position = new Vector3 (x, 0.0f, y) * Settings.dimension_ratio;
						mainCharacter.transform.position += (position - mainCharacter.transform.position)/2.0f;

						// re-enable commands and update previousPosition
						Settings.verticalAxisLocked = false;
						previousPosition = game_control.getAgentPose ( ref mainCharacter );

						// the next movement will request a orientation correction
						horizontalAxisCorrectionEnabled = false;

						// notify the experiment_controller the new checkpoint (if needed)
						if(Settings.mode == "task"){
							experiment_control.addCheckpoint (previousPosition);
						}

					}
				}
				return; // keep going
			}

			float horizontal = 0.0f; // disable lateral motion capabilities
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

			if (vertical < 0.0f){
				// no backward movements
				speed = 0.0f;
				return; // do nothing
			}

			// set the desired speed to 0
			speed = 0.0f;
            // set the Input vector
            m_Input = new Vector2(horizontal, vertical);

			if (m_Input.sqrMagnitude != 0.0f){
				// input catched
				// 1. request orientation correction (if needed)
				if ( !horizontalAxisCorrectionEnabled ) {
					horizontalAxisCorrectionEnabled = true;
					// ask the orientation controller to correct the orientation
					Settings.horizontalAxisCorrection = true;
					return;
				}
			}

			// 2. start moving
			if (horizontalAxisCorrectionEnabled) {
				// correction requested during the previous time steps, check if completed
				if (Settings.horizontalAxisCorrection) {
					// still correcting, do nothing
					return;
				} else {
					// correction completed, now you can walk
					// set the desired speed to be walking or running
					speed = m_WalkSpeed;
					m_Input = new Vector2(horizontal, 1.0f);
					// activate autonomous walking
					Settings.verticalAxisLocked = true;
					previousPosition = mainCharacter.transform.position;
				}
			}

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1){
                m_Input.Normalize();
            }

        }


        private void RotateView(){
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit){
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below){
                return;
            }

            if (body == null || body.isKinematic){
                return;
            }

            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}

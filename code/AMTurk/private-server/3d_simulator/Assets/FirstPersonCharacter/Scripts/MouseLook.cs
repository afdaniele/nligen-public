using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook{
		
        public float XSensitivity = 2f;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

		private float orientationCorrectionThreshold = 2.0f;

		private float[] discreteTheta = new float[5]{ 0.0f, 90.0f, 180.0f, 270.0f, 0.0f };

        public void Init(Transform character, Transform camera){
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera){
			float agentTheta = camera.rotation.eulerAngles.y;

			// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps
			agentTheta = float.Parse( ( ( agentTheta + 180.0f ) % 360.0f ).ToString("F1") );
			// TODO: this block corrects X-reflection and XY-displacement on the MARCO maps

			float yRot = 0.0f;
	
			if (Settings.horizontalAxisCorrection) {
				
				// a orientation correction is required
				yRot = 1.0f;
				// find the target theta
				int closest_theta_index = (int)Mathf.Round( agentTheta / 90.0f );
				float desiredTheta = discreteTheta[ closest_theta_index ];

				// correct orientation
				float diff = (agentTheta - desiredTheta + 180.0f) % 360.0f - 180.0f;

				if( diff > 0.0f ){
					yRot *= -1.0f;
				}

				// finally
				if( Mathf.Abs(diff) < orientationCorrectionThreshold ){
					// notify correction completed
					Settings.horizontalAxisCorrection = false;
				}

			} else {
				// the avatar is waiting or walking
				if (Settings.verticalAxisLocked) {
					// it is walking
					yRot = 0.0f; // do not allow the avatar to turn
				} else {
					// it is waiting


					yRot = CrossPlatformInputManager.GetAxis("Horizontal") * XSensitivity;


				}
			}

			

			float xRot = 0.0f; // disable vertical rotation

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(smooth){
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }else{
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }

        public void SetCursorLock(bool value){
            lockCursor = value;
            if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock(){
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate(){
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q){
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}

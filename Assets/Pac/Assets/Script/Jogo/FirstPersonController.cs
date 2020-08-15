using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent(typeof (CharacterController))]
	[RequireComponent(typeof (AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField] private bool m_IsWalking;
		[SerializeField] private float m_WalkSpeed;
		[SerializeField] private float m_RunSpeed;
		[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
		[SerializeField] private float m_JumpSpeed;
		[SerializeField] private float m_StickToGroundForce;
		[SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
		[SerializeField] private bool m_UseFovKick;
		[SerializeField] private FOVKick m_FovKick = new FOVKick();
		[SerializeField] private bool m_UseHeadBob;
		[SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
		[SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
		[SerializeField] private float m_StepInterval;
		[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
		[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

		private Camera m_Camera;
		private bool m_Jump;
		//Added by aaron;
		private bool vr_Jump;
		private bool vr_Jumping;

		private float m_YRotation;
		private Vector2 m_Input;
		private Vector3 m_MoveDir = Vector3.zero;
		private CharacterController m_CharacterController;
		private CollisionFlags m_CollisionFlags;
		private bool m_PreviouslyGrounded;
		private Vector3 m_OriginalCameraPosition;
		private float m_StepCycle;
		private float m_NextStep;
		private bool m_Jumping;
		private AudioSource m_AudioSource;
		public GameObject head;

		private float _moveDirection;   // 1 or -1
		private Gyroscope vr_Gyro;
		private float vr_GyroAccYAxis = 0;

		public int tmp_count;
		public int tmp_device;

		private Quaternion initialRotation, gyroInitialRotation;

		private bool gyroEnabled;
		private Gyroscope gyro;
		private GameObject GyroControl;
		private Quaternion rot;



		// Use this for initialization
		private void Start()
		{
			m_CharacterController = GetComponent<CharacterController>();
			m_Camera = Camera.main;
			m_OriginalCameraPosition = m_Camera.transform.localPosition;
			m_FovKick.Setup(m_Camera);
			m_HeadBob.Setup(m_Camera, m_StepInterval);
			m_StepCycle = 0f;
			m_NextStep = m_StepCycle/2f;
			m_Jumping = false;
			m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
			_moveDirection = 1;

			tmp_count = 0;
			tmp_device = 0;

			GyroControl = new GameObject ("Gyro Control");
			GyroControl.transform.position = transform.position;
			transform.SetParent (GyroControl.transform);
			gyroEnabled = EnableGyro();
		}

		private bool EnableGyro()
		{
			if (SystemInfo.supportsGyroscope) 
			{
				gyro = Input.gyro;
				gyro.enabled = true;
				GyroControl.transform.rotation = Quaternion.Euler(90f, -90f, 0f);
				rot = new Quaternion(0, 0, 1, 0);
				return true;
			}
			return false;
		}


		// Update is called once per frame
		private void Update()
		{
			RotateView();
			//Debug.Log("Update time :" + Time.deltaTime);
			// the jump state needs to read here to make sure it is not missed // comment by aaron, yes, quite right, I missed when putting it in FixedUpdate

			if (gyroEnabled) 
			{
				transform.localRotation = gyro.attitude * rot;
			}

			if (!m_Jump)
			{
				vr_Gyro = Input.gyro;
				vr_GyroAccYAxis = vr_Gyro.userAcceleration.y;
				if (vr_GyroAccYAxis > 2 && vr_GyroAccYAxis <= 3) {
					print ("jump in update over 2 acc is" + vr_GyroAccYAxis);	
				} else if (vr_GyroAccYAxis >= 1 && vr_GyroAccYAxis <= 2) {
					print ("jump in update over 1 acc is" + vr_GyroAccYAxis);	
					m_Jump = true;
				} else if (vr_GyroAccYAxis >= 0.1 && vr_GyroAccYAxis < 1) {
					print ("jump in update over 0.1 acc is" + vr_GyroAccYAxis);	
					m_Jump = false;
				} else if (vr_GyroAccYAxis >= 0.08 && vr_GyroAccYAxis < 0.1) {
					print ("jump in update over 0.08 acc is" + vr_GyroAccYAxis);	
					m_Jump = false;
				} else if (vr_GyroAccYAxis >= 0.05 && vr_GyroAccYAxis < 0.08) {
					print ("jump in update over 0.05 acc is" + vr_GyroAccYAxis);	
					m_Jump = false;
				} else if (vr_GyroAccYAxis >= 0.02 && vr_GyroAccYAxis < 0.05) {
					print ("jump in update over 0.02 acc is" + vr_GyroAccYAxis);	
					m_Jump = false;
				} else if (vr_GyroAccYAxis > 0 && vr_GyroAccYAxis < 0.02) {
					print ("jump in update over 0, acc is" + vr_GyroAccYAxis);	
					m_Jump = false;
				}
			}

			#if !MOBILE_INPUT
			if (!m_Jump)
			{
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
			#endif

			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
			{
				StartCoroutine(m_JumpBob.DoBobCycle());
				PlayLandingSound();
				m_MoveDir.y = 0f;
				m_Jumping = false;
			}
			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
			{
				m_MoveDir.y = 0f;
			}

			m_PreviouslyGrounded = m_CharacterController.isGrounded;
		}


		private void PlayLandingSound()
		{
			m_AudioSource.clip = m_LandSound;
			m_AudioSource.Play();
			m_NextStep = m_StepCycle + .5f;
		}



		private void FixedUpdate()
		{

			float speed;

			GetInput(out speed);


			//Debug.Log("FixedUpdate time :" + Time.deltaTime);
			// always move along the camera forward as it is the direction that it being aimed at
			//Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

			//For Auto walk, uncomment this line.
			m_Input.y=0.2f;

			//			if (m_walk == true) {
			//				m_Input.y = 0.2f;
			//			} else {
			//				m_Input.y = 0;
			//			}
			//
			//			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
			//			{
			//				StartCoroutine(m_JumpBob.DoBobCycle());
			//				PlayLandingSound();
			//				m_MoveDir.y = 0f;
			//				m_Jumping = false;
			//			}
			//			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
			//			{
			//				m_MoveDir.y = 0f;
			//			}
			//
			//			m_PreviouslyGrounded = m_CharacterController.isGrounded;

			Vector3 desiredMove = head.transform.forward*m_Input.y + head.transform.right*m_Input.x;

			//print (desiredMove);

			// get a normal for the surface that is being touched to move along it
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
				m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

			m_MoveDir.x = desiredMove.x*speed;
			m_MoveDir.z = desiredMove.z*speed;

			if (m_CharacterController.isGrounded)
			{
				m_MoveDir.y = -m_StickToGroundForce;

				if (m_Jump)
				{
					m_MoveDir.y = m_JumpSpeed;
					PlayJumpSound();
					m_Jump = false;
					m_Jumping = true;
				}
			}
			else
			{
				m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
			}
				
				
			m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

			ProgressStepCycle(speed);
			UpdateCameraPosition(speed);

			m_MouseLook.UpdateCursorLock();
		}


		private void PlayJumpSound()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play();
		}


		private void ProgressStepCycle(float speed)
		{
			if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
			{
				m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
					Time.fixedDeltaTime;
			}

			if (!(m_StepCycle > m_NextStep))
			{
				return;
			}

			m_NextStep = m_StepCycle + m_StepInterval;

			PlayFootStepAudio();
		}


		private void PlayFootStepAudio()
		{
			if (!m_CharacterController.isGrounded)
			{
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			int n = Random.Range(1, m_FootstepSounds.Length);
			m_AudioSource.clip = m_FootstepSounds[n];
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			m_FootstepSounds[n] = m_FootstepSounds[0];
			m_FootstepSounds[0] = m_AudioSource.clip;
		}


		private void UpdateCameraPosition(float speed)
		{
			Vector3 newCameraPosition;
			if (!m_UseHeadBob)
			{
				return;
			}
			if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
			{
				m_Camera.transform.localPosition =
					m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
						(speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
			}
			else
			{
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
			}
			m_Camera.transform.localPosition = newCameraPosition;
		}


		private void GetInput(out float speed)
		{
			// Read input
			//float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			//float vertical = CrossPlatformInputManager.GetAxis("Vertical");

			float horizontal = 0.0f;
			float vertical = 0.0f;

			int i = 0;
			//Touch touch;

			if (Input.touchCount > 0) {
				Debug.Log("touchcount" + Input.touchCount);
				for (i = 0; i < Input.touchCount; i++) {
					Touch touch = Input.GetTouch (i);
					if (touch.phase == TouchPhase.Began) {
						//print ("Touch began");
						if (touch.tapCount >= 2) {
							//print ("Tap count >= 2");
							_moveDirection = -1.0f;
						}  
						else {
							//print ("Tap count = 2");
							_moveDirection = 1.0f;
						}
					}  
					else if (touch.phase == TouchPhase.Stationary) {
						vertical = 0.1f * _moveDirection;
						//print ("keep touching" + vertical);
					}  
				}
			}  else
				vertical = 0.0f;


			// Use mouse to simulate the touch screen, if user touch then move forward
			// Added by Aaron 2016.07.16
			if (Input.GetMouseButton (0)) {
				Debug.Log("foward");
				vertical = 0.2f;
			} 

			//Analógico esquerdo no DS4
			if (Input.GetAxis ("Vertical") > 0) {
				//print ("foward");
				vertical = 0.2f;
			} else if (Input.GetAxis ("Vertical") < 0) {
				vertical = -0.2f;
			}

			if (Input.GetAxis ("Horizontal") > 0) {
				//print ("foward");
				horizontal = 0.2f;
			} else if (Input.GetAxis ("Horizontal") < 0) {
				horizontal = -0.2f;
			}



			//Quadrado no DS4
			if (Input.GetButton("Jump") || Input.GetKeyDown(KeyCode.Joystick1Button0)) {
				//print ("foward");
				m_Jump = true;
			} 

		
			bool waswalking = m_IsWalking;

			#if !MOBILE_INPUT
			// On standalone builds, walk/run speed is modified by a key press.
			// keep track of whether or not the character is walking or running
			m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
			#endif

			// set the desired speed to be walking or running
			//speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;

			//Modify by aaron, set speed;
			speed = m_WalkSpeed * 1.5f;

			m_Input = new Vector2(horizontal, vertical);

			// normalize input if it exceeds 1 in combined length:
			if (m_Input.sqrMagnitude > 1)
			{
				m_Input.Normalize();
			}

			// handle speed change to give an fov kick
			// only if the player is going to a run, is running and the fovkick is to be used
			if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
			{
				StopAllCoroutines();
				StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
			}
		}


		private void RotateView()
		{
			m_MouseLook.LookRotation (transform, m_Camera.transform);
		}


		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
			//dont move the rigidbody if the character is on top of it
			if (m_CollisionFlags == CollisionFlags.Below)
			{
				return;
			}

			if (body == null || body.isKinematic)
			{
				return;
			}
			body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
		}
	}
}
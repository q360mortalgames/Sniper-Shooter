//InputControl.cs by Azuline Studios© All Rights Reserved
//Manages button and axis input to be read by the other scripts.
using UnityEngine;
using System.Collections;
using TouchControlsKit;
using UnityEngine.UI;
public class InputControl : MonoBehaviour {

	private FPSPlayer FPSPlayerComponent;
	
	//button states that are accessed by the other scripts
	[HideInInspector]
	public bool fireHold;
	[HideInInspector]
	public bool firePress;
	[HideInInspector]
	public bool reloadPress;
	[HideInInspector]
	public bool fireModePress;
	[HideInInspector]
	public bool jumpHold;
	[HideInInspector]
	public bool jumpPress;
	[HideInInspector]
	public bool crouchHold;
	[HideInInspector]
	public bool proneHold;
	[HideInInspector]
	public bool sprintHold;
	[HideInInspector]
	public bool zoomHold;
	[HideInInspector]
	public bool zoomPress;
	[HideInInspector]
	public bool leanLeftHold;
	[HideInInspector]
	public bool leanRightHold;
	[HideInInspector]
	public bool useHold;
	[HideInInspector]
	public bool usePress;
	[HideInInspector]
	public bool usePressUp;
	[HideInInspector]
	public bool toggleCameraHold;
	[HideInInspector]
	public bool toggleCameraDown;
	[HideInInspector]
	public bool grenadeHold;
	[HideInInspector]
	public bool deadzonePress;
	[HideInInspector]
	public bool meleePress;
	[HideInInspector]
	public bool flashlightPress;
	[HideInInspector]
	public bool holsterPress;
	[HideInInspector]
	public bool dropPress;
	[HideInInspector]
	public bool bulletTimePress;
	[HideInInspector]
	public bool moveHold;
	[HideInInspector]
	public bool movePress;
	[HideInInspector]
	public bool throwHold;
	[HideInInspector]
	public bool helpPress;
	[HideInInspector]
	public bool menuPress;
	[HideInInspector]
	public bool pausePress;
	[HideInInspector]
	public bool selectNextPress;
	[HideInInspector]
	public bool selectPrevPress;
	[HideInInspector]
	public bool selectGrenPress;
	[HideInInspector]
	public bool selectWeap1Press;
	[HideInInspector]
	public bool selectWeap2Press;
	[HideInInspector]
	public bool selectWeap3Press;
	[HideInInspector]
	public bool selectWeap4Press;
	[HideInInspector]
	public bool selectWeap5Press;
	[HideInInspector]
	public bool selectWeap6Press;
	[HideInInspector]
	public bool selectWeap7Press;
	[HideInInspector]
	public bool selectWeap8Press;
	[HideInInspector]
	public bool selectWeap9Press;
	[HideInInspector]
	public bool selectWeap0Press;
	
	[HideInInspector]
	public float mouseWheel;
	
	[HideInInspector]
	public bool leftHold;
	[HideInInspector]
	public bool rightHold;
	[HideInInspector]
	public bool forwardHold;
	[HideInInspector]
	public bool backHold;
	[HideInInspector]
	public float moveXButton;
	[HideInInspector]
	public float moveYButton;
	
	//gamepad input axes
	[HideInInspector]
	public float deadzone = 0.25f;
	private Vector2 moveInput;
	private Vector2 lookInput;
	
	//combined button and axis inputs for moving
	[HideInInspector]
	public float moveX;
	[HideInInspector]
	public float moveY;
	//combined button and axis inputs for looking
	[HideInInspector]
	public float lookX;
	[HideInInspector]
	public float lookY;
	
	//Xbox 360 dpad controls (button held)
	[HideInInspector]
	public bool xboxDpadLeftHold;
	[HideInInspector]
	public bool xboxDpadRightHold;
	[HideInInspector]
	public bool xboxDpadUpHold;
	[HideInInspector]
	public bool xboxDpadDownHold;
	
	//Xbox 360 dpad controls (button press)
	[HideInInspector]
	public bool xboxDpadLeftPress;
	[HideInInspector]
	public bool xboxDpadRightPress;
	[HideInInspector]
	public bool xboxDpadUpPress;
	[HideInInspector]
	public bool xboxDpadDownPress;
	
	private bool xbdpLstate;
	private bool xbdpRstate;
	private bool xbdpUstate;
	private bool xbdpDstate;

	public bool mobileControls;
	void Start () {
		FPSPlayerComponent = GetComponent<FPSPlayer>();

		#if UNITY_EDITOR
		mobileControls=false;
		#else 
		mobileControls=true;
		#endif
        mobileControls=true;

	}
	public void SwitchWeapon()
	{
		if (PlayerWeapons.instance.currentWeapon < 3) {
			PlayerWeapons.instance.StartCoroutine (PlayerWeapons.instance.SelectWeapon (PlayerWeapons.instance.currentWeapon + 1));

		} else 
		{
			PlayerWeapons.instance.StartCoroutine (PlayerWeapons.instance.SelectWeapon (1));

		}
	}
	public void moveMobile(Vector2 axis)
	{
		moveX = axis.x;
		moveY = axis.y;
		if (moveY > 0.82f) {
			sprintHold = true;
			Debug.Log ("Sprinting");
		} else 
		{
			sprintHold = false;
		}
	}
	public void lookMobile(Vector2 axis)
	{
		lookX = axis.x;
		lookY = axis.y;
	}
	public void Fire(bool status)
	{
		fireHold = status;
        if(status&&!GameManager.instance.FistshotDone)
        {
            GameManager.instance.FistshotDone = true;
            GameManager.instance.CurLevelObj.SendMessage("FirstShot");
        }
	}
    public void Reload(bool status)
    {
        reloadPress = status;
       
    }
    public void Zoom()
	{
		zoomHold = !zoomHold;
	}
    public void ZoomBtn()
    {
        zoomHold = !zoomHold;
        if (zoomHold)
        {
            GameManager.instance.ZoomSlider.value = 0.005f;
            GameManager.instance.ZoomPanel.SetActive(zoomHold);
            GameManager.instance.Sliderparent.SetActive(true);
        }else
        {
            GameManager.instance.ZoomSlider.value = 0f;
            GameManager.instance.ZoomPanel.SetActive(zoomHold);
            GameManager.instance.Sliderparent.SetActive(false);
        }
       
    }
    public void ZoomSliderInput()
    {
        //if(GameManager.instance.ZoomSlider.value>0)
        //{
        //    zoomHold = true;
        //    GameManager.instance.ZoomPanel.SetActive(true);
        //}
        //else
        //{
        //    zoomHold = false;
        //    zoomPress = false;
        //    //GameManager.instance.ZoomPanel.SetActive(false);
            
         
        //}

    }
    
    void Update () {

		if(FPSPlayerComponent && !FPSPlayerComponent.restarting){
			
				//player movement buttons
				leftHold = Input.GetButton ("Left");
				rightHold = Input.GetButton ("Right");
				forwardHold = Input.GetButton ("Forward");
				backHold = Input.GetButton ("Back");
			
				//cancel player movement if opposite buttons are held at the same time
				if (leftHold && !rightHold) {
					moveXButton = -1.0f;
				} else if (rightHold && !leftHold) {
					moveXButton = 1.0f;	
				} else {
					moveXButton = 0.0f;	
				}
			
				if (forwardHold && !backHold) {
					moveYButton = 1.0f;
				} else if (backHold && !forwardHold) {
					moveYButton = -1.0f;	
				} else {
					moveYButton = 0.0f;	
				}
			
				//scaled radial deadzone for joysticks for smooth player movement ramp from deadzone
				moveInput = new Vector2 (Input.GetAxis ("Joystick Move X"), Input.GetAxis ("Joystick Move Y"));
				if (moveInput.magnitude < deadzone) {
					moveInput = Vector2.zero;
				} else {
					moveInput = moveInput.normalized * ((moveInput.magnitude - deadzone) / (1 - deadzone));
				}
			
				lookInput = new Vector2 (Input.GetAxis ("Joystick Look X"), Input.GetAxis ("Joystick Look Y"));
				if (lookInput.magnitude < deadzone) {
					lookInput = Vector2.zero;
				} else {
					lookInput = lookInput.normalized * ((lookInput.magnitude - deadzone) / (1 - deadzone));
				}
			
				//combine button and axis input for player movement
			if (!mobileControls) {
                //	moveX = moveXButton + moveInput.x;
                //moveY = moveYButton + moveInput.y;

                //combine mouse and axis input for player looking (accelerate axis input)
                //lookX = Input.GetAxisRaw ("Mouse X") + AccelerateInput (lookInput.x);
                //lookY = Input.GetAxisRaw ("Mouse Y") + AccelerateInput (lookInput.y);
                lookX = TCKInput.GetAxis("Touchpad", "Horizontal");
                lookY = TCKInput.GetAxis("Touchpad", "Vertical");

            } else 
			{
                //lookX = TCKInput.GetAxis ("Touchpad", "Horizontal") + AccelerateInput (lookInput.x);
                //lookY = TCKInput.GetAxis ("Touchpad", "Vertical") + AccelerateInput (lookInput.y);
                lookX = TCKInput.GetAxis("Touchpad", "Horizontal");
                lookY = TCKInput.GetAxis("Touchpad", "Vertical");
                moveX =0;
                moveY =0;
                //moveX = TCKInput.GetAxis ("Joystick", "Horizontal") + AccelerateInput (lookInput.x);
                //moveY = TCKInput.GetAxis ("Joystick", "Vertical") + AccelerateInput (lookInput.y);
                if (moveY > 0.85f)
					sprintHold = true;
				else
					sprintHold=false;
			}

                
            
					
				//determine if the Xbox 360 dpad buttons have been pressed or held
				if (Input.GetAxis ("Xbox Dpad X") > 0.0f) {
					xboxDpadRightHold = true;
					xboxDpadLeftHold = false;
					xbdpLstate = false;
					if (!xbdpRstate) {
						xboxDpadRightPress = true;
						xbdpRstate = true;
					} else {
						xboxDpadRightPress = false;
					}
				} else if (Input.GetAxis ("Xbox Dpad X") < 0.0f) {
					xboxDpadRightHold = false;
					xboxDpadLeftHold = true;
					xbdpRstate = false;
					if (!xbdpLstate) {
						xboxDpadLeftPress = true;
						xbdpLstate = true;
					} else {
						xboxDpadLeftPress = false;
					}
				} else {
					xboxDpadRightHold = false;
					xboxDpadLeftHold = false;
					xboxDpadRightPress = false;
					xboxDpadLeftPress = false;
					xbdpLstate = false;
					xbdpRstate = false;
				}
			
				if (Input.GetAxis ("Xbox Dpad Y") > 0.0f) {
					xboxDpadUpHold = true;
					xboxDpadDownHold = false;
					xbdpDstate = false;
					if (!xbdpUstate) {
						xboxDpadUpPress = true;
						xbdpUstate = true;
					} else {
						xboxDpadUpPress = false;
					}
				} else if (Input.GetAxis ("Xbox Dpad Y") < 0.0f) {
					xboxDpadUpHold = false;
					xboxDpadDownHold = true;
					xbdpUstate = false;
					if (!xbdpDstate) {
						xboxDpadDownPress = true;
						xbdpDstate = true;
					} else {
						xboxDpadDownPress = false;
					}
				} else {
					xboxDpadUpHold = false;
					xboxDpadDownHold = false;
					xboxDpadUpPress = false;
					xboxDpadDownPress = false;
					xbdpUstate = false;
					xbdpDstate = false;
				}
			
				//read button input and set the button state vars for use by the other scripts
				mouseWheel = Input.GetAxis ("Mouse Scroll Wheel");
			
				firePress = Input.GetButtonDown ("Fire");
				//zoomPress = Input.GetButtonDown ("Zoom");
				//reloadPress = Input.GetButtonDown ("Reload");
				fireModePress = Input.GetButtonDown ("Fire Mode");
				jumpHold = Input.GetButton ("Jump");
				jumpPress = Input.GetButtonDown ("Jump");
				crouchHold = Input.GetButton ("Crouch");
				proneHold = Input.GetButton ("Prone");
			if (!mobileControls)
				sprintHold = Input.GetButton ("Sprint");
				leanLeftHold = Input.GetButton ("Lean Left");
				leanRightHold = Input.GetButton ("Lean Right");
				useHold = Input.GetButton ("Use");
				usePress = Input.GetButtonDown ("Use");
				usePressUp = Input.GetButtonUp ("Use");
//				toggleCameraHold = Input.GetButton ("Toggle Camera");
//				toggleCameraDown = Input.GetButtonDown ("Toggle Camera");
				grenadeHold = Input.GetButton ("Throw Grenade");
				meleePress = Input.GetButtonDown ("Melee Attack");
				flashlightPress = Input.GetButtonDown ("Toggle Flashlight");
				holsterPress = Input.GetButtonDown ("Holster Weapon");
				dropPress = Input.GetButtonDown ("Drop Weapon");
				bulletTimePress = Input.GetButtonDown ("Bullet Time");
				deadzonePress = Input.GetButtonDown ("Toggle Deadzone Aiming");
				helpPress = Input.GetButtonDown ("Help");
				menuPress = Input.GetButtonDown ("Main Menu");
				pausePress = Input.GetButtonDown ("Pause");
				selectNextPress = Input.GetButtonDown ("Select Next Weapon");
				selectPrevPress = Input.GetButtonDown ("Select Previous Weapon");
				selectGrenPress = Input.GetButtonDown ("Select Next Grenade");
				selectWeap1Press = Input.GetButtonDown ("Select Weapon 1");
				selectWeap2Press = Input.GetButtonDown ("Select Weapon 2");
				selectWeap3Press = Input.GetButtonDown ("Select Weapon 3");
				selectWeap4Press = Input.GetButtonDown ("Select Weapon 4");
				selectWeap5Press = Input.GetButtonDown ("Select Weapon 5");
				selectWeap6Press = Input.GetButtonDown ("Select Weapon 6");
				selectWeap7Press = Input.GetButtonDown ("Select Weapon 7");
				selectWeap8Press = Input.GetButtonDown ("Select Weapon 8");
				selectWeap9Press = Input.GetButtonDown ("Select Weapon 9");
				selectWeap0Press = Input.GetButtonDown ("Select Weapon 0");
//
//			} else 
//			{
////				UnityEngine.UI.Joystick.
//			}
		}
		else{
			fireHold = false;//stop shooting if level is restarting
		}
	
	}
	
	//accelerate axis input for easier control and reduction of axis drift (deadzone improvement)
	float AccelerateInput(float input){
		float inputAccel;
		inputAccel = ((1.0f/4.0f) * input * (Mathf.Abs(input)*4.0f)) * Time.smoothDeltaTime * 60.0f;
		return inputAccel;
	}
	
}

using UnityEngine;
using System.Collections;

public class TouchDragControll : MonoBehaviour
{
	public float rotateSpeed = 100.0f;
	public int invertPitch = 1;
	public Transform player;
	private float pitch = 0.0f;
	private Vector3 oRotation;
	private Vector3 CurRotation;
	float offsetX,offsetY;
	Quaternion tempQ;
	private Vector2 TouchPos,PrevPos,NextPos;
	public TextMesh x,y,x1,y1;
	private bool Dragging;
	private float X1,X2,Y1,Y2;
//	SmoothMouseLook mouelook;
	public bool dontreset;
	InputControl InputControlComponent;
	void Start()
	{
		InputControlComponent = GetComponent<InputControl> ();
		Dragging = false;
		offsetX = offsetY = 0;

	}
	void Update()
	{
		if (!Dragging) 
		{
//			mouelook.offsetX = Mathf.Lerp (mouelook.offsetX, 0, Time.deltaTime * 7);
//			mouelook.offsetY = Mathf.Lerp (mouelook.offsetY, 0, Time.deltaTime * 7);
			if (InputControlComponent.mobileControls)
			{
				InputControlComponent.lookX = 0;
				InputControlComponent.lookY = 0;
			}
		}
		Y1 = Mathf.Lerp (Y1, Y2, Time.deltaTime*4 );
		X1 = Mathf.Lerp (X1, X2, Time.deltaTime*4 );
		if (Input.touchCount == 0)
		{
			Dragging = false;
		}
//		if (Input.touchCount >0) {

			for (int i = 0; i < Input.touchCount; i++) {
				if(Input.GetTouch (i).position.x>Screen.width/2)
				{
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					Dragging = true;
					TouchPos = Input.GetTouch (i).position;
					
					X1 = X2 = TouchPos.x / Screen.width;
					Y1 = Y2 = TouchPos.y / Screen.height;
				}
				if (Input.GetTouch (i).phase == TouchPhase.Moved) {
					Dragging = true;
					TouchPos = Input.GetTouch (i).position;
					X2 = TouchPos.x / Screen.width;
					Y2 = TouchPos.y / Screen.height;
					offsetX = (X2 - X1) * 10f;
					offsetY = (Y2 - Y1) * 10f;

					offsetX = Mathf.Clamp (offsetX, -4, 4);
					offsetY = Mathf.Clamp (offsetY, -4, 4);
//						mouelook.offsetX = offsetX;
//						mouelook.offsetY = offsetY;
					InputControlComponent.lookX = offsetX;
					InputControlComponent.lookY = offsetY;
//					PrevPos = Vector2.Lerp (PrevPos, NextPos, Time.deltaTime);
				}
				if (Input.GetTouch (i).phase == TouchPhase.Stationary) {

					Y1 = Mathf.Lerp (Y1, Y2, Time.deltaTime * 0.05f);
					X1 = Mathf.Lerp (X1, X2, Time.deltaTime * 0.05f);
				}

						if (Input.GetTouch (i).phase == TouchPhase.Ended) {
							Dragging = false;
							X1 = Y1 = X2 = Y2 = 0;
							InputControlComponent.lookX=0;
							InputControlComponent.lookY=0;
						}
				}
			}
//			}
//		}
	}


}
using UnityEngine;
using System.Collections;



public class PuckCode : MonoBehaviour {
	
	float RotationSpeed = 100;
	
	bool puckMoving=false;
	
	
	// Use this for initialization
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DIRECTION direction=DIRECTION.NONE;
		//Debug.Log(rigidbody.velocity.magnitude);
		
		if (rigidbody.velocity.magnitude>1) puckMoving=true;
		if (puckMoving && rigidbody.velocity.magnitude<1) {
			puckMoving=false;
			transform.rotation=Quaternion.Euler(0,0,0);
		}	
		
		if (Input.GetKey(KeyCode.LeftArrow)) direction=DIRECTION.LEFT;
		if (Input.GetKey(KeyCode.RightArrow)) direction=DIRECTION.RIGHT;
		
		if (MainGameCode.gamestate==GAMESTATE.AIM && direction!=DIRECTION.NONE) {
			int rotationSwitch=1;
			if (direction==DIRECTION.LEFT) rotationSwitch=-1;
			
		 transform.Rotate(0, (RotationSpeed*Time.deltaTime*rotationSwitch), 0, Space.World); 
		}	
	}
	
}

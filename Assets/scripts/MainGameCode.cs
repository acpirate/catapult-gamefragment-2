using UnityEngine;
using System.Collections;

public enum GAMESTATE { TITLE, PLAY, GAMEOVER, SETTINGS, AIM };
public enum DIRECTION { LEFT, RIGHT, NONE, UP, DOWN};
public enum ENGINE { BALLISTA, CATAPULT, TREBUCHET};

public class MainGameCode : MonoBehaviour {
	
	public static GameObject king=null;
	public static GameObject puck=null;
	public static GameObject aimCamera=null;
	static Camera mainCamera=null;
	static Vector3 mainCameraStart=new Vector3(0,30,-280);
	
	
	public GameObject brickPrefab;
	static GameObject staticBrickPrefab;
	static int wallWidth=12;
	static int wallHeight=10;
	static float brickWidth=20f;
	static float brickHeight=10f;
	static float brickHeightOffset=5.47986f;
	static float wallXStart=-300;
	static float wallZStart=950;
	
	static ENGINE engine = ENGINE.BALLISTA;
	
	
	static float powerChargeRate=50;
	public static float maxPower=100;
	public static float currentPower=0;
	public static float powerMultiplier=100f;
	public static float puckResetLocation=-245;
	
	public static GAMESTATE gamestate=GAMESTATE.TITLE;

	// Use this for initialization
	void Awake() {
		if (king==null) king=GameObject.Find("King");
		if (puck==null) puck=GameObject.Find("Puck");
		if (aimCamera==null) aimCamera=GameObject.Find("AimCamera");
		if (mainCamera==null) mainCamera=GameObject.Find("Main Camera").GetComponent<Camera>();
		
		staticBrickPrefab=brickPrefab;
		
		BuildWall();
		
	}	
	
	void Start () {
		mainCamera.transform.LookAt(puck.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		//look at the puck while its flying
		if (puck.rigidbody.velocity.magnitude>0) mainCamera.transform.LookAt(puck.transform.position);
		//reset the puck if it falls off
		if (puck.transform.position.y<-100) ResetPuck();
	}
	//static methods
	public static void SetEngine(ENGINE inEngine) {
		engine=inEngine;
	}	
	
	static void BuildWall() {
		for (int yCounter=0;yCounter<wallHeight;yCounter++) {
			for (int xCounter=0;xCounter<wallWidth;xCounter++) {
				float brickOffset=0;
				if (yCounter%2==1) brickOffset=brickWidth/2;
				Vector3 brickLocation=new Vector3(wallXStart+xCounter*brickWidth+brickOffset,brickHeight*yCounter+brickHeightOffset,wallZStart);
				Instantiate(staticBrickPrefab,brickLocation,Quaternion.identity);
			}	
		}	
	}	
	
	
	
	
	public static void ShootPuck() {
		EndAim();
		puck.rigidbody.AddRelativeForce(new Vector3(0,currentPower*powerMultiplier,currentPower*powerMultiplier));
		puck.rigidbody.AddTorque(new Vector3(currentPower*powerMultiplier*10,0,0));
		
		currentPower=0;
	}	
	
	public static void QuitGame() {
		ClearBricks();
		ResetKingPedestal();
		ResetKing();
		ResetPuck();
		BuildWall();
		gamestate=GAMESTATE.GAMEOVER;
	}
	
	public static void ResetGame() {
		QuitGame();
		gamestate=GAMESTATE.TITLE;	
	}	
	
	public static void ClearBricks() {
		foreach(GameObject brick in GameObject.FindGameObjectsWithTag("Brick")) {
			Destroy(brick);	
		}	
	}	
	
	
	public static void PlayGame() {
		gamestate=GAMESTATE.PLAY;	
	}
	
	public static void GameOver() {
		gamestate=GAMESTATE.GAMEOVER;	
	}	
	
	public static void AimMode() {
		mainCamera.enabled=false;
		puck.GetComponent<LineRenderer>().enabled=true;
		gamestate=GAMESTATE.AIM;
		//if (gamestate==GAMESTATE.AIM) GUICode.testPositive=true;
	}
	
	public static void EndAim() {
		mainCamera.enabled=true;
		puck.GetComponent<LineRenderer>().enabled=false;
		gamestate=GAMESTATE.PLAY;
	}	
	
	public static void ResetKingPedestal() {
		
		GameObject tempObject=null;
		tempObject=(GameObject) Instantiate(staticBrickPrefab,new Vector3(-150,6f,1000),Quaternion.identity);	
		tempObject.rigidbody.velocity=new Vector3(0,0,0);
		tempObject=(GameObject) Instantiate(staticBrickPrefab,new Vector3(-150,17f,1000),Quaternion.identity);
		tempObject.rigidbody.velocity=new Vector3(0,0,0);
	}	
	
	public static void ResetKing() {
		king.rigidbody.velocity=new Vector3(0,0,0);
		king.transform.position=new Vector3(-150,27f,1000);
		king.transform.eulerAngles=new Vector3(0,0,0);
		king.rigidbody.velocity=new Vector3(0,0,0);
		king.GetComponent<KingCode>().Stabilize();
	}	
	
	public static void ResetPuck() {
		puck.transform.position=new Vector3(0,1.5f,puckResetLocation);
		puck.transform.rotation=Quaternion.Euler(0,0,0);
		puck.rigidbody.velocity=new Vector3(0,0,0);
		//mainCamera.transform.position=mainCameraStart;
		//mainCamera.transform.LookAt(puck.transform.position);
	}	
	
	public static void PowerCharge() {
		currentPower+=powerChargeRate*Time.deltaTime;
		if (currentPower>maxPower) currentPower=0;
		
	}	
}

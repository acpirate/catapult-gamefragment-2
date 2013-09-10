﻿using UnityEngine;
using System.Collections;

public class GUICode : MonoBehaviour {
	
	
	public GUIStyle titleStyle;
	public GUIStyle instructionStyle;
	public GUIStyle powerMeterTextStyle;
	public GUIStyle powerMeterInteriorStyle;
	
	//buttons
	int buttonSize=60;
	int buttonOffset=20;
	//settings window
	int settingsWindowWidth=20; //percent
	int settingsWindowHeight=50; //percent
	//target indicator
	int targetIndicatorSize=20;
	//power meter
	int powerMeterMax=100;
	int powerMeterLength=100;
	int powerMeterWidth=20;
	//engine selection
	int engineSelectNumber=0;
			
	public Texture2D settingsTexture;
	public Texture2D resetTexture;
	public Texture2D targetTexture;
	//test code
	//public static bool testPositive=false;
	
	void Awake() {
	}	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		switch (MainGameCode.gamestate) {
			case GAMESTATE.TITLE:
				if (Input.GetMouseButtonDown(0))  {
					MainGameCode.PlayGame();
				}			
			break;
			case GAMESTATE.GAMEOVER:
				if (Input.GetMouseButtonDown(0))  {
					MainGameCode.ResetGame();
				}			
			break;
			case GAMESTATE.PLAY:
				if (Input.GetKey(KeyCode.Return)) {
					MainGameCode.AimMode(); 
				}
			break;
			case GAMESTATE.AIM:
				if (Input.GetKey(KeyCode.Escape)) {
					MainGameCode.EndAim();
				}
				if (Input.GetKey(KeyCode.Space)) {
						MainGameCode.PowerCharge();
					}			
				if (Input.GetKeyUp(KeyCode.Space) && MainGameCode.currentPower>0) {
					MainGameCode.ShootPuck();
				}
			break;
			
		}	
		
	}		
	
	void OnGUI() {
		
		//if (testPositive) GUI.Box(new Rect(10,10,100,100),"test positive");
		
		switch (MainGameCode.gamestate) {
			case GAMESTATE.TITLE:	
				DrawTitle();
			break;
			case GAMESTATE.PLAY:
				PlayUIButtons();
				PlayInstructions();
			break;
			case GAMESTATE.SETTINGS:
				DrawSettingsWindow();
			break;
			case GAMESTATE.GAMEOVER:
				DrawGameOver();
			break;
			case GAMESTATE.AIM:
				AimInstructions();
				DisplayTargetIndicator();
				DrawPowerMeter();
				DrawEngineSelect();
			break;			
		}	
	}	
	
	
	void DrawEngineSelect() {
		GUIContent[] selectionContent=new GUIContent[3];
		selectionContent[0]=new GUIContent("ballista");
		selectionContent[1]=new GUIContent("catapult");
		selectionContent[2]=new GUIContent("trebuchet");
		
		
		Rect engineSelectPosition=new Rect(10,20,100,100);
		
		
		engineSelectNumber=GUI.SelectionGrid(engineSelectPosition,engineSelectNumber,selectionContent,1);
		switch (engineSelectNumber) {
			case 0:
				MainGameCode.SetEngine(ENGINE.BALLISTA);
			break;
			case 1:
				MainGameCode.SetEngine(ENGINE.CATAPULT);
			break;
			case 2:
				MainGameCode.SetEngine(ENGINE.TREBUCHET);
			break;
		}	
		
		
	}	
	
	void DrawPowerMeter() {
		Rect powerTextPosition=new Rect(10,Screen.height*.9f,100,20);
		ShadowAndOutline.DrawOutline(powerTextPosition,"Power",powerMeterTextStyle,Color.black,Color.white,2);
		
		Rect powerContainerPosition= new Rect(50,Screen.height*.9f-powerMeterLength-10,powerMeterWidth,powerMeterLength);
		GUI.Box(powerContainerPosition,"");
		
		float powerMeterInteriorLength=powerMeterLength*MainGameCode.currentPower/powerMeterMax;
		float powerMeterInteriorVeritcalPositionAdjustment=powerMeterLength-powerMeterLength*MainGameCode.currentPower/powerMeterMax;
		
		Rect powerDisplay=new Rect(50,Screen.height*.9f-powerMeterLength+powerMeterInteriorVeritcalPositionAdjustment-10,powerMeterWidth, powerMeterInteriorLength);
		GUI.Box(powerDisplay,"",powerMeterInteriorStyle);
		
			
	}	
	
	void DisplayTargetIndicator() {
		GUIStyle tempStyle=new GUIStyle();
		Vector3 kingPosition=MainGameCode.king.transform.position;
		Vector3 kingScreenPosition = MainGameCode.aimCamera.GetComponentInChildren<Camera>().WorldToViewportPoint(kingPosition);
		
		float kingScreenPositionHorizontal=kingScreenPosition.x;
		if (kingScreenPositionHorizontal>1) kingScreenPositionHorizontal=1;
		if (kingScreenPositionHorizontal<0) kingScreenPositionHorizontal=0;
		
		float kingScreenPostionHorizontalTranslation=Screen.width*kingScreenPositionHorizontal;
		
		Rect targetIndicatorLocation=new Rect(kingScreenPostionHorizontalTranslation-targetIndicatorSize*.5f,10,targetIndicatorSize,targetIndicatorSize);		
		
		//Debug.Log("guicode: king x viewport position " +  viewPos.x);
		
		
		
		GUI.Box(targetIndicatorLocation,targetTexture,tempStyle);
		
	}	
	
	void DrawGameOver() {
		ShadowAndOutline.DrawOutline(new Rect(0,Screen.height*.25f,Screen.width,Screen.height*.5f),"Game Over",titleStyle,Color.black,Color.white,2f);
		if ((float.Parse(Time.time.ToString("0.0"))) % 3<2.5f)
		ShadowAndOutline.DrawOutline(new Rect(0,Screen.height*.75f,Screen.width,Screen.height*.25f),"Click anywhere to go back to title",instructionStyle,Color.black,Color.white,2f);	
	}	
	
	void DrawTitle() {
		ShadowAndOutline.DrawOutline(new Rect(0,Screen.height*.25f,Screen.width,Screen.height*.5f),"Catapult!",titleStyle,Color.black,Color.white,2f);
		if ((float.Parse(Time.time.ToString("0.0"))) % 3<2.5f)
		ShadowAndOutline.DrawOutline(new Rect(0,Screen.height*.75f,Screen.width,Screen.height*.25f),"Click anywhere to play",instructionStyle,Color.black,Color.white,2f);
	}	
	
	void PlayUIButtons() {
		DrawSettingsButton();
		DrawResetPuckButton();
	}	
	
	void PlayInstructions() {
		Rect instructionPosition=new Rect(0,Screen.height*.9f,Screen.width,Screen.height*.10f);
		ShadowAndOutline.DrawOutline(instructionPosition,"left ctrl+mouse to look, enter to aim puck\n wasd moves view",instructionStyle,Color.black,Color.white,2f);
	}	
	
	void AimInstructions() {
		Rect instructionPosition=new Rect(0,Screen.height*.8f,Screen.width,Screen.height*.10f);
		ShadowAndOutline.DrawOutline(instructionPosition,"arrow keys to turn puck and tilt view, esc to cancel\nhold space to power up, release to fire",instructionStyle,Color.black,Color.white,2f);
	}	
	
	void DrawSettingsButton() {	
		if (GUI.Button(new Rect(buttonOffset,Screen.height-buttonOffset-buttonSize,buttonSize,buttonSize),
			settingsTexture)) 
			MainGameCode.gamestate=GAMESTATE.SETTINGS;
		
	}	
	
	void DrawResetPuckButton() {	
		if (GUI.Button(new Rect(Screen.width-buttonSize-buttonOffset,Screen.height-buttonOffset-buttonSize,buttonSize,buttonSize),
			resetTexture)) 
			MainGameCode.ResetPuck();
		
	}	
	
	void DrawSettingsWindow() {
		/*GUI.Box(new Rect(Screen.width*.5f-Screen.width*.5f*.01f*settingsWindowWidth,
						 Screen.height*.5f-Screen.height*.5f*.01f*settingsWindowHeight,
						 Screen.width*.01f*settingsWindowWidth,
						 Screen.height*.01f*settingsWindowHeight),
				"");*/

		GUILayout.BeginArea(new Rect(Screen.width*.5f-Screen.width*.5f*.01f*settingsWindowWidth,
						 Screen.height*.5f-Screen.height*.5f*.01f*settingsWindowHeight,
						 Screen.width*.01f*settingsWindowWidth,
						 Screen.height*.01f*settingsWindowHeight));
		if (GUILayout.Button("Back To Game")) 
			MainGameCode.gamestate=GAMESTATE.PLAY;
		if (GUILayout.Button("Quit Game")) 
			MainGameCode.QuitGame();
		
		
		GUILayout.EndArea();
	}	
	
}

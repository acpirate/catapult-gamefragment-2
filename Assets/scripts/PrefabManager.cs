using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {
	
	public static GameObject puckDustPrefab=null;
	public GameObject puckDustPrefabLoader;
	
	
	// Use this for initialization
	void Awake () {
		puckDustPrefab=puckDustPrefabLoader;
	}		
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

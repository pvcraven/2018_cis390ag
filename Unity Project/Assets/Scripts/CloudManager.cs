using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	public GameObject[] cloudPrefabList;

	public float delay;

	public int cloudLimit;

	public static bool spawnClouds = true;

	public static int cloudCount = 0;

	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnClouds ());
	}

	IEnumerator SpawnClouds(){
		while (true) {
			while (spawnClouds) {
				if (cloudPrefabList.Length > 0 && cloudCount < cloudLimit) {
					cloudCount += 1;
					GameObject cloudPrefab = cloudPrefabList[Random.Range(0, cloudPrefabList.Length)];
					Instantiate (cloudPrefab);
				}
				yield return new WaitForSeconds (delay);
			}
		}
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couloirs : MonoBehaviour {

	// Use this for initialization
	public List<Vector3> couloirsList;
	public int numberOfCorridor = 4;
	public GameObject background;
	private float lengthCorridor;
	private float corridorPosition;
	private float distanceCorridor = 0;
	public GameObject test;
	private float firstCorridorDistance;
	Camera cam ;

	public void createCorridors () {
		cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		lengthCorridor = (2f * cam.orthographicSize-1) / numberOfCorridor;
		distanceCorridor = (cam.transform.position.y - 2f * cam.orthographicSize / 2) + lengthCorridor / 2;
		firstCorridorDistance = distanceCorridor;

		for (int i = 0; i< numberOfCorridor; i ++) {
			couloirsList.Add((new Vector3(cam.transform.position.x, distanceCorridor,-1)));
			distanceCorridor = distanceCorridor + lengthCorridor;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour {


	public Button playGame;
	public Button exit;

	// Use this for initialization
	void Start () {
		playGame.onClick.AddListener(loadExplanationPage);
		exit.onClick.AddListener(leavegame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void loadExplanationPage()
	{
		SceneManager.LoadScene("ExplanationPage");
	}
	void leavegame()
	{
		Application.Quit();
	}
}

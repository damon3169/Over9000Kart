using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {

    public GameObject textSpeedJ1Value;
    public GameObject textSpeedJ2Value;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Ship ship in GameManager.instance.getListShip())
        {
            switch (ship.idJoueur)
            {
                case 1:
                    textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(ship.score).ToString();
                    break;
                case 2:
                    textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(ship.score).ToString();
                    break;
            }
        }
    }
}

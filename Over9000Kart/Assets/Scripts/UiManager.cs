using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {

    public GameObject textSpeedJ1Value;
    public GameObject textSpeedJ2Value;
	
	void Update () {
        foreach (Ship ship in GameManager.instance.getListShip()) // pour chaque ship in game
        {
            switch (ship.idJoueur) // selon son numéro de joueur
            {
                case 1:
                    // on met à jour son score sur l'interface
                    if (ship.score > 9000)
                    {
                        textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = "WIN"; 
                    } else if (!GameManager.instance.finished) {
                        textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(ship.score).ToString();
                    } else {
                        textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = "LOSE";
                    }
                    break;
                case 2:
                    if (ship.score > 9000)
                    {
                        textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = "WIN";
                    }
                    else if (!GameManager.instance.finished)
                    {
                        textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(ship.score).ToString();
                    }
                    else
                    {
                        textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = "LOSE";
                    }
                    break;
            }
        }
    }
}

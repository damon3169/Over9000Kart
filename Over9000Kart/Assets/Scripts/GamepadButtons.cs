using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadButtons : MonoBehaviour {

    public GameObject start;
    public GameObject exit;

    int buttonSelected;
    float baseColorNum;
    float selectedColorNum;
    Color colorBase;
    Color colorSelected;

    SpriteRenderer sStart;
    SpriteRenderer sExit;

    bool dPadPressed;

    // Use this for initialization
    void Start () {
        dPadPressed = false;
        baseColorNum = 0.5f;
        selectedColorNum = 1;
        colorBase = new Color(baseColorNum, baseColorNum, baseColorNum);
        colorSelected = new Color(selectedColorNum, selectedColorNum, selectedColorNum);
        buttonSelected = 0;

        sStart = start.GetComponentInChildren<SpriteRenderer>();
        sExit = exit.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonSelected == 0)
        {
            sStart.color = colorSelected;
            sExit.color = colorBase;
        }
        else
        {
            sStart.color = colorBase;
            sExit.color = colorSelected;
        }

        if (Input.GetButtonDown("Player1_ChangeCorridor_K") || (Input.GetAxis("Player1_ChangeCorridor_J") != 0 && !dPadPressed))
        {
            dPadPressed = true;
            if (Input.GetAxis("Player1_ChangeCorridor_K") < 0 || Input.GetAxis("Player1_ChangeCorridor_J") < 0)
            {
                if (buttonSelected < 1) buttonSelected++;
                //sStart.color = colorSelected;
            }
            if (Input.GetAxis("Player1_ChangeCorridor_K") > 0 || Input.GetAxis("Player1_ChangeCorridor_J") > 0)
            {
                if (buttonSelected > 0) buttonSelected--;
            }
        }

        else if (Input.GetAxis("Player1_ChangeCorridor_J") == 0 && dPadPressed) dPadPressed = false;

        if (Input.GetButtonDown("Submit"))
        {
            switch(buttonSelected)
            {
                case 0:
                    start.GetComponent<Button>().onClick.Invoke();
                    break;
                case 1:
                    exit.GetComponent<Button>().onClick.Invoke();
                    break;
            }
        }
    }
}

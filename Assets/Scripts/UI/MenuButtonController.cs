using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtonController : MonoBehaviour
{
    public MenuButton[] menuButtons;

    [HideInInspector]
    public int buttonIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && buttonIndex > 0)
        {
            menuButtons[buttonIndex].Toggle();
            buttonIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && buttonIndex < menuButtons.Length - 1)
        {
            menuButtons[buttonIndex].Toggle();
            buttonIndex++;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            menuButtons[buttonIndex].Select();
        }
    }
}

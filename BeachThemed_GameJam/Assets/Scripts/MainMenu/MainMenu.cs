using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, playMenu, settingsMenu;

    //public Animator menuAnimator;

    public AudioSource MenuASource;
    public AudioClip buttonClick, buttonSwoosh;

    public void Update()
    {
        if (playMenu.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current.buttonEast.isPressed)
            {
                MainMenuButton();
                ButtonClickSound();
            }
        }
    }


    public void PlayButton()
    {
        mainMenu.SetActive(false);
        //menuAnimator.SetBool("MToL", true);
        //menuAnimator.SetBool("LToM", false);
        playMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        //menuAnimator.SetBool("MToL", false);
        //menuAnimator.SetBool("LToM", true);
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ButtonClickSound()
    {
        MenuASource.PlayOneShot(buttonClick);
    }

    //Future maybe???
/*    public void ButtonSwooshSound()
    {
        MenuASource.PlayOneShot(buttonSwoosh);
    }*/

    public void QuitButton()
    {
        Application.Quit();
    }
}

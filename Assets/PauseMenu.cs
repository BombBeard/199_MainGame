using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;



public class PauseMenu : MonoBehaviour {

    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    MouseLook mouseControl;

    public Button[] buttons;
    int selectedBtnIndex = 0;
    float time;

    private void Start()
    {
        mouseControl = GameManager.instance.GetPlayerObject().gameObject.GetComponent<FirstPersonController>().m_MouseLook;
        time = Time.unscaledTime;
    }

    // Update is called once per frame
    void Update () {

        if (CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            if (GameManager.instance.isGamePaused)
            {
                Resume();

            }
            else
            {
                Pause();
            }
        }
        if (GameManager.instance.isGamePaused && Input.GetJoystickNames()[0] == "Controller (Xbox 360 Wireless Receiver for Windows)")
        {
           if (CrossPlatformInputManager.GetAxis("Vertical") >= 0.2 && Time.unscaledTime - time > .25f)
            {
                selectedBtnIndex--;
                if (selectedBtnIndex < 0) selectedBtnIndex = buttons.Length - 1;
                buttons[selectedBtnIndex].Select();
                time = Time.unscaledTime;
            }
            else if(CrossPlatformInputManager.GetAxis("Vertical") < -0.2 && Time.unscaledTime - time > .25f)
            {
                selectedBtnIndex++;
                selectedBtnIndex = selectedBtnIndex % buttons.Length;
                buttons[selectedBtnIndex].Select();
                time = Time.unscaledTime;
            }
            else if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                buttons[selectedBtnIndex].onClick.Invoke();
            }
        }



    }

    public void Resume()
    {
        GameManager.instance.isGamePaused = false;
        mouseControl.SetCursorLock(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        mouseControl.SetCursorLock(false);
        GameManager.instance.isGamePaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Resume();
        SceneManager.LoadScene(GameManager.instance.judgementRoomSceneName);
    }
}

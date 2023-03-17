using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;//Instance Menu Script

    [SerializeField] public Menu[] menus;//Ref All Menus in the Scene
    bool inMenu;

    void Awake()
    {
        Instance = this;//Make this an Instance
        Time.timeScale = 1f;//Set TIme Scale to 1 i.e Active
        inMenu = false;
        if(SceneManager.GetActiveScene().name == "MainMenu")
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenMenu(string menuName)//To Open Menu
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                //AudioManager.Instance.UISound();//Play UI Sound...
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void CloseMenu(Menu menu)//Close the Menu
    {
        menu.Close();
    }

    public void InMenu()
    {
        inMenu = true;
        CheckEnemies(false);
    }

    public void NotInMenu()
    {
        inMenu = false;
        CheckEnemies(true);
    }

    public void Play(string Game)
    {
        //AudioManager.Instance.UISound();//Play UI Sound...
        SceneManager.LoadScene(Game);
    }

    public void MainMenu()//Got to Main Menu Scene
    {
        //AudioManager.Instance.UISound();//Play UI Sound...
        SceneManager.LoadScene("MainMenu");
    }

    public void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
            return;
        if(FindObjectOfType<PlayerInput>().pause && !inMenu)
        {
            OpenMenu("pause");
            FindObjectOfType<ThirdPersonController>()._isInteracting = true;
            Pause();
            FindObjectOfType<PlayerInput>().pause = false;
        }
    }

    public void Pause()//Pause the Game
    {
        CheckEnemies(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void Resume()//Resume the Game
    {
        CheckEnemies(true);
        FindObjectOfType<PlayerInput>().cursorLocked = true;
        FindObjectOfType<ThirdPersonController>()._isInteracting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void Restart()//Restart the Game
    {
        //AudioManager.Instance.UISound();//Play UI Sound...
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//Activate the Game Scene Again
        //AudioManager.Instance.SwapTrack(AudioManager.Instance.gameAudio);//Change game Aud
    }

    public void Quit()//Quit the Game App
    {
		//AudioManager.Instance.UISound();//Play UI Sound...
        Application.Quit();
    }

    public void CheckEnemies(bool enemyActive)
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI _enemy in enemies)
        {
            _enemy.enabled = true;
        }
    }

    public void ResetSave()
    {
        SaveManager.Instance.ResetSave();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance{ set; get;}//Instance Set var and Get var
    public SaveState state;//Ref Save State

    private void Awake()
    {
        Instance = this;//Instance the Script
        DontDestroyOnLoad(this.gameObject);

        Load();//Load all info on the save state
        Debug.Log(Helper.Serialize<SaveState>(state));
    }

    // Save the whole state of this saveState script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));

    }

    //Load Saved PlayerPrefs
    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            
            state = new SaveState();
            Save();

        }
    }

    //Reset the whole save file
    public void ResetSave()
    {
        //AudioManager.Instance.firstStart = true;//Make it to True when Player Resets Game...
        PlayerPrefs.DeleteKey("save");
        //AudioManager.Instance.UISound();//Play UI Sound...
        SceneManager.LoadScene(0);
    }
}

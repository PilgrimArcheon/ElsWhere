using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDataUI : MonoBehaviour
{
    public Sprite[] popUpSprites;
    public Text info;
    public Image activePopUp;
    
    public void PopUpObjects(string popUpInfo)
    {
        for (int i = 0; i < popUpSprites.Length; i++)
        {
            if(popUpSprites[i].name == popUpInfo)
            {
                activePopUp.sprite = popUpSprites[i];
                info.text = popUpInfo;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrigger : MonoBehaviour
{
    public void UncoveredRoots()
    {
        if(Inventory.Instance.popUpItem != null)
        {
            Invoke("PopUp", 7.5f);
        }
    }

    void PopUp()
    {
        Inventory.Instance.popUpItem.PopUp();
        FindObjectOfType<PopUpDataUI>().PopUpObjects(Inventory.Instance.popUpItem.popUpInfo);
        FindObjectOfType<ThirdPersonController>()._isInteracting = true;
        Inventory.Instance.popUpItem = null;
        gameObject.SetActive(false);
    }
}
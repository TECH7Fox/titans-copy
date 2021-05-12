using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreePanel : MonoBehaviour
{
    public void TogglePanel()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

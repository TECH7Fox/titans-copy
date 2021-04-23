using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoBehaviour
{
    public bool unlocked = false;
    public Color lockedColor;
    public Color unlockedColor;

    // Start is called before the first frame update
    private void Awake()
    {
        if (unlocked)
        {
            GetComponent<Image>().color = unlockedColor;
        } else
        {
            GetComponent<Image>().color = lockedColor;
        }
    }

    public void Unlock()
    {
        unlocked = true;
        GetComponent<Image>().color = unlockedColor;
    }
}

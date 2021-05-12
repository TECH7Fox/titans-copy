using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class techTreeButton : MonoBehaviour
{
    public string researchString;
    public Player player;

    private bool mouseover = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Info").gameObject.SetActive(false);
        transform.Find("Progress").gameObject.GetComponent<Slider>().value = player.unlocked[researchString] / 100;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("RESEARCH: " + player.research);

        if (mouseover)
            transform.Find("Info").transform.position = new Vector2(transform.Find("Info").transform.position.x, Input.mousePosition.y - 63);

        if (player.currentResearch == researchString)
            transform.Find("Progress").gameObject.GetComponent<Slider>().value = player.unlocked[researchString] / 100;
    }

    public void select()
    {
        player.currentResearch = researchString;
    }

    public void show()
    {
        Debug.Log("ENTER");
        mouseover = true;
        transform.Find("Info").gameObject.SetActive(true);
    }

    public void hide()
    {
        mouseover = false;
        transform.Find("Info").gameObject.SetActive(false);
    }
}

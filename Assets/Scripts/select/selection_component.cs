using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EntityType
{
    Unit,
    Building
}

public class selection_component : MonoBehaviour
{
    private Outline outline;
    private GameObject canvas;

    public float progress = 0f;
    
    private Image progressBar;

    public GameObject barsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        barsCanvas = Resources.Load<GameObject>("Bars Canvas");

        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 5f; 

        if (gameObject.tag == "Building")
        {
            outline.OutlineColor = GetComponent<Building>().player.color;
        }
        else
        {
            outline.OutlineColor = GetComponent<EntityController>().player.color; // FIX THIS SHIT WITH BASE CLASS FOR ENTITY'S (BUILDINGS AND UNITS)
        } // CAN OVERRIDE CERTAIN FUNCTIONS OF ENTITY CLASS (MOVE, SET TARGET ETC.)

        canvas = Instantiate(barsCanvas);
        canvas.transform.SetParent(transform);
        canvas.transform.position = this.transform.position + transform.up * 2;
        progressBar = canvas.transform.Find("Progress Bar/Mask/Fill").gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        canvas.transform.forward = Camera.main.transform.forward;
        progressBar.fillAmount = progress;
    }

    private void OnDestroy()
    {
        Destroy(canvas);
        Destroy(outline);
    }
}

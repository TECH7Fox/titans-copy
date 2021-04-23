using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selection_component : MonoBehaviour
{
    private Outline outline;
    private GameObject canvas;

    private Image progressBar;
    private Image healthBar;

    public GameObject barsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        barsCanvas = Resources.Load<GameObject>("Bars Canvas");

        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 5f; 

        outline.OutlineColor = GetComponent<Entity>().player.color;

        canvas = Instantiate(barsCanvas);
        canvas.transform.SetParent(transform);
        canvas.transform.position = transform.position + transform.up * 2;
        progressBar = canvas.transform.Find("Progress Bar/Mask/Fill").gameObject.GetComponent<Image>();
        healthBar = canvas.transform.Find("Health Bar/Mask/Fill").gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        canvas.transform.forward = Camera.main.transform.forward;
        progressBar.fillAmount = GetComponent<Entity>().progress / 100;
        healthBar.fillAmount = GetComponent<Entity>().health / 100;
    }

    private void OnDestroy()
    {
        Destroy(canvas);
        Destroy(outline);
    }
}

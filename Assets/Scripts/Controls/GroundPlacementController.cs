using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour
{

    public Player player;

    [SerializeField]
    private float distance = 500f;

    [SerializeField]
    private LayerMask layerMask;

    public Material buildableMaterial;
    public Material nonBuildingMaterial;

    private GameObject currentPlaceableObject;
    private MeshRenderer meshRenderer;
    private Material oldMaterial;
    private float rotation;
    private ResourceType resource;
    private int cost;

    private void Update()
    {
        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            Rotate();
            ReleaseIfClicked();
            CancelIfClicked();
            //overlapbox or checkbox ( get sizes of currentPlaceableObject)
        }
    }

    public void PlaceObject(GameObject prefeb)
    {
        if (currentPlaceableObject != null)
        {
            Destroy(currentPlaceableObject);
        }
        else
        {
            currentPlaceableObject = Instantiate(prefeb);
            this.resource = currentPlaceableObject.GetComponent<Building>().resource;
            this.cost = currentPlaceableObject.GetComponent<Building>().cost;
            meshRenderer = currentPlaceableObject.GetComponent<MeshRenderer>();
            oldMaterial = meshRenderer.material;
            meshRenderer.material = buildableMaterial; // change
        }
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotation -= 10;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rotation += 10;
        }
        currentPlaceableObject.transform.Rotate(Vector3.up, rotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.HasResource(resource, cost))
            {
                player.RemoveResource(resource, cost);
                meshRenderer.material = oldMaterial;
                currentPlaceableObject.GetComponent<Building>().Built(player);
                currentPlaceableObject = null;
            }
        }
    }

    private void CancelIfClicked()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentPlaceableObject);
            currentPlaceableObject = null;
        }
    }
}
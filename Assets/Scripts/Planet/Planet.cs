﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPWAsset;

public class Planet : MonoBehaviour {

    [Range(2,256)]
    public int resolution = 10;
    public float waterHeight = 1;
    public int layerNumber;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    MeshFilter[] waterMeshFilters;
    TerrainFace[] terrainFaces;
    WaterFace[] waterFaces;

    public LayerMask layerMask;

    public GameObject treePrefeb;
    public int treesNumber;
    private GameObject[] instTrees;
    private GameObject trees;

    public GameObject stonePrefeb;
    public int stonesNumber;
    private GameObject[] instStones;
    private GameObject stones;

	void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshFilters[i].gameObject.layer = layerNumber;
                meshFilters[i].gameObject.AddComponent<MeshCollider>();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }

        if (waterMeshFilters == null || waterMeshFilters.Length == 0)
        {
            waterMeshFilters = new MeshFilter[6];
        }
        waterFaces = new WaterFace[6];

        for (int i = 0; i < 6; i++)
        {
            if (waterMeshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("waterMesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                waterMeshFilters[i] = meshObj.AddComponent<MeshFilter>();
                waterMeshFilters[i].sharedMesh = new Mesh();
            }

            waterFaces[i] = new WaterFace(waterMeshFilters[i].sharedMesh, resolution, waterHeight, directions[i]);
            
            DestroyImmediate(waterMeshFilters[i].gameObject.GetComponent<LowPolyWaterScript>());
            waterMeshFilters[i].gameObject.AddComponent<LowPolyWaterScript>();
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateWater();
        GenerateColours();
        GenerateTrees();
        GenerateStones();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
            GenerateWater();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    public void GenerateTrees()
    {
        if (trees == null)
        {
            trees = new GameObject("trees");
            trees.transform.parent = transform;
        }

        if (instTrees != null)
        {
            for (int i = 0; i < instTrees.Length; i++)
            {
                if (instTrees[i] != null)
                    DestroyImmediate(instTrees[i]);
            }
        }

        instTrees = new GameObject[treesNumber];

        for (int i = 0; i < treesNumber; i++)
        {
            Vector3 direction = Random.onUnitSphere * shapeSettings.planetRadius * 2;
            Physics.Raycast(transform.position + direction, -direction, out RaycastHit hit, 1000, layerMask);
            if (Vector3.Distance(hit.point, transform.position) > waterHeight)
            {
                instTrees[i] = Instantiate(treePrefeb);
                instTrees[i].GetComponent<Outline>().enabled = false;
                instTrees[i].transform.parent = trees.transform;
                instTrees[i].transform.position = hit.point;
                instTrees[i].transform.up = hit.normal;
            }
        }
    }

    public void GenerateStones()
    {
        if (stones == null)
        {
            stones = new GameObject("stones");
            stones.transform.parent = transform;
        }

        if (instStones != null)
        {
            for (int i = 0; i < instStones.Length; i++)
            {
                if (instStones[i] != null)
                    DestroyImmediate(instStones[i]);
            }
        }

        instStones = new GameObject[stonesNumber];

        for (int i = 0; i < stonesNumber; i++)
        {
            Vector3 direction = Random.onUnitSphere * shapeSettings.planetRadius * 2;
            Physics.Raycast(transform.position + direction, -direction, out RaycastHit hit, 1000, layerMask);
            if (Vector3.Distance(hit.point, transform.position) > waterHeight)
            {
                instStones[i] = Instantiate(stonePrefeb);
                instStones[i].GetComponent<Outline>().enabled = false;
                instStones[i].transform.parent = stones.transform;
                instStones[i].transform.position = hit.point;
                instStones[i].transform.up = hit.normal;
            }
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateWater()
    {
        for (int i = 0; i < 6; i++)
        {
            if (waterMeshFilters[i].gameObject.activeSelf)
            {
                waterFaces[i].ConstructMesh();
            }
        }
    }

    void GenerateColours()
    {
        colourGenerator.UpdateColours();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }

            if (waterMeshFilters[i].gameObject.activeSelf)
            {
                waterFaces[i].UpdateUVs();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> levelComponents;
    public List<float> xPlacements;
    public List<float> yPlacements;
    public List<int> rightSideUp;

    public GameObject goalPrefab;
    public GameObject levelParent;
    public GameObject goalParent;
    public GameObject flagAreaPrefab;
    public GameObject triangleOutlinePrefab;

    public int teleporterCount;
    public GameObject teleporterPrefab;
    public GameObject teleporterParent;
    public GameObject itemAreaPrefab;


    public int goalCount;

    public GameObject MenuManager;

    void Start()
    {
        MenuManager = GameObject.Find("MenuManager");
        goalCount = MenuManager.GetComponent<MenuManager>().goalCount;
        var rand = new System.Random(MenuManager.GetComponent<MenuManager>().GetSeed());
        //var rand = new System.Random(3);
        Quaternion flipQuaternion = Quaternion.identity;
        flipQuaternion.eulerAngles = new Vector3(0, 0, 180);

        for (int i = 0; i < 24; i++)
        {
            int randComponentNumber = rand.Next(levelComponents.Count);

            if (rightSideUp.Contains(i))
            {
                GameObject newComponent = Instantiate(levelComponents[randComponentNumber], new Vector3(xPlacements[i], yPlacements[i], 0), randomRightSideUpRotation(rand), levelParent.transform) as GameObject;
                //GameObject newComponent = Instantiate(levelComponents[randComponentNumber], new Vector3(xPlacements[i], yPlacements[i], 0), randomRightSideUpRotation(rand), levelParent.transform) as GameObject;

            }
            else
            {
                GameObject newComponent = Instantiate(levelComponents[randComponentNumber], new Vector3(xPlacements[i], yPlacements[i], 0), randomUpsideDownRotation(rand), levelParent.transform) as GameObject;
                //GameObject newComponent = Instantiate(levelComponents[randComponentNumber], new Vector3(xPlacements[i], yPlacements[i], 0), randomUpsideDownRotation(rand), levelParent.transform) as GameObject;
            }
        }

        List<GameObject> flagAreaList = new List<GameObject>();

        for (int i = 0; i < levelParent.transform.childCount; i++)
        {
            Transform flagAreaHolder = levelParent.transform.GetChild(i).Find("FlagAreaHolder");
            if (flagAreaHolder == null)
            {
                continue;
            }
            for (int j = 0; j < flagAreaHolder.childCount; j++)
            {
                flagAreaList.Add(flagAreaHolder.GetChild(j).gameObject);
            }

        }

        List<GameObject> itemAreaList = new List<GameObject>();

        for (int i = 0; i < levelParent.transform.childCount; i++)
        {
            Transform itemAreaHolder = levelParent.transform.GetChild(i).Find("ItemAreaHolder");
            if (itemAreaHolder == null)
            {
                continue;
            }
            for (int j = 0; j < itemAreaHolder.childCount; j++)
            {
                itemAreaList.Add(itemAreaHolder.GetChild(j).gameObject);
            }

        }


        List<GameObject> flagAreaListBackup = new List<GameObject>();
        for (int i = 0; i < flagAreaList.Count; i++)
        {
            flagAreaListBackup.Add(flagAreaList[i]);
        }

        //I think list is mutable so the backup is changing when the original changes

        for (int i = 0; i < goalCount; i++)
        {
            if (flagAreaList.Count == 0)
            {
                flagAreaList = flagAreaListBackup;
            }
            int randomIndex = rand.Next(flagAreaList.Count);
            Transform currentFlagAreaTransform = flagAreaList[randomIndex].transform;
            Instantiate(goalPrefab, RandomLocationInRectangularTransform(currentFlagAreaTransform,rand), 
                Quaternion.identity, goalParent.transform);
            flagAreaList.RemoveAt(randomIndex);
        }


        for (int i = 0; i < teleporterCount; i++)
        {
            int randomIndex = rand.Next(itemAreaList.Count);
            Transform currentItemAreaTransform = itemAreaList[randomIndex].transform;
            Instantiate(teleporterPrefab, RandomLocationInRectangularTransform(currentItemAreaTransform,rand),
                Quaternion.identity, teleporterParent.transform);
            itemAreaList.RemoveAt(randomIndex);
        }

        for (int i = 0; i < teleporterParent.transform.childCount; i++)
        {
            int randomIndex = rand.Next(itemAreaList.Count);
            Transform currentItemAreaTransform = itemAreaList[randomIndex].transform;
            teleporterParent.transform.GetChild(i).gameObject.GetComponent<Teleporter>().GenerateExit(RandomLocationInRectangularTransform(currentItemAreaTransform, rand));
            itemAreaList.RemoveAt(randomIndex);
        }

    }

    Vector3 RandomLocationInRectangularTransform(Transform rectangularTransform, System.Random rand)
    {
        float xRandFactor = (float)rand.NextDouble() - 0.5f;
        float yRandFactor = (float)rand.NextDouble() - 0.5f;
        Vector3 outputVector = new Vector3(rectangularTransform.position.x + rectangularTransform.localScale.x * xRandFactor,
                rectangularTransform.position.y + rectangularTransform.localScale.y * yRandFactor, 0);
        return outputVector;
    }

    Quaternion randomRightSideUpRotation(System.Random rand)
    {
        Quaternion OneTwentyDegreeRotation = Quaternion.identity;
        OneTwentyDegreeRotation.eulerAngles = new Vector3(0, 0, 120);
        Quaternion TwoFortyDegreeRotation = Quaternion.identity;
        TwoFortyDegreeRotation.eulerAngles = new Vector3(0, 0, 240);

        return Quaternion.identity;                                     //THIS LINE FORCES NO ROTATION
        /*
        switch (rand.Next(0,2))
        {
            case 1:
                return Quaternion.identity;
            case 2:
                return OneTwentyDegreeRotation;
            case 3:
                return TwoFortyDegreeRotation;
            default:
                return TwoFortyDegreeRotation;
        }
        */
    }

    Quaternion randomUpsideDownRotation(System.Random rand)
    {
        Quaternion SixtyDegreeRotation = Quaternion.identity;        
        SixtyDegreeRotation.eulerAngles = new Vector3(0, 0, 60);
        Quaternion OneEightyDegreeRotation = Quaternion.identity;
        OneEightyDegreeRotation.eulerAngles = new Vector3(0, 0, 180);
        Quaternion ThreeHundredDegreeRotation = Quaternion.identity;
        ThreeHundredDegreeRotation.eulerAngles = new Vector3(0, 0, 300);

        return OneEightyDegreeRotation;                                 //THIS LINE FORCES NO ROTATION
        /*
        switch (rand.Next(0, 2))
        {
            case 1:
                return SixtyDegreeRotation;
            case 2:
                return OneEightyDegreeRotation;
            case 3:
                return ThreeHundredDegreeRotation;
            default:
                return ThreeHundredDegreeRotation;
        }
        */
    }
}

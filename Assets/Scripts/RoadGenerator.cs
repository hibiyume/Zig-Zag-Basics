using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roadPart;
    [SerializeField] private GameObject crystal;

    [SerializeField] private Transform roadFolder;
    [SerializeField] private Transform roadStartPoint;

    [SerializeField] private CharController charController;
    private float charMovementSpeed;
    private float crystalChance = 0.15f;

    private Vector3 previousLocalPosition;
    private float spawningSpeedInSeconds = 0.4f; //TODO objects are destroyed and spawned too fast
    private float destroyDelayInSeconds = 15f;

    private void Start()
    {
        previousLocalPosition = roadStartPoint.localPosition;
        charMovementSpeed = charController.GetMovementSpeed();
        spawningSpeedInSeconds = charMovementSpeed / 12.5f;
        
        Invoke("InstantiateNewRoadPart", spawningSpeedInSeconds);
    }

    private void InstantiateNewRoadPart()
    {
        bool isRight = Mathf.RoundToInt(Random.Range(0.0f, 1.0f)) == 1;

        Vector3 newPos;
        if (isRight)
            newPos = previousLocalPosition + new Vector3(0f, 0f, 1f);
        else
            newPos = previousLocalPosition + new Vector3(-1f, 0f, 0f);
            
        // Spawning Road Part
        GameObject g = Instantiate(roadPart, roadFolder, false);
        g.transform.localPosition = newPos;
        previousLocalPosition = newPos;

        // Spawning Crystal
        if (Random.Range(0f, 1f) < crystalChance)
        {
            GameObject c = Instantiate(crystal, g.transform.GetChild(0), false);
            c.transform.localPosition = crystal.transform.localPosition;
            c.transform.localRotation = new Quaternion(c.transform.localRotation.x, Random.Range(0f, 180f),
                c.transform.localRotation.z, c.transform.localRotation.w);
        }
        
        g.transform.GetChild(0).GetComponent<Animator>().Play("Road_Part_Spawning");
        
        Invoke("InstantiateNewRoadPart", spawningSpeedInSeconds);
        Destroy(g, destroyDelayInSeconds);
    }
}

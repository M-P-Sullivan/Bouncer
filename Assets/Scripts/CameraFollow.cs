using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BouncerTools;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public Camera mainCamera;
    public float superSonicSpeed;
    public int defaultPlayCameraSize;
    public int superSonicCameraSize;
    public int minCameraSize;
    public int maxCameraSize;
    public float spotterZoomSpeed;
    public float targetOffsetScalar;
    private Vector3 currentOffsetVector = new Vector3(0,0,0);
    public float cameraDriftStepSize;
    public GameObject GameManager;
    public GameObject GuidanceController;

    public GameObject GoalsParent;
    private List<GameObject> goals = new List<GameObject>();

    private int mode = 0;

    public int UIChangeCameraSize;
    public float UISizeMultiplier;
    public bool isUIBig = false;


    void FixedUpdate()
    {
        switch (GameManager.GetComponent<GameManager>().CurrentGameMode)
        {
            case GameMode.Spotting:
                if (Input.GetAxis("Zoom") == 1 & mainCamera.orthographicSize > minCameraSize)
                {
                    MultiplicativeZoomIn(spotterZoomSpeed);
                }
                if (Input.GetAxis("Zoom") == -1 & mainCamera.orthographicSize < maxCameraSize)
                {
                    MultiplicativeZoomOut(spotterZoomSpeed);
                }
                if (mainCamera.orthographicSize > UIChangeCameraSize && !isUIBig)
                {
                    MakeUIBig();
                }
                if (mainCamera.orthographicSize < UIChangeCameraSize && isUIBig)
                {
                    MakeUISmall();
                }
                break;
            case GameMode.Ready:
                if (isUIBig)
                {
                    MakeUISmall();
                }
                break;
            case GameMode.Playing:
                if (isUIBig)
                {
                    MakeUISmall();
                }
                if (player.GetComponent<Rigidbody2D>().velocity.magnitude > superSonicSpeed & mainCamera.orthographicSize < superSonicCameraSize)
                {
                    zoomOut(5);
                }
                if (player.GetComponent<Rigidbody2D>().velocity.magnitude < (superSonicSpeed * 0.75f) & mainCamera.orthographicSize > defaultPlayCameraSize)
                {
                    zoomIn(10);
                }
                break;
            default:
                break;
        }
    }

    private void LateUpdate()                                                                                   //LateUpdate is a built-in function that is called every frame *after* all Update functions have been called
    {
        switch (GameManager.GetComponent<GameManager>().CurrentGameMode)
        {
            case GameMode.Spotting:
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
                break;
            case GameMode.Playing:
                transform.position = OffsetCameraPosition();
                break;
            default:
                break;
        }
    }

    public void MakeUIBig()
    {
        ChangeUISizeHelper(UISizeMultiplier, GoalsParent);
        ChangeUISizeHelper(UISizeMultiplier, GuidanceController);
        isUIBig = true;
    }


    public void MakeUISmall()
    {
        ChangeUISizeHelper(1 / UISizeMultiplier, GoalsParent);
        ChangeUISizeHelper(1 / UISizeMultiplier, GuidanceController);
        isUIBig = false;
    }

    private void ChangeUISizeHelper(float multiplier, GameObject ParentOfObjects)
    {
        for (int i = 0; i < ParentOfObjects.transform.childCount; i++)
        {
            ParentOfObjects.transform.GetChild(i).localScale = ParentOfObjects.transform.GetChild(i).localScale * multiplier;
        }
    }

    public void setPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public void zoomOut(int zoomSpeed)
    {
        mainCamera.orthographicSize += 0.01f * zoomSpeed;
    }

    public void zoomIn(int zoomSpeed)
    {
        mainCamera.orthographicSize -= 0.01f * zoomSpeed;
    }

    public void MultiplicativeZoomIn(float zoomSpeed)
    {
        mainCamera.orthographicSize = mainCamera.orthographicSize/zoomSpeed;
    }

    public void MultiplicativeZoomOut(float zoomSpeed)
    {
        mainCamera.orthographicSize = mainCamera.orthographicSize * zoomSpeed;
    }

    public float getZoom()
    {
        return mainCamera.orthographicSize;
    }

    public int getMode()
    {
        return mode;
    }

    public void setMode(int i)
    {
        mode = i;
    }


    private Vector3 OffsetCameraPosition()
    {
        Vector3 targetOffsetVector = player.GetComponent<Rigidbody2D>().velocity.normalized * targetOffsetScalar;
        Vector3 targetPosition = player.transform.position + targetOffsetVector;
        Vector3 currentPosition = player.transform.position + currentOffsetVector;
        Vector3 stepDirectionVector = targetPosition - currentPosition;
        Vector3 stepVector = stepDirectionVector * cameraDriftStepSize;
        currentPosition = currentPosition + stepVector;
        currentOffsetVector = currentPosition - player.transform.position;
        return new Vector3(currentPosition.x, currentPosition.y, -10);
    }

}

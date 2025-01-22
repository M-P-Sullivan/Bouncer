using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_WaypointPointer : MonoBehaviour
{
    private RectTransform pointerRectTransform;

    public int xAxisSize;
    public int yAxisSize;
    public int negXAxisSize;
    public int negYAxisSize;
    public float xScreenLength;
    public float yScreenLength;


    public GameObject guidanceController;
    public GameObject wayPointPointer;
    public GameObject Player;

    private void Start()
    {
        //Vector2 vectA = new Vector2(10, 10);
        //Vector2 vectB = new Vector2(0, 10);
    }

    private void Awake()
    {
        //pointerRectTransform = transform.Find("WaypointPointer").GetComponent<RectTransform>();
        pointerRectTransform = wayPointPointer.GetComponent<RectTransform>();

    }

    void Update()
    {
        if (guidanceController.transform.childCount == 0)
        {
            wayPointPointer.SetActive(false);
            return;
        }
        
        Vector3 toPosition = guidanceController.transform.GetChild(0).transform.position;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 distance = toPosition - fromPosition;
        Vector3 dir = distance.normalized;
        float angle = angleOfVector2(new Vector2(dir.x, dir.y));
        pointerRectTransform.localEulerAngles = new Vector3(0, 0,  angle-90);

        if (Math.Abs(distance.x) < xScreenLength & Math.Abs(distance.y) < yScreenLength)
        {
            wayPointPointer.SetActive(false);
        }
        else
        {
            wayPointPointer.SetActive(true);
        }


        //pointerRectTransform.anchoredPosition = new Vector2(dir.x*5, dir.y*5);

        
        if (angleOfXY(xAxisSize,yAxisSize) < angle & angle < angleOfXY(negXAxisSize,yAxisSize))
        {
            pointerRectTransform.anchoredPosition = new Vector2(distance.x / (distance.y / yAxisSize), yAxisSize);
        }
        if (angleOfXY(negXAxisSize, yAxisSize) < angle & angle < angleOfXY(negXAxisSize, negYAxisSize))
        {
            pointerRectTransform.anchoredPosition = new Vector2(negXAxisSize, distance.y / (distance.x / negXAxisSize));
        }
        if (angleOfXY(negXAxisSize, negYAxisSize) < angle & angle < angleOfXY(xAxisSize, negYAxisSize))
        {
            pointerRectTransform.anchoredPosition = new Vector2(distance.x / (distance.y / negYAxisSize), negYAxisSize);
        }
        if (angleOfXY(xAxisSize, negYAxisSize) < angle || angle < angleOfXY(xAxisSize, yAxisSize))
        {
            pointerRectTransform.anchoredPosition = new Vector2(xAxisSize, distance.y / (distance.x / xAxisSize));
        }
        
    }

    public float angleOfVector2(Vector2 vectA)
    {
        float angle = Mathf.Atan2(vectA.y, vectA.x);
        angle =  angle * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }

    public float angleOfXY(float x, float y)
    {
        return angleOfVector2(new Vector2(x, y));
    }
}

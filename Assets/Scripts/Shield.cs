using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    float zAxis = 0f;
    public float shieldDistance;
    Vector3 mousePosition;
    public Transform ThePlayer;

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = zAxis;

        transform.position = ThePlayer.position + (mousePosition - ThePlayer.position).normalized * shieldDistance;
        float angle = AngleToRotate(ThePlayer, shieldDistance);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private float AngleToRotate(Transform ThePlayer, float shieldDistance)
    {
        float angle = Mathf.Asin((transform.position.x - ThePlayer.position.x) / shieldDistance);
        angle = angle * Mathf.Rad2Deg;
        if (transform.position.y > ThePlayer.position.y)
        {
            angle = -(angle + 180);
        }
        return angle;
    }
}

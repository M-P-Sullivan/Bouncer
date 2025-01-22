using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BouncerTools;

public class Spotter : MonoBehaviour
{
    public float defaultSpeed;
    public GameObject myCamera;
    public GameObject guidanceController;
    public GameObject targetPrefab;
    public List<GameObject> Items = new List<GameObject>();
    public GameObject ItemInHand;
    public GameObject SpotterUI;
    public GameObject SpotterObjects;
    public GameObject HandImage;
    private Camera mainCam;
    public GameObject UnitSquare;

    private Rigidbody2D rb2d;
    public float moveHorizontal;
    public float moveVertical;
    public float defaultZoom;
    public float activeSpeed;

    public float objectRotationAngle = 0.0f;
    public float scrollSpeed = 1.0f;
    public float rotationAngleSize = 45.0f;
    private Quaternion objectRotationQuaternion = Quaternion.identity;

    public List<GameObject> mouseOverObjects = new List<GameObject>();

    private Vector2 inputForce;
    private Vector2 movement;

    public int maxTargetCount;
    public int currentTargetCount;

    public GameObject TeleporterLine;
    public GameObject TargetLine;
    public GameObject TeleporterParent;
    public GameObject Teleporter;
    public GameObject TeleporterExit;
    public GameObject TeleporterExitImage;
    private GameObject JustPlacedTeleporter = null;
    public float lineWidth;
    public GameObject LineParent;

    public bool isMouseOverUI;
    public bool isMouseOverSprite;
    public bool isLineDrawn = false;

    void Start()
    {
        mainCam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
        defaultZoom = myCamera.gameObject.GetComponent<CameraFollow>().getZoom();
        currentTargetCount = maxTargetCount;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        if (ItemInHand != null)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            objectRotationAngle += scrollInput * scrollSpeed;
            if (objectRotationAngle > 360)
            {
                objectRotationAngle -= 360;
            }
            float roundedRotationAngle = Mathf.Floor(objectRotationAngle / rotationAngleSize) * rotationAngleSize;
            objectRotationQuaternion.eulerAngles = new Vector3(0,0,roundedRotationAngle);

            if (HandImage != null)
            {
                HandImage.transform.position = mousePos;
                HandImage.transform.rotation = objectRotationQuaternion;
            }
        }

        if (Input.GetButtonDown("LeftMouse"))
        {
            if (mouseOverObjects.Count == 1 && mouseOverObjects[0].tag == "Cancel")
            {
                ReleaseItem();
            }
            else if (mouseOverObjects.Count == 1 && mouseOverObjects[0].tag == "InventoryItem")
            {
                if (HandImage != null)
                {
                    Destroy(HandImage);
                }
                TakeInHand(mouseOverObjects[0].transform.parent.gameObject);
            }
            else if (ItemInHand != null)
            {
                if (JustPlacedTeleporter == null)
                {
                    PlaceItem(ItemInHand);
                }
                else
                {
                    PlaceTeleporterExit();
                }
            }
        }
        if (Input.GetButtonDown("RightMouse") && ItemInHand != null)
        {
            ReleaseItem();
        }

        if (mouseOverObjects.Count > 0 && !isLineDrawn)
        {
            GameObject mouseObject = mouseOverObjects[0];
            if (mouseObject.tag == "Teleporter" && mouseObject.transform.childCount > 0)
            {
                DrawLine(mouseObject.transform, mouseObject.transform.GetChild(0), TeleporterLine);
                isLineDrawn = true;
            }
            if (mouseObject.tag == "TeleporterExit")
            {
                DrawLine(mouseObject.transform, mouseObject.transform.parent, TeleporterLine);
                isLineDrawn = true;
            }
            if (mouseObject.tag == "Targets")
            {
                for (int i = 0; i < guidanceController.transform.childCount - 1; i++)
                {
                    DrawLine(guidanceController.transform.GetChild(i), guidanceController.transform.GetChild(i + 1), TargetLine);
                }
                isLineDrawn = true;
            }
        }
        if (mouseOverObjects.Count == 0)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Line");

            foreach (GameObject obj in objectsWithTag)
            {
                Destroy(obj);
            }
            isLineDrawn = false;
        }
    }

    void FixedUpdate()
    {
        activeSpeed = defaultSpeed * myCamera.gameObject.GetComponent<CameraFollow>().getZoom() / defaultZoom;

        moveHorizontal = Input.GetAxis("Horizontal");                                           
        moveVertical = Input.GetAxis("Vertical");                                               

        movement = new Vector2(moveHorizontal, moveVertical);                                   

        inputForce = movement * activeSpeed;

        rb2d.AddForce(new Vector2(-rb2d.velocity.x, -rb2d.velocity.y), ForceMode2D.Impulse);
        rb2d.AddForce(inputForce, ForceMode2D.Impulse);

        rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, activeSpeed);

        if (transform.position.y < -225)
        {
            transform.position = new Vector3(transform.position.x, -225, transform.position.z);
        }
        if (transform.position.y > 225)
        {
            transform.position = new Vector3(transform.position.x, 225, transform.position.z);
        }

        if (transform.position.x < -250)
        {
            transform.position = new Vector3(-250, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 250)
        {
            transform.position = new Vector3(250, transform.position.y, transform.position.z);
        }
    }

    public void TakeTeleExitInHand()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        ItemInHand = TeleporterExit;
        HandImage = Instantiate(TeleporterExitImage, mainCam.ScreenToWorldPoint(mousePos), Quaternion.identity, SpotterUI.transform);
        HandImage.SetActive(true);
    }

    public void TakeInHand(GameObject ItemForHand)
    {
        if (ItemForHand.GetComponent<SpotterItems>().itemsInInventory < 1)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        ItemInHand = ItemForHand;
        HandImage = Instantiate(ItemInHand.GetComponent<SpotterItems>().ItemImage, mainCam.ScreenToWorldPoint(mousePos), Quaternion.identity,SpotterUI.transform);
        HandImage.SetActive(true);
    }

    public void ReleaseItem()
    {
        ItemInHand = null;
        Destroy(HandImage);
        objectRotationAngle = 0;
        if (JustPlacedTeleporter != null)
        {
            Destroy(JustPlacedTeleporter);
            Teleporter.GetComponent<SpotterItems>().itemsInInventory += 1;

        }
    }

    public void PlaceTeleporterExit()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        GameObject NewExit = Instantiate(TeleporterExit, mainCam.ScreenToWorldPoint(mousePos), objectRotationQuaternion, JustPlacedTeleporter.transform);
        Destroy(HandImage);
        JustPlacedTeleporter = null;
    }
    
    public void PlaceItem(GameObject Item)
    {
        if (Item.GetComponent<SpotterItems>().itemsInInventory < 1)
        {
            return;
        }
        Item.GetComponent<SpotterItems>().itemsInInventory -= 1;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        GameObject newItem;
        newItem = Instantiate(Item.GetComponent<SpotterItems>().Object, mainCam.ScreenToWorldPoint(mousePos), objectRotationQuaternion);
        if (newItem.tag == "Targets")
        {
            newItem.transform.parent = guidanceController.transform;
            if (myCamera.GetComponent<CameraFollow>().isUIBig)
            {
                newItem.transform.localScale = newItem.transform.localScale * myCamera.GetComponent<CameraFollow>().UISizeMultiplier;
            }
        }
        else if (newItem.tag == "Teleporter")
        {
            newItem.transform.parent = TeleporterParent.transform;
            JustPlacedTeleporter = newItem;
            Destroy(HandImage);
            TakeTeleExitInHand();
            return;
        }
        else
        {
            newItem.transform.parent = SpotterObjects.transform;
        }
        if (Item.GetComponent<SpotterItems>().itemsInInventory < 1)
        {
            Destroy(HandImage);
        }
    }

    bool IsMouseOverObject(GameObject objectToCheck)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Perform a raycast from the mouse position
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // Check if the raycast hit the object
        if (hit.collider != null && hit.collider.gameObject == objectToCheck)
        {
            return true;
        }

        return false;
    }

    public void AddMouseOverObject(GameObject NewObject)
    {
        mouseOverObjects.Add(NewObject);
    }

    public void RemoveMouseOverObject(GameObject ObjectToRemove)
    {
        mouseOverObjects.Remove(ObjectToRemove);
    }

    public void DrawLine(Transform transformA, Transform transformB, GameObject WhichLine)
    {
        Vector3 midpoint = (transformB.position + transformA.position)/2.0f;
        float distance = Vector3.Distance(transformA.position, transformB.position);
        GameObject NewLine = Instantiate(WhichLine, midpoint, Quaternion.identity, LineParent.transform);
        NewLine.transform.LookAt(transformB);
        NewLine.transform.Rotate(0.0f, 90.0f, 0.0f);
        NewLine.transform.localScale = new Vector3(NewLine.transform.localScale.x * distance, lineWidth, 1);
    }
}

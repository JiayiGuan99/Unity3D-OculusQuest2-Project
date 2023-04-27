using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Dropdown menu;
    public GameObject TV;
    public GameObject cabinet;
    public GameObject chair;
    public GameObject desk;
    public GameObject locker;
    public GameObject room;
    public GameObject whiteboard;

    private GameObject currentObject;
    private bool placingObject = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Update()
    {
        // if right controller's index trigger is pressed, and object is not placed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && !placingObject)
        {
            SpawnObject();
        }
        else if (placingObject)
        {
            MoveObject();
        }
    }

    private void SpawnObject()
    {
        switch (menu.value)
        {
            case 0:
                currentObject = Instantiate(TV);
                break;
            case 1:
                currentObject = Instantiate(cabinet);
                break;
            case 2:
                currentObject = Instantiate(chair);
                break;
            case 3:
                currentObject = Instantiate(desk);
                break;
            case 4:
                currentObject = Instantiate(locker);
                break;
            case 5:
                currentObject = Instantiate(room);
                break;
            case 6:
                currentObject = Instantiate(whiteboard);
                break;
            default:
                break;
        }

        if (currentObject != null)
        {
            placingObject = true;
            currentObject.GetComponent<Rigidbody>().useGravity = false;
            currentObject.GetComponent<Rigidbody>().isKinematic = true;
            initialPosition = transform.position;
            initialRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        }
    }

    private void MoveObject()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = initialPosition.y;
        currentObject.transform.position = newPosition;
        currentObject.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Quaternion.Inverse(initialRotation);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            placingObject = false;
            currentObject.GetComponent<Rigidbody>().useGravity = true;
            currentObject.GetComponent<Rigidbody>().isKinematic = false;
            currentObject = null;
        }
    }
}
public class Spawner : MonoBehaviour
{
    public Dropdown menu;
    public GameObject TV;
    public GameObject cabinet;
    public GameObject chair;
    public GameObject desk;
    public GameObject locker;
    public GameObject whiteboard;
    public GameObject[] items = {TV, cabinet, chair, desk, locker, whiteboard};
    public GameObject rayPrefab;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 100.0f;

    private int dropdownIndex = -1;
    private GameObject currentObject;
    private bool menuOpen = false;
    private Dropdown menu;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
    }

    void Update()
    {
        // Open or close the menu
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Dropdown>())
                {
                    dropdown = hit.transform.gameObject.GetComponent<Dropdown>();
                    menuOpen = true;
                }
            }
        }

        if (dropdown == menu && menuOpen)
        {
            // Close the menu
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                menuOpen = false;
            }

            // Change selected item with dropdown menu
            dropdownIndex = dropdown.value;
        }
        else if(dropdownIndex != -1 &&  menuOpen == false)
        {
            // Spawn the item
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Vector3 spawnPos = GetRaycastHitPoint(OVRInput.Controller.RTouch);

                // Instantiate the selected item at the hit point
                currentObject = Instantiate(items[dropdownIndex], spawnPos, Quaternion.identity);

                // Add physics properties
                currentObject.GetComponent<Rigidbody>().useGravity = true;
                currentObject.GetComponent<Rigidbody>().isKinematic = false;
                currentObject = null;

                // Add collider
                currentObject.AddComponent<BoxCollider>();

                // Destroy the ray indicator
                Destroy(rayInstance);
            }

            // Move the item
            if (currentObject != null)
            {
                currentObject.GetComponent<Rigidbody>().useGravity = false;
                currentObject.GetComponent<Rigidbody>().isKinematic = true;

                Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                currentObject.transform.Translate(thumbstick.x * Time.deltaTime * moveSpeed, 0, thumbstick.y * Time.deltaTime * moveSpeed, Space.World);

                // Rotate the item horizontally
                Quaternion currentRotation = currentObject.transform.rotation;
                float rotationAmount = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x * rotationSpeed * Time.deltaTime;
                currentObject.transform.rotation = currentRotation * Quaternion.AngleAxis(rotationAmount, Vector3.up);

                // Check for collisions
                //RaycastHit[] hits = Physics.RaycastAll(new Ray(currentObject.transform.position, Vector3.down), 1.0f);
                //foreach (RaycastHit hit in hits)
                //{
                //    if (hit.collider != currentObject.GetComponent<Collider>())
                //    {
                        // Move the object up
                //        currentObject.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed, Space.World);
                //    }
                //}
            }
        }
    }

    Vector3 GetRaycastHitPoint(OVRInput.Controller controller)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return transform.position + transform.forward * 5.0f;
        }
    }
}
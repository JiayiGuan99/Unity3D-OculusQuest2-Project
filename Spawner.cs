using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    //public Dropdown menu;
    //public OVRInput.controller controller;
    public GameObject TV;
    public GameObject cabinet;
    public GameObject chair;
    public GameObject desk;
    public GameObject locker;
    public GameObject whiteboard;
    public GameObject[] items;
    //public GameObject rayPrefab;
    public GameObject RightHandAnchor;
    public GameObject LeftHandAnchor;
    public float moveSpeed;
    public float rotationSpeed;

    private int dropdownIndex;
    private GameObject currentObject;
    private bool menuOpen;
    //private Dropdown dropdown;

    void Start()
    {
        //dropdown = GameObject.Find("menu").GetComponent<Dropdown>();
        //dropdown = GetComponent<Dropdown>();
        TV = GameObject.Find("3DTV");
        cabinet = GameObject.Find("cabinet");
        chair = GameObject.Find("chair");
        desk = GameObject.Find("desk");
        locker = GameObject.Find("locker");
        whiteboard = GameObject.Find("whiteboard");
        items = new GameObject[]{TV, cabinet, chair, desk, locker, whiteboard};
        moveSpeed = 2.0f;
        rotationSpeed = 50.0f;
        dropdownIndex = -1;
        menuOpen = false;
    }

    void Update()
    {
        //{
        //    RaycastHit hit;
        //    Ray ray = new Ray(RightHandAnchor.transform.position, RightHandAnchor.transform.forward);
        //    Physics.Raycast(ray, out hit);
        //    Debug.Log("akvvivpihvnlas"+ OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) + (hit.transform.gameObject.name == "menu"));
        //}
        // Open or close the menu
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9)
        {
            RaycastHit hit;
            Ray ray = new Ray(RightHandAnchor.transform.position, RightHandAnchor.transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.gameObject.name + hit.transform.gameObject.GetComponent<Dropdown>());
                //if (hit.transform.gameObject.name == "menu")
                //{
                //    dropdown = hit.transform.gameObject.GetComponentInChildren<Dropdown>();
                //    menuOpen = true;
                    //dropdown.show();
                //}
                if (hit.transform.gameObject.tag == "floor"){

                }
            }
        }

        //if (menuOpen)
        {
            // Close the menu
            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9)
            {
                menuOpen = false;
                //dropdown.hide();
            }

            // Change selected item with dropdown menu
            dropdownIndex = dropdown.value;
        }
        else if(dropdownIndex != -1 &&  menuOpen == false)
        {
            // Spawn the item
            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9)
            {
                Vector3 spawnPos = GetRaycastHitPoint(OVRInput.Controller.RTouch);

                // Instantiate the selected item at the hit point
                currentObject = Instantiate(items[dropdownIndex], spawnPos, Quaternion.identity);

                // Add physics properties
                currentObject.GetComponent<Rigidbody>().useGravity = true;
                currentObject.GetComponent<Rigidbody>().isKinematic = false;
                currentObject.tag = "Selectable";

                // Add collider
                currentObject.AddComponent<MeshCollider>();
                currentObject = null;
                // Destroy the ray indicator
                //Destroy(rayInstance);
            }

            // Move the item
            if (currentObject != null)
            {
                currentObject.GetComponent<Rigidbody>().useGravity = false;
                currentObject.GetComponent<Rigidbody>().isKinematic = true;

                Vector2 thumbstick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
                currentObject.transform.Translate(thumbstick.x * Time.deltaTime * moveSpeed, 0, thumbstick.y * Time.deltaTime * moveSpeed, Space.World);

                // Rotate the item horizontally
                Quaternion currentRotation = currentObject.transform.rotation;
                float rotationAmount = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * rotationSpeed * Time.deltaTime;
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
        Ray ray = new Ray(RightHandAnchor.transform.position, RightHandAnchor.transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return transform.position + transform.forward * 20.0f;
        }
    }
}
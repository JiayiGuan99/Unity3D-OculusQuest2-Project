using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject TV;
    public GameObject cabinet;
    public GameObject chair;
    public GameObject desk;
    public GameObject locker;
    public GameObject whiteboard;
    public GameObject RightHandAnchor;
    public GameObject LeftHandAnchor;
    private GameObject currentObject;
    private bool spawned;
    private bool ing;


    void Start()
    {
        spawned = false;
        ing = false;
    }

    void Update()
    {
        // {
        //     RaycastHit hit;
        //     Ray ray = new Ray(RightHandAnchor.transform.position, RightHandAnchor.transform.forward);
        //     Physics.Raycast(ray, out hit);
        //     Debug.Log("akvvivpihvnlas"+ OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) + (hit.transform.gameObject.tag));
        // }
        // Open or close the menu
        if ((OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9 && spawned == false) || ing == true)
        {
            RaycastHit hit;
            Ray ray = new Ray(RightHandAnchor.transform.position, RightHandAnchor.transform.forward);
            if (Physics.Raycast(ray, out hit) && spawned != true && ing != true)
            {
                switch(hit.transform.gameObject.tag){
                    case "TV":
                        currentObject = spawnObject(TV);
                        spawned = true;
                        break;
                    case "cabinet":
                        currentObject = spawnObject(cabinet);
                        spawned = true;
                        break;
                    case "chair":
                        currentObject = spawnObject(chair);
                        spawned = true;
                        break;
                    case "desk":
                        currentObject = spawnObject(desk);
                        spawned = true;
                        break;
                    case "locker":
                        currentObject = spawnObject(locker);
                        spawned = true;
                        break;
                    case "whiteboard":
                        currentObject = spawnObject(whiteboard);
                        spawned = true;
                        break;
                    default:
                        break;
                }
                ing = true;
            }

        if (currentObject != null && spawned == true && ing == true)
        {
            currentObject.GetComponent<Rigidbody>().useGravity = false;
            currentObject.GetComponent<Rigidbody>().isKinematic = true;
                
            // Rotate the item horizontally
            Quaternion currentRotation = currentObject.transform.rotation;
            float rotationAmount = 0.0f;
            if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9)
            {
                rotationAmount = 90.0f;
            }
            else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.9)
            {
                rotationAmount = -90.0f;
            }
            currentObject.transform.rotation = currentRotation * Quaternion.AngleAxis(rotationAmount, Vector3.forward);
            if(OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9){
                RaycastHit hit2;
                if (Physics.Raycast(RightHandAnchor.transform.position, RightHandAnchor.transform.forward, out hit2))
                {
                    currentObject.transform.position = new Vector3(hit2.point.x, currentObject.transform.lossyScale.y/2, hit2.point.z);
                }
            }
            currentObject.GetComponent<Rigidbody>().useGravity = true;
            currentObject.GetComponent<Rigidbody>().isKinematic = false;
            if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9 && ing == true){
                currentObject = null;
                spawned = false;
                ing = false;
            }
            //Debug.Log("AGFGDSDFGADFAd   " + spawned + ing + currentObject);
        }
    }
        }
        //Debug.Log("fagfgbaskjf" + currentObject);
        //.Log("fagfgbaskjf" + spawned);

        // Move the item
       

    GameObject spawnObject(GameObject obj){
        Vector3 spawnPos = GetRaycastHitPoint(OVRInput.Controller.RTouch);

        //Instantiate the selected item at the hit point
        currentObject = Instantiate(obj, spawnPos, obj.transform.rotation);

        // Add physics properties
        currentObject.GetComponent<Rigidbody>().useGravity = true;
        currentObject.GetComponent<Rigidbody>().isKinematic = false;
        currentObject.tag = "Selectable";

        //         // Add collider
        currentObject.AddComponent<MeshCollider>();
            //currentObject = null;
        return currentObject;
    }

    Vector3 GetRaycastHitPoint(OVRInput.Controller controller)
    {
        RaycastHit hit;
        Ray ray = new Ray(RightHandAnchor.transform.position, RightHandAnchor.transform.forward);

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "floor")
        {   
            return hit.point;
        }
        else
        {
            return transform.position + transform.forward * 2.0f;
        }
    }
}
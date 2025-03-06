using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading;

public class SelectorAndManipulator : MonoBehaviour
{
    // Variables for selection and manipulation
    public Material highlight;
    public GameObject RightHandAnchor;
    public GameObject LeftHandAnchor;
    public GameObject rightRay;
    private Transform selectedObject;
    //private Material originalMaterial;
    //private Material selectedMaterial;
    private float selectionTime;
    private bool manipulation;
    private bool rotation;
    private bool scaling;
    private bool placememt;
    private bool isSelected;
    private Vector3 initialScale;
    private const float scaleSpeed = 0.01f;
    // Variables for scaling
    
    void Start(){
        //scaleSpeed = 0.01f;
        manipulation = false;
        rotation = false;
        isSelected = false;
        scaling = false;
        placememt = false;
    }
    void Update()
    {
        // Check for right hand laser selection
        if ((OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9 && isSelected == false) || manipulation == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(RightHandAnchor.transform.position, RightHandAnchor.transform.forward, out hit) && isSelected == false && manipulation == false)
            {
                if(hit.transform.gameObject.tag == "Selectable" && isSelected == false){
                    selectedObject = hit.collider.gameObject.transform;
                    //selectedMaterial = selectedObject.GetComponent<MeshRenderer>().material;
                    initialScale = selectedObject.localScale;
                    //originalMaterial = selectedObject.GetComponent<Renderer>().material;
                    //selectedObject.GetComponent<Renderer>().material = highlight;
                    if(selectedObject != null){
                        //Debug.Log("sabkfowfgoaihwfoaif          "+ selectedObject);
                        rightRay.GetComponent<Renderer>().material.color = Color.yellow;
                        isSelected = true;
                        manipulation = true;
                        
                        placememt = true;
                    }
                }
            }
            if(isSelected == true && manipulation == true){
                if(OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9 && placememt == true){
                    RaycastHit hit2;
                    if (Physics.Raycast(RightHandAnchor.transform.position, RightHandAnchor.transform.forward, out hit2))
                    {
                        selectedObject.transform.position = new Vector3(hit2.point.x, selectedObject.transform.lossyScale.y/2, hit2.point.z);
                    }
                    if(OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9){
                        placememt = false;
                        rotation = true;
                    }
                }
                if(OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9){
                    placememt = false;
                    rotation = true;
                }
                //Debug.Log("afgfohfoaiwhfohao" + !placememt);
                //Debug.Log("afgfohfoaiwhfohao" + (scaling!= true && rotation == true && placememt == false));
                if(scaling!= true && rotation == true && placememt == false){
                    Quaternion currentRotation = selectedObject.rotation;
                    float rotationAmount = 0.0f;
                    if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9)
                    {
                        rotationAmount = 90.0f;
                    }
                    else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.9)
                    {
                        rotationAmount = -90.0f;
                    }
                    //Debug.Log("afgfohfoaiwhfohao" + rotationAmount);
                    selectedObject.transform.rotation = currentRotation * Quaternion.AngleAxis(rotationAmount, Vector3.forward);
                    //Debug.Log("sabkfowfgoaihwfoaif "+ OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger));
                    if(OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9 && placememt == false){
                        rotation = false;
                        scaling = true;
                    }
                    //Debug.Log("afgfohfoaiwhfohao" + rotation);
                }
                
                //Debug.Log("afgfohfoaiwhfohao" + (rotation == false && scaling == true && placememt == false));
                if(rotation == false && scaling == true && placememt == false){
                    // Scale the object with the left hand
                    if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9)
                    {
                        selectedObject.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
                    }
                    else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > 0.9)
                    {
                        selectedObject.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
                    }
                    //Debug.Log("sabkfowfgoaihwfoaif          "+ OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger));
                    if(OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.85){
                        
                        isSelected = false;
                    //selectedObject.GetComponent<Renderer>().material = originalMaterial;
                        rightRay.GetComponent<Renderer> ().material.color = Color.white;
                        selectedObject = null;
                        manipulation = false;
                        scaling = false;
                    //selectedMaterial = null;
                    }  
                    
                }        
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

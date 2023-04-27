using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorAndManipulator : MonoBehaviour
{
    // Variables for selection and manipulation
    private Transform selectedObject;
    private Material selectedMaterial;
    private float selectionTime;
    private bool isSelecting;
    private bool isSelected;
    private Vector3 initialScale;

    // Variables for scaling
    private const float scaleSpeed = 0.01f;

    // Tags for selectable objects
    private const string selectableTag = "Selectable";

    void Update()
    {
        // Check for right hand laser selection
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Vector3 hitPoint = GetRaycastHitPoint(OVRInput.Controller.RTouch);
            GameObject hitObject = GetSelectableObjectAtPoint(hitPoint);

            if (hitObject != null)
            {
                SelectObject(hitObject.transform);
            }
        }

        // Check for headset/camera gaze selection
        if (isSelecting)
        {
            selectionTime += Time.deltaTime;

            if (selectionTime >= 3.0f)
            {
                Vector3 hitPoint = GetRaycastHitPoint(OVRInput.Controller.None);
                GameObject hitObject = GetSelectableObjectAtPoint(hitPoint);

                if (hitObject != null)
                {
                    SelectObject(hitObject.transform);
                }

                isSelecting = false;
                selectionTime = 0.0f;
            }
        }

        // Check for object manipulation
        if (isSelected)
        {
            // Move the object with the right hand
            selectedObject.position = GetRaycastHitPoint(OVRInput.Controller.RTouch);

            // Rotate the object horizontally with the right hand
            Quaternion currentRotation = selectedObject.rotation;
            float rotationAmount = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x * Time.deltaTime;
            selectedObject.rotation = currentRotation * Quaternion.AngleAxis(rotationAmount, Vector3.up);

            // Scale the object with the left hand
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                selectedObject.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
            }
            else if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                selectedObject.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
            }

            // Confirm changes with the right index trigger
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                isSelected = false;
                selectedObject = null;
                selectedMaterial = null;
            }
        }
    }

    void SelectObject(Transform newObject)
    {
        if (selectedObject != null)
        {
            DeselectObject();
        }

        selectedObject = newObject;
        selectedMaterial = selectedObject.GetComponent<MeshRenderer>().material;
        initialScale = selectedObject.localScale;
        selectedMaterial.color = Color.yellow;
        isSelected = true;
    }

    void DeselectObject()
    {
        selectedMaterial.color = Color.white;
        selectedObject.localScale = initialScale;
    }

    GameObject GetSelectableObjectAtPoint(Vector3 point)
    {
        RaycastHit hit;
        Ray ray = new Ray(point, Vector3.down);

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag(selectableTag))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
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




using UnityEngine;
using UnityEngine.UI;

public class GazeSelection : MonoBehaviour
{
    public float gazeTime = 3f;
    public Image progressImage;
    public Color highlightColor = Color.blue;
    public string selectableTag = "Selectable";
    public GameObject selectedObject;

    private float timer;
    private bool isGazing;
    private Color originalColor;
    private RaycastHit hit;

    private void Start()
    {
        progressImage.fillAmount = 0;
    }

    private void Update()
    {
        // Cast a ray from the center of the camera view
        Ray ray = new Ray(cam.transform.position, cam.transform.forward,);

        if (Physics.Raycast(ray, out hit))
        {
            // If the ray hits an object with the selectable tag, highlight it
            if (hit.collider.CompareTag(selectableTag))
            {
                // Change the object's material color to the highlight color
                MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
                originalColor = renderer.material.color;
                renderer.material.color = highlightColor;

                // Start the gaze timer and update the progress bar
                timer += Time.deltaTime;
                progressImage.fillAmount = timer / gazeTime;

                // If the timer reaches the gaze time, select the object
                if (timer >= gazeTime)
                {
                    SelectObject(hit.collider.gameObject);
                }
            }
            else
            {
                // Reset the gaze timer and progress bar if the ray doesn't hit a selectable object
                timer = 0f;
                progressImage.fillAmount = 0f;

                // Reset the material color of the previously highlighted object
                if (selectedObject != null)
                {
                    MeshRenderer renderer = selectedObject.GetComponent<MeshRenderer>();
                    renderer.material.color = originalColor;
                    selectedObject = null;
                }
            }
        }
        else
        {
            // Reset the gaze timer and progress bar if the ray doesn't hit anything
            timer = 0f;
            progressImage.fillAmount = 0f;

            // Reset the material color of the previously highlighted object
            if (selectedObject != null)
            {
                MeshRenderer renderer = selectedObject.GetComponent<MeshRenderer>();
                renderer.material.color = originalColor;
                selectedObject = null;
            }
        }
    }

    private void SelectObject(GameObject obj)
    {
        // Deselect the previously selected object
        if (selectedObject != null)
        {
            MeshRenderer renderer = selectedObject.GetComponent<MeshRenderer>();
            renderer.material.color = originalColor;
        }

        // Select the new object and change its material color to the highlight color
        selectedObject = obj;
        MeshRenderer selectedRenderer = selectedObject.GetComponent<MeshRenderer>();
        originalColor = selectedRenderer.material.color;
        selectedRenderer.material.color = highlightColor;

        // Reset the gaze timer and progress bar
        timer = 0f;
        progressImage.fillAmount = 0f;
    }
}

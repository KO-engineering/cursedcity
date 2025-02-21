using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [SerializeField] float range = 10;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera cam;
    
    public UnityEvent<Interactable> OnInteract;

    Ray ray;
    RaycastHit hit;

    Interactable lastInteractedObject;

    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, range, layerMask))
        {
            if(hit.transform.TryGetComponent(out Interactable interactedObject))
            {
                UpdateOutlines(interactedObject);

                if(Input.GetMouseButtonDown(0))
                {
                    interactedObject.Interact();
                    OnInteract?.Invoke(interactedObject);
                }
            }
            else
            {
                UpdateOutlines();
            }
        }
        else
        {
            UpdateOutlines();
        }
    }

    void UpdateOutlines(Interactable newInteractedObject = null)
    {
        if(newInteractedObject == null && lastInteractedObject == null) 
        {
            return;
        }

        if(lastInteractedObject != null && newInteractedObject != lastInteractedObject)
        {
            lastInteractedObject.ShowOutline(false);
            lastInteractedObject = null;

        }
        
        if(newInteractedObject != null)
        {
            newInteractedObject.ShowOutline(true);
            lastInteractedObject = newInteractedObject;
        }
        
    }
}

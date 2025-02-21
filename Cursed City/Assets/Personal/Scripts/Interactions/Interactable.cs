using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Outline))]
public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        ShowOutline(false);
    }

    public void Interact()
    {
        OnInteract?.Invoke();
    }

    public void ShowOutline(bool show)
    {
        outline.enabled = show;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int carIndex; // Set this in the Inspector to match the car
    private CarCustomizer carCustomizer;

    private void Start()
    {
        carCustomizer = CarCustomizer.Instance; // Get the CarCustomizer instance
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        carCustomizer.ShowTooltip(carIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        carCustomizer.HideTooltip();
    }
}
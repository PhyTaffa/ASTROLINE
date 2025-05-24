using UnityEngine;
using UnityEngine.EventSystems;

public class ResetHighlight : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData){
        EventSystem.current.SetSelectedGameObject(null);
    }
}
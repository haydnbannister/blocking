using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
public class SpaceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public bool buttonPressed;

public GameGrid gameGrid;
 
void Update() {
    if (buttonPressed) 
    {
        gameGrid.HandleMobileControl("space");
    }
}

public void OnPointerDown(PointerEventData eventData){
     buttonPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    buttonPressed = false;
}
}
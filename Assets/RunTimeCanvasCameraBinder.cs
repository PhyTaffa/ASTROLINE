using UnityEngine;
using UnityEngine.UI;

public class RuntimeCanvasCameraBinder : MonoBehaviour{
    public Camera[] cameras;          
    Canvas canvas;

    void Awake(){
        canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    void LateUpdate(){

        foreach (var cam in cameras){
            
            if (cam.enabled){
                if (canvas.worldCamera != cam)
                canvas.worldCamera = cam;
                break;
            }
        }
    }
}

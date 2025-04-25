using UnityEngine;
using UnityEngine.UI;
    
public class ButtonBlocker : MonoBehaviour, ICanvasRaycastFilter {
    public float alphaThreshold = 0.1f;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Image image = GetComponent<Image>();
        if (image.sprite == null) return false;

        RectTransform rectTransform = transform as RectTransform;

        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out local);

        Rect rect = rectTransform.rect;
        float x = (local.x - rect.x) / rect.width;
        float y = (local.y - rect.y) / rect.height;

        Texture2D tex = image.sprite.texture;
        Color pixel = tex.GetPixelBilinear(x, y);
        return pixel.a >= alphaThreshold;
    }
}

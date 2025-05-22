using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        HideOutline(); // Hide on start
    }

    public void ShowOutline(Color color)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_OutlineColor", color);  // You must have this property in your shader
        propBlock.SetFloat("_OutlineWidth", 1.04f); // Outline width (visible)
        rend.SetPropertyBlock(propBlock);
    }

    public void HideOutline()
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_OutlineWidth", 0f); // Hide by setting width to 0
        rend.SetPropertyBlock(propBlock);
    }
}



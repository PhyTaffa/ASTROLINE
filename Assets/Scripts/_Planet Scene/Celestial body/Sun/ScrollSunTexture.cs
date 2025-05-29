using UnityEngine;

public class ScrollSunTexture : MonoBehaviour
{
    public float scrollSpeedU = 0.01f;
    public float scrollSpeedV = 0.005f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetU = Time.time * scrollSpeedU;
        float offsetV = Time.time * scrollSpeedV;
        Vector2 offset = new Vector2(offsetU, offsetV);

        rend.material.mainTextureOffset = offset;
        rend.material.SetTextureOffset("_EmissionMap", offset);
    }
}


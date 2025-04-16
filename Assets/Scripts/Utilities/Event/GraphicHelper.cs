using UnityEngine;
using UnityEngine.UI;

public class GraphicHelper : MaskableGraphic
{
    private Texture texture;

    public override Texture mainTexture
    {
        get
        {
            if (texture != null)
            {
                return texture;
            }

            return s_WhiteTexture;
        }
    }

    protected override void OnEnable()
    {
    }

    protected override void OnRectTransformDimensionsChange()
    {
    }

    public void SetTexture(Texture t)
    {
        texture = t;
    }

    public new void UpdateMaterial()
    {
        base.UpdateMaterial();
    }
}
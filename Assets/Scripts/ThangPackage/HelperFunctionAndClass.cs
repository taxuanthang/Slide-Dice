using UnityEngine;

public static class HelperFunctionAndClass 
{
        // những function cho phép gọi nhiêu lần, để trợ giúp Thắng đẹp trai trong quá trình code do Thắng tự tổng hợp

    public static Sprite SpriteFromTexture(Texture2D tex2D)
    {
        if (tex2D != null)
        {
            Sprite sprite = Sprite.Create(
                tex2D,
                new Rect(0, 0, tex2D.width, tex2D.height),
                new Vector2(0.5f, 0.5f) // pivot center
            );
            return sprite;
        }
        else
        {
            Debug.LogError("Lam gi có texture đâu mà truyền vào như đúng rồi");
            return null;
        }
    }
}

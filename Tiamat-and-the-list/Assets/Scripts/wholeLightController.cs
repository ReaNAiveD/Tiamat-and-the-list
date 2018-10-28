using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wholeLightController : MonoBehaviour {

    public List<SpriteRenderer> sprites;
    private float alpha;
    public float Alpha
    {
        get
        {
            return alpha;
        }
        set
        {
            alpha = value;
            SetFGDarkAlpha(alpha);
        }
    }

    private void Start()
    {
        if (sprites.Count != 0)
        {
            alpha = sprites[0].color.a;
        }
        else
        {
            alpha = 0;
        }
    }

    public void SetFGDarkAlpha(float alpha)
    {
        float alphaSet = Mathf.Clamp(alpha, 0, 1);
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alphaSet);
        }
    }

}

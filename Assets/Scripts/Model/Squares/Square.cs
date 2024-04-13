using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public Vector2 Position;
    protected string defaultColourHex;
    protected SpriteRenderer _spriteRenderer;

    public void SetDefaultColour()
    {
        Color colour = HexToColor(defaultColourHex);
        _spriteRenderer.color = colour;
    }

    private Color HexToColor(string hex)
    {
        Color colour = new Color();
        ColorUtility.TryParseHtmlString(hex, out colour);
        return colour;
    }
}

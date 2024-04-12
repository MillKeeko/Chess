using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSquare : Square
{
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColourHex = "#FFCF9F";
    }
}

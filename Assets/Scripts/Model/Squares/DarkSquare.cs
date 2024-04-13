using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSquare : Square
{
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColourHex = "#D28C45";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHandler : MonoBehaviour
{
    public static CheckHandler instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            enabled = false;
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

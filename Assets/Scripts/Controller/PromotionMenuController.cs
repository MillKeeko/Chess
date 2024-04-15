using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PromotionMenuController : MonoBehaviour
{
    public VisualElement ui;
    public Button QueenBtn;
    public Button KnightBtn;
    public Button RookBtn;
    public Button BishopBtn;
    
    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    void Start()
    {
        
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        QueenBtn = ui.Q<Button>("QueenBtn");
        KnightBtn = ui.Q<Button>("KnightBtn");
        RookBtn = ui.Q<Button>("RookBtn");
        BishopBtn = ui.Q<Button>("BishopBtn");

        QueenBtn.clicked += OnQueenBtnClicked;
        KnightBtn.clicked += OnKnightBtnClicked;
        RookBtn.clicked += OnRookBtnClicked;
        BishopBtn.clicked += OnBishopBtnClicked;
    }

    private void OnQueenBtnClicked()
    {

    }

    private void OnKnightBtnClicked()
    {

    }

    private void OnRookBtnClicked()
    {

    }

    private void OnBishopBtnClicked()
    {

    }
}

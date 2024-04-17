using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Controller

public class PromotionMenuController : MonoBehaviour
{
    public GameObject MenuPromotion;

    private VisualElement _rootVisualElement;
    private UIDocument _uiDocument;

    public Button QueenBtn, KnightBtn, RookBtn, BishopBtn;

    public delegate void OnPromotion(Pieces pieceType);
    public static event OnPromotion OnPromotionEvent;
    
    void Awake()
    {
        TrackingHandler.OnPromotionSelectEvent += EnablePromotionMenu;

        SetupButtons();

        MenuPromotion.SetActive(false);
    }

    private void EnablePromotionMenu()
    {
        MenuPromotion.SetActive(true);
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        SetupButtons();
    }

    private void OnDisable()
    {
        QueenBtn.clicked -= OnQueenBtnClicked;
        KnightBtn.clicked -= OnKnightBtnClicked;
        RookBtn.clicked -= OnRookBtnClicked;
        BishopBtn.clicked -= OnBishopBtnClicked;
    }

    private void SetupButtons()
    {
        _uiDocument = GetComponent<UIDocument>();
        _rootVisualElement = _uiDocument.rootVisualElement;
        
        QueenBtn = _rootVisualElement.Q<Button>("QueenBtn");
        KnightBtn = _rootVisualElement.Q<Button>("KnightBtn");
        RookBtn = _rootVisualElement.Q<Button>("RookBtn");
        BishopBtn = _rootVisualElement.Q<Button>("BishopBtn");

        QueenBtn.clicked += OnQueenBtnClicked;
        KnightBtn.clicked += OnKnightBtnClicked;
        RookBtn.clicked += OnRookBtnClicked;
        BishopBtn.clicked += OnBishopBtnClicked;
    }

    private void OnQueenBtnClicked()
    {
        OnPromotionEvent?.Invoke(Pieces.Queen);
        MenuPromotion.SetActive(false);
    }

    private void OnKnightBtnClicked()
    {
        OnPromotionEvent?.Invoke(Pieces.Knight);
        MenuPromotion.SetActive(false);
    }   

    private void OnRookBtnClicked()
    {
        OnPromotionEvent?.Invoke(Pieces.Rook);
        MenuPromotion.SetActive(false);
    }

    private void OnBishopBtnClicked()
    {
        OnPromotionEvent?.Invoke(Pieces.Bishop);
        MenuPromotion.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private enum CardState
    {
        Idle,
        Hover,
        Dragging,
        Played
    }

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Quaternion originalRotation;
    private CardState currentState = CardState.Idle;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
    }

    void Update()
    {
        switch (currentState)
        {
            case CardState.Hover:
                HandleHoverState();
                break;
            case CardState.Dragging:
                HandleDragState();
                if (!Input.GetMouseButton(button: 0))
                {
                    TransitionToState0();
                }
                break;
            case CardState.Played:
                HandlePlayState();
                if (!Input.GetMouseButton(button: 0))
                {
                    TransitionToState0();
                }
                break;
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }
    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }
    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;

        if (Input.mousePosition.y < cardPlay.y)
        {
            currentState = CardState.Dragging;
            playArrow.SetActive(false);
        }
    }

    private void TransitionToState0()
    {
        currentState = CardState.Idle;
        rectTransform.localScale = originalScale;
        rectTransform.localPosition = originalPosition;
        rectTransform.localRotation = originalRotation;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == CardState.Dragging)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition))
            {
                rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = CardState.Played;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor); ;
                }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == CardState.Hover)
        {
            currentState = CardState.Dragging;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect: canvas.GetComponent<RectTransform>(), screenPoint: eventData.position, cam: eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == CardState.Idle)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;
            currentState = CardState.Hover;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == CardState.Hover)
        {
            TransitionToState0();
        }
    }
}

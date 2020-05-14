﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace UMVC
{
    public enum EAxisOptions
    { 
        Both,
        Horizontal, 
        Vertical
    }

    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public float Horizontal { get { return (snapX) ? SnapFloat(input.x, EAxisOptions.Horizontal) : input.x; } }
        public float Vertical { get { return (snapY) ? SnapFloat(input.y, EAxisOptions.Vertical) : input.y; } }
        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        public float HandleRange
        {
            get { return handleRange; }
            set { handleRange = Mathf.Abs(value); }
        }

        public float DeadZone
        {
            get { return deadZone; }
            set { deadZone = Mathf.Abs(value); }
        }

        public EAxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
        public bool SnapX { get { return snapX; } set { snapX = value; } }
        public bool SnapY { get { return snapY; } set { snapY = value; } }

        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;
        [SerializeField] private EAxisOptions axisOptions = EAxisOptions.Both;
        [SerializeField] private bool snapX = false;
        [SerializeField] private bool snapY = false;

        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        private RectTransform baseRect = null;

        private Canvas canvas;
        public Camera Cam
        {
            get
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceCamera
                    || canvas.renderMode == RenderMode.WorldSpace)
                    return canvas.worldCamera;

                return null;
            }
        }

        private Vector2 input = Vector2.zero;

        protected virtual void Start()
        {
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 position = RectTransformUtility.WorldToScreenPoint(Cam, background.position);
            Vector2 radius = background.sizeDelta / 2;

            input = (eventData.position - position) / (radius * canvas.scaleFactor);

            FormatInput();

            HandleInput(input.magnitude, input.normalized, radius, Cam);

            handle.anchoredPosition = input * radius * handleRange;
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
                input = Vector2.zero;
        }

        private void FormatInput()
        {
            if (axisOptions == EAxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == EAxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        private float SnapFloat(float value, EAxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions == EAxisOptions.Both)
            {
                float angle = Vector2.Angle(input, Vector2.up);
                if (snapAxis == EAxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == EAxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }
            return 0;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, Cam, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }
    }
}
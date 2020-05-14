﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UMVC
{
    public class JoystickMoveInputController : MoveInputControllerBase
    {
        private Joystick _joystick;
        private bool _isJoystickPressed;

        public JoystickMoveInputController(bool isActive, bool ignoreZeroDir)
            : base(isActive, ignoreZeroDir)
        {
            _joystick = Object.FindObjectOfType<Joystick>();

            UnityUIButton button = _joystick.GetComponent<UnityUIButton>();

            button.OnButtonPressedDown += OnJoystickPressDown;
            button.OnTappedAndHeld += OnJoystickPressDown;
            button.OnButtonPressedUp += OnJoystickPressUp;
            button.OnButtonTapped += OnJoystickPressUp;
        }

        protected override void DisposeCustomActions()
        {
            UnityUIButton button = _joystick.GetComponent<UnityUIButton>();

            button.OnButtonPressedDown -= OnJoystickPressDown;
            button.OnTappedAndHeld -= OnJoystickPressDown;
            button.OnButtonPressedUp -= OnJoystickPressUp;
            button.OnButtonTapped -= OnJoystickPressUp;
        }

        private void OnJoystickPressDown(PointerEventData eventData)
        {
            _isJoystickPressed = true;
        }

        private void OnJoystickPressUp(PointerEventData eventData)
        {
            _isJoystickPressed = false;
        }

        protected override Vector2 CheckDirection()
        {
            return _joystick.Direction;
        }

        protected override bool IsAnyInput()
        {
            return _isJoystickPressed;
        }

        protected override bool AdditionalInputEligibleCheck()
        {
            return CheckIfAnyInputBlockerUIActive();
        }

        private bool CheckIfAnyInputBlockerUIActive()
        {
            UIMenuManager.Instance.IsAnyUIActive(out List<UIMenu> activeUIMenuCollection, typeof(UIMenu));

            for (int i = 0; i < activeUIMenuCollection.Count; i++)
            {
                if (activeUIMenuCollection[i] is IInputBlocker)
                    return false;
            }

            return true;
        }
    }
}
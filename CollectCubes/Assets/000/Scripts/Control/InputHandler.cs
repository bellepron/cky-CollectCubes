using System;
using UnityEngine;
using cky.Managers;

namespace cky
{
    public class InputHandler : MonoBehaviour
    {
        public static event Action<Vector3> PointerPressed;
        public static event Action<Vector3> PointerMoved;
        public static event Action<Vector3> PointerRemoved;

        private Vector3 lastMousePosition;

        private void Start()
        {
            EventManager.GameStart += OnStart;
            EventManager.GameSuccess += OnSuccess;
            EventManager.GameFail += OnFail;
        }

        private void OnStart()
        {
            EventManager.GameUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
                PointerPressed?.Invoke(lastMousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                var currentMousePosition = Input.mousePosition;

                if (lastMousePosition != currentMousePosition)
                {
                    PointerMoved?.Invoke(currentMousePosition - lastMousePosition);
                    lastMousePosition = currentMousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                PointerRemoved?.Invoke(Input.mousePosition);
            }
        }

        private void OnSuccess()
        {
            EventManager.GameUpdate -= OnUpdate;
        }

        private void OnFail()
        {
            EventManager.GameUpdate -= OnUpdate;
        }
    }
}
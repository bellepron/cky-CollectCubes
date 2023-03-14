using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace cky.GamePanels
{
    public class GamePanelInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public static event Action OnDown, OnUp, OnDrag;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag?.Invoke();
        }
    }
}
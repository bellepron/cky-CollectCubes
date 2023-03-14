using cky.GamePanels;
using UnityEngine;
using cky.Extensions;

namespace cky.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [SerializeField] LevelSettings _s;

        [SerializeField] Rigidbody _rigidbody;
        Vector2 _firstClickPos, _currentMousePos;
        Vector3 _velocity;

        #endregion

        #region Preparing

        private void Start()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GamePanelInput.OnDown += OnClick;
            GamePanelInput.OnUp += OnRelease;
            GamePanelInput.OnDrag += OnDrag;
        }

        private void UnsubscribeEvents()
        {
            GamePanelInput.OnDown -= OnClick;
            GamePanelInput.OnUp -= OnRelease;
            GamePanelInput.OnDrag -= OnDrag;
        }

        #endregion

        private void OnClick() => _firstClickPos = Input.mousePosition;

        private void OnRelease() => _rigidbody.velocity = Vector3.zero;

        private void OnDrag()
        {
            _currentMousePos = (Vector2)Input.mousePosition;
            var moveVector = _currentMousePos - _firstClickPos;
            var moveVector3 = new Vector3(moveVector.x, 0, moveVector.y);
            moveVector3.Normalize();

            transform.TurnToDirection(moveVector3, _s.playerRotationSpeed);
            _rigidbody.velocity = transform.forward * _s.playerMoveSpeed * Time.deltaTime;
        }
    }
}
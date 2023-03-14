using UnityEngine;
using cky.Extensions;

namespace cky.AI
{
    public class Load_State : IState
    {
        Transform _actorTr;
        float _moveSpeed;
        float _turnSpeed;
        Vector3 _storagePos;
        INpc _iNpc;
        Rigidbody _rb;

        public Load_State(Transform actorTr, float moveSpeed, float turnSpeed, Vector3 storagePos, INpc iNpc)
        {
            _actorTr = actorTr;
            _moveSpeed = moveSpeed;
            _turnSpeed = turnSpeed;
            _storagePos = storagePos;
            _iNpc = iNpc;

            _rb = _actorTr.GetComponent<Rigidbody>();
        }

        public void Execute()
        {
            _rb.MoveWithVelocity(_storagePos, _moveSpeed, _turnSpeed);

            if (_actorTr.CloseToThisXZ(_storagePos, 1.5f))
            {
                _iNpc.ArrivedToTheStorage();
            }
        }

        public void OnEnter()
        {
            _iNpc.TargetIsGone();
        }

        public void OnExit()
        {

        }
    }
}
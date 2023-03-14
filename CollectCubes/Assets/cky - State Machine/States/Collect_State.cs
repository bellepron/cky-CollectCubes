using UnityEngine;
using cky.Extensions;
using cky.Managers;
using cky.Helpers;

namespace cky.AI
{
    public class Collect_State : IState
    {
        Transform _actorTr;
        float _moveSpeed;
        float _turnSpeed;
        INpc _iNpc;
        GameObject _target;
        Rigidbody _rb;

        public Collect_State(Transform actorTr, float speed, float turnSpeed, INpc iNpc)
        {
            _actorTr = actorTr;
            _moveSpeed = speed;
            _turnSpeed = turnSpeed;
            _iNpc = iNpc;

            _rb = _actorTr.GetComponent<Rigidbody>();
        }

        public void Execute()
        {
            _target = _iNpc.GetTargetObject();

            if (_target == null)
            {
                _iNpc.TargetIsGone();
            }
            else
            {
                var targetPos = _target.transform.position;

                _rb.MoveWithVelocity(targetPos, _moveSpeed, _turnSpeed);

                if (_actorTr.CloseToThisXZ(targetPos, 1.5f))
                {
                    _iNpc.ArrivedToTheTarget();
                }
            }
        }

        public void OnEnter()
        {
            EventManager.CheckAiTarget += CheckTargetsLayer;
        }

        public void OnExit()
        {
            EventManager.CheckAiTarget -= CheckTargetsLayer;
        }

        private void CheckTargetsLayer()
        {
            if (_target == null) return;

            if (_target.layer != LayerHelper.FREECUBE && _target.layer != LayerHelper.OPPONENTSCUBE)
                _iNpc.TargetIsGone();
        }
    }
}
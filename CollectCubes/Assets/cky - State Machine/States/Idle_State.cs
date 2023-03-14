using UnityEngine;

namespace cky.AI
{
    public class Idle_State : IState
    {
        Transform _actorTr;
        Rigidbody _rb;

        public Idle_State(Transform actorTr)
        {
            _actorTr = actorTr;

            _rb = _actorTr.GetComponent<Rigidbody>();
        }

        public void Execute()
        {

        }

        public void OnEnter()
        {
            _rb.velocity = Vector3.zero;
        }

        public void OnExit()
        {

        }
    }
}
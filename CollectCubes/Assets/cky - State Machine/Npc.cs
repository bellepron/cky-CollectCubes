using System.Collections;
using UnityEngine;
using System;
using cky.Managers;
using cky.Storages;

namespace cky.AI
{
    public class Npc : MonoBehaviour, INpc
    {
        #region Variables

        LayerMask _targetMask;
        float _moveSpeed;
        float _turnSpeed;
        float _findingRadiusStartValue;
        bool searchingTarget;

        GameObject _targetFound;

        Vector3 _storagePos;
        StateMachine _stateMachine;

        // States
        Idle_State IdleState;
        Collect_State CollectState;
        Load_State LoadState;

        // Conditions
        bool _isArrivedToTheTarget;
        bool _isArrivedToTheStorage;
        bool _isGameEnded;

        #endregion

        #region Preparing

        protected virtual void Start()
        {
            GetComponents();
            SetValuesViaLevelSettings();
            StateOperations();

            EventManager.GameStart += GameStart;
            EventManager.GameEnd += GameEnd;

            _stateMachine.SetState(IdleState);
        }

        private void GetComponents()
        {
            _stateMachine = new StateMachine();
            _storagePos = FindObjectOfType<OpponentStorage>().transform.position;
        }

        private void SetValuesViaLevelSettings()
        {
            var settings = LevelManager.Instance.levelSettings;

            _moveSpeed = settings.aiMoveSpeed;
            _turnSpeed = settings.aiTurnSpeed;
            _findingRadiusStartValue = settings.aiFindingRadiusStartValue;
            _targetMask = settings.aiTargetMask;
        }

        private void StateOperations()
        {
            // Create States
            IdleState = new Idle_State(transform);
            CollectState = new Collect_State(transform, _moveSpeed, _turnSpeed, this);
            LoadState = new Load_State(transform, _moveSpeed, _turnSpeed, _storagePos, this);

            // Define Conditions
            Func<bool> FoundTarget() => () => _targetFound != null;
            Func<bool> NoTarget() => () => _targetFound == null;
            Func<bool> ArrivedToTheTarget() => () => _isArrivedToTheTarget == true;
            Func<bool> ArrivedToTheStorage() => () => _isArrivedToTheStorage == true;
            Func<bool> GameEnd() => () => _isGameEnded == true;

            // Define Transitions
            _stateMachine.AddTransition(IdleState, CollectState, FoundTarget());
            _stateMachine.AddTransition(CollectState, IdleState, NoTarget());
            _stateMachine.AddTransition(CollectState, LoadState, ArrivedToTheTarget());
            _stateMachine.AddTransition(LoadState, CollectState, ArrivedToTheStorage());
            _stateMachine.AddAnyTransition(IdleState, GameEnd());

            _stateMachine.RemoveTransition(IdleState, CollectState, FoundTarget());
            _stateMachine.RemoveTransition(CollectState, IdleState, NoTarget());
            _stateMachine.RemoveTransition(CollectState, LoadState, ArrivedToTheTarget());
            _stateMachine.RemoveTransition(LoadState, CollectState, ArrivedToTheStorage());
        }

        #endregion

        private void GameStart()
        {
            EventManager.GameUpdate += GameUpdate;
        }

        private void GameEnd(bool b)
        {
            _isGameEnded = true;

            UnsubscribeEvents();
        }

        private void OnDestroy() => UnsubscribeEvents();

        private void UnsubscribeEvents()
        {
            EventManager.GameUpdate -= GameUpdate;
            EventManager.GameStart -= GameStart;
            EventManager.GameEnd -= GameEnd;
        }

        private void GameUpdate()
        {
            _stateMachine.Execute();

            if (_targetFound == null)
            {
                StartCoroutine(FindTargetObject());
            }
        }

        public GameObject GetTargetObject() => _targetFound;

        public void TargetIsGone() => _targetFound = null;

        public void ArrivedToTheTarget()
        {
            _isArrivedToTheTarget = true;
            StartCoroutine(Res());
        }

        IEnumerator Res()
        {
            yield return null;
            _isArrivedToTheTarget = false;
        }

        public void ArrivedToTheStorage()
        {
            _isArrivedToTheStorage = true;
            StartCoroutine(Res1());
        }

        IEnumerator Res1()
        {
            yield return null;
            _isArrivedToTheStorage = false;
        }

        IEnumerator FindTargetObject()
        {
            if (searchingTarget == true) yield break;

            searchingTarget = true;

            var radius = _findingRadiusStartValue;
            while (_targetFound == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, _targetMask);

                if (hitColliders.Length > 0)
                {
                    _targetFound = hitColliders[0].gameObject;
                    searchingTarget = false;

                    yield break;
                }

                yield return null;
                radius += 1;
            }
        }
    }
}
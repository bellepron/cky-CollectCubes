using cky.Enums;
using System;

namespace cky.Managers
{
    public class EventManager : SingletonPersistent<EventManager>
    {
        public static event Action GameStart, GameUpdate, GameSuccess, GameFail, GameTimeEnd;
        public static event Action OpenStartPanel;
        public static event Action<bool> GameEnd;
        public static event Action<int> SetTargetScore, AddPlayerScore, AddOpponentScore;
        public static event Action CheckMaxScoreHasBeenReached;
        public static event Action<GameTypes> SetByGameType;
        public static event Action CheckAiTarget;
        public static event Action CreateBuildingFromPNG;
        public static event Action TriggerCubeSpawner;

        protected override void OnPerAwake() => ResetEvents();

        public void ResetEvents()
        {
            GameStart = null;
            GameUpdate = null;
            GameSuccess = null;
            GameFail = null;
            GameTimeEnd = null;
            GameEnd = null;
            OpenStartPanel = null;

            SetTargetScore = null;
            AddPlayerScore = null;
            AddOpponentScore = null;
            CheckMaxScoreHasBeenReached = null;

            SetByGameType = null;

            CheckAiTarget = null;

            CreateBuildingFromPNG = null;
            TriggerCubeSpawner = null;
        }

        #region Basic Game Flow
        public void GameStartEvent() => GameStart?.Invoke();
        private void Update() => GameUpdate?.Invoke();
        public void GameSuccessEvent() => GameSuccess?.Invoke();
        public void GameFailEvent() => GameFail?.Invoke();
        public void GameTimeEndEvent() => GameTimeEnd?.Invoke();
        public void GameEndEvent(bool isSuccess) => GameEnd?.Invoke(isSuccess);
        public void OpenStartPanelEvent() => OpenStartPanel?.Invoke();
        #endregion

        #region Score
        public void SetTargetScoreEvent(int targetScore) => SetTargetScore?.Invoke(targetScore);
        public void AddPlayerScoreEvent(int score) => AddPlayerScore?.Invoke(score);
        public void AddOpponentScoreEvent(int score) => AddOpponentScore?.Invoke(score);
        public void HasMaxScoreBeenReached() => CheckMaxScoreHasBeenReached?.Invoke();
        #endregion

        #region Game Type
        public void SetGameTypeEvent(GameTypes gameType) => SetByGameType?.Invoke(gameType);
        #endregion

        #region AI
        public void CheckAiTargetEvent() => CheckAiTarget?.Invoke();
        #endregion

        #region Building
        public void CreateBuildingFromPNGEvent() => CreateBuildingFromPNG?.Invoke();
        #endregion

        #region Cube Spawner
        public void TriggerCubeSpawnerEvent() => TriggerCubeSpawner?.Invoke();
        #endregion
    }
}
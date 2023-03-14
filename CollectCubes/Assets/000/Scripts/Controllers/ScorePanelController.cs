using UnityEngine;
using cky.Managers;
using cky.Enums;
using TMPro;

namespace cky.Controllers
{
    public class ScorePanelController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerScoreTMP, opponentScoreTMP;

        int _targetScore;
        int _playerScore = 0;
        int _opponentScore = 0;

        EventManager _eventManager;

        private void Start()
        {
            _eventManager = EventManager.Instance;

            UpdatePlayerScoreTMP("");
            UpdateOpponentScoreTMP("");

            EventManager.SetByGameType += SetByGameType;
            EventManager.SetTargetScore += SetTargetScore;
            EventManager.AddPlayerScore += AddPlayerScore;
            EventManager.AddOpponentScore += AddOpponentScore;
            EventManager.GameTimeEnd += Decision;
        }

        private void SetByGameType(GameTypes gameType)
        {
            if (gameType == GameTypes.NormalGame)
            {
                EventManager.CheckMaxScoreHasBeenReached += CheckIfMaxScoreHasBeenReached;
            }
            else if (gameType == GameTypes.AgainstTime)
            {
                EventManager.CheckMaxScoreHasBeenReached += CheckIfMaxScoreHasBeenReached;
            }
        }

        private void SetTargetScore(int targetScore) => _targetScore = targetScore;

        private void AddPlayerScore(int score)
        {
            _playerScore += score;
            UpdatePlayerScoreTMP($"PLAYER:{_playerScore}");

            _eventManager.HasMaxScoreBeenReached();
        }

        private void AddOpponentScore(int score)
        {
            _opponentScore += score;
            UpdateOpponentScoreTMP($"OPPONENT:{_opponentScore}");

            _eventManager.HasMaxScoreBeenReached();
        }

        private void CheckIfMaxScoreHasBeenReached()
        {
            if (HasMaxScoreBeenReached())
                Decision();
        }

        private bool HasMaxScoreBeenReached() => _playerScore + _opponentScore >= _targetScore;

        private void UpdatePlayerScoreTMP(string txt) =>
            playerScoreTMP.text = txt;

        private void UpdateOpponentScoreTMP(string txt) =>
            opponentScoreTMP.text = txt;

        private void Decision()
        {
            if (_playerScore >= _opponentScore)
            {
                _eventManager.GameEndEvent(true);
            }
            else
            {
                _eventManager.GameEndEvent(false);
            }
        }
    }
}
using System.Collections;
using UnityEngine;
using cky.Managers;
using TMPro;
using cky.Enums;

namespace cky.Controllers
{
    public class GameTimeController : MonoBehaviour
    {
        [SerializeField] GameObject gameTimePanel;
        [SerializeField] TextMeshProUGUI gameTimeTMP;
        WaitForSeconds _wfsInterval = new WaitForSeconds(1.0f);
        EventManager _eventManager;
        float _gameTime;
        bool _isGameActive;

        private void Start()
        {
            _eventManager = EventManager.Instance;
            _gameTime = LevelManager.Instance.levelSettings.gameTime;

            gameTimePanel.SetActive(false);

            EventManager.SetByGameType += SetByGameType;
        }

        private void SetByGameType(GameTypes gameType)
        {
            switch (gameType)
            {
                case GameTypes.NormalGame:
                    break;
                case GameTypes.AgainstTime:
                    SubscribeEvents();
                    break;
                case GameTypes.AgainstOpponent:
                    SubscribeEvents();
                    break;
            }
        }

        private void SubscribeEvents()
        {
            EventManager.GameStart += OnGameStart;
            EventManager.GameEnd += OnGameEnd;
        }

        private void OnDestroy()
        {
            EventManager.SetByGameType -= SetByGameType;
            EventManager.GameStart -= OnGameStart;
            EventManager.GameEnd -= OnGameEnd;
        }

        private void OnGameStart()
        {
            gameTimePanel.SetActive(true);
            UpdateGameTimeTMP();

            StartCoroutine(DecreaseTime());
        }

        private void OnGameEnd(bool isSuccess)
        {
            gameTimePanel.SetActive(false);
            _isGameActive = false;
        }

        IEnumerator DecreaseTime()
        {
            _isGameActive = true;

            while (_isGameActive == true)
            {
                _gameTime -= 1;

                UpdateGameTimeTMP();

                if (_gameTime == 0)
                {
                    _eventManager.GameTimeEndEvent();

                    _isGameActive = false;
                }

                yield return _wfsInterval;
            }
        }

        private void UpdateGameTimeTMP()
        {
            if (_gameTime > 59)
            {
                var mod = _gameTime % 60;
                var left = mod.ToString();
                if (mod < 10) left = "0" + left;

                var right = Mathf.FloorToInt(_gameTime / 60);

                gameTimeTMP.text = $"{right}:{left}";
            }
            else if (_gameTime > 9)
            {
                gameTimeTMP.text = $"00:{_gameTime}";
            }
            else
            {
                gameTimeTMP.text = $"00:0{_gameTime}";
            }
        }
    }
}
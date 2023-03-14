using UnityEngine.UI;
using UnityEngine;
using cky.Managers;
using cky.Enums;
using DG.Tweening;
using TMPro;

namespace cky.Controllers
{
    public class ProcessSliderController : MonoBehaviour
    {
        [SerializeField] GameObject processSliderPanel;
        [SerializeField] Slider processSlider;
        [SerializeField] TextMeshProUGUI percentageTMP;

        int _percentage = 0;
        [SerializeField] int _targetScore;
        int _playerScore = 0;

        private void Start()
        {
            OpenProcessSliderPanel(false);

            UpdatePercentageTMP();
            UpdateProcessSlider();

            EventManager.SetByGameType += SetByGameType;
        }

        private void SetByGameType(GameTypes gameType)
        {
            if (gameType == GameTypes.NormalGame || gameType == GameTypes.AgainstTime)
            {
                OpenProcessSliderPanel(true);

                EventManager.SetTargetScore += SetTargetScore;
                EventManager.AddPlayerScore += AddPlayerScore;
                EventManager.GameEnd += GameEnd;
            }
        }

        private void SetTargetScore(int targetScore) => _targetScore = targetScore;

        private void AddPlayerScore(int score)
        {
            _playerScore += score;
            _percentage = Mathf.FloorToInt((float)_playerScore * 100 / (float)_targetScore);

            UpdateProcessSlider();
            UpdatePercentageTMP();
        }

        private void OpenProcessSliderPanel(bool open) => processSliderPanel.SetActive(open);

        private void UpdateProcessSlider() => processSlider.value = _percentage;

        private void UpdatePercentageTMP() => percentageTMP.text = $"%{_percentage}";

        private void GameEnd(bool obj) => CloseProcessSliderPanel();

        private void CloseProcessSliderPanel() =>
            processSliderPanel.transform.DOScale(1.1f * Vector3.one, 0.5f).
                SetEase(Ease.OutBounce).
                OnComplete(() => processSliderPanel.SetActive(false));
    }
}
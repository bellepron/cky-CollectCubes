using cky.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace cky.GamePanels
{
    public class GameTypePanelController : MonoBehaviour
    {
        [SerializeField] GameObject gameTypePanel;
        [SerializeField] Button normalGameButton, againstTimeButton, againstOpponentButton;
        cky.Managers.EventManager _eventManager;

        GameObject[] _opponents;

        private void Start()
        {
            _eventManager = cky.Managers.EventManager.Instance;

            FindOpponent();
            OpenOpponent(false);

            gameTypePanel.SetActive(true);

            normalGameButton.onClick.AddListener(NormalGame);
            againstTimeButton.onClick.AddListener(AgainstTimeGame);
            againstOpponentButton.onClick.AddListener(AgainstOpponentGame);
        }

        private void SameForAllButtons(GameTypes gameType)
        {
            gameTypePanel.SetActive(false);
            _eventManager.OpenStartPanelEvent();

            _eventManager.SetGameTypeEvent(gameType);
        }

        private void NormalGame()
        {
            SameForAllButtons(GameTypes.NormalGame);

            _eventManager.CreateBuildingFromPNGEvent();
        }

        private void AgainstTimeGame()
        {
            SameForAllButtons(GameTypes.AgainstTime);

            _eventManager.CreateBuildingFromPNGEvent();
        }

        private void AgainstOpponentGame()
        {
            SameForAllButtons(GameTypes.AgainstOpponent);

            OpenOpponent(true);
            _eventManager.TriggerCubeSpawnerEvent();
        }

        #region Opponent

        private void FindOpponent() => _opponents = GameObject.FindGameObjectsWithTag("Opponent");

        private void OpenOpponent(bool open)
        {
            foreach (var opponent in _opponents)
                opponent.SetActive(open);
        }

        #endregion
    }
}
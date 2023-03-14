using UnityEngine;
using cky.Pooling;

namespace cky.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] bool showLabels;
        cky.GamePanels.EventManager _gamePanelEvents;

        #endregion

        #region Preparing

        private void Awake()
        {
            _gamePanelEvents = cky.GamePanels.EventManager.Instance;
            _gamePanelEvents.Initialize();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            PoolManager.Instance.ResetPool();

            SubscribeEvents();
        }

        #endregion

        #region Event Operations

        private void SubscribeEvents()
        {
            EventManager.GameEnd += WhenGameEnd;
        }

        private void UnSubscribeEvents()
        {
            EventManager.GameEnd -= WhenGameEnd;
        }

        private void WhenGameEnd(bool isSuccess)
        {
            if (isSuccess == true)
                Success();
            else
                Fail();
        }

        #endregion

        private void Success()
        {
            _gamePanelEvents.GameSuccessEvent();
            EventManager.Instance.GameSuccessEvent();
        }

        private void Fail()
        {
            _gamePanelEvents.GameFailEvent();
            EventManager.Instance.GameFailEvent();
        }


















        //[SerializeField] GameSettings _s;

        //[SerializeField] private CkyEvents _ckyEvents;
        //[SerializeField] private TextMeshProUGUI levelTMP;
        //[SerializeField] private CanvasGroup fadeCanvasGroup;
        //[SerializeField] private GameObject startPanel, successPanel, failPanel, storePanel;
        //private WaitForSeconds _wfs = new WaitForSeconds(1.5f);
        //[SerializeField] GameObject[] levels;
        //int levelIndex;

        //#region Start

        //private void Start()
        //{
        //    //Application.targetFrameRate = 60;

        //    PoolManager.Instance.Subscribe();
        //    PoolManager.Instance.ResetPool();
        //    BulletSpawner.Instance.Initialize();
        //    EffectSpawner.Instance.Initialize();

        //    levelIndex = PlayerPrefs.GetInt("levelIndex");

        //    levels[levelIndex % levels.Length].SetActive(true);
        //    levelTMP.text = $"Level {levelIndex + 1}";

        //    Globals.levelIndex = levelIndex;

        //    storePanel.SetActive(true);

        //    SetPanelsAtStart();
        //    SubscribeEvents();
        //}
        //private void SetPanelsAtStart()
        //{
        //    startPanel.SetActive(true);
        //    successPanel.SetActive(false);
        //    failPanel.SetActive(false);
        //}

        //private void GameStart()
        //{
        //    startPanel.SetActive(false);
        //    storePanel.SetActive(false);
        //    levelTMP.gameObject.SetActive(false);
        //}

        //#endregion

        //#region Event Operations
        //private void SubscribeEvents()
        //{
        //    CkyEvents.GameStart += GameStart;
        //    CkyEvents.GameSuccess += GameSuccess;
        //    CkyEvents.GameFail += GameFail;
        //}

        //private void UnSubscribeEvents()
        //{
        //    CkyEvents.GameStart -= GameStart;
        //    CkyEvents.GameSuccess -= GameSuccess;
        //    CkyEvents.GameFail -= GameFail;
        //}

        //private void OnDestroy()
        //{
        //    UnSubscribeEvents();
        //}
        //#endregion

        //#region Panels&Buttons

        //public void StartPanelClicked()
        //{
        //    _ckyEvents.GameStartEvent();
        //}

        //public void OnSuccessPanelClicked()
        //{
        //    ReloadCurrentScene();
        //}

        //public void OnFailPanelClicked()
        //{
        //    ReloadCurrentScene();
        //}

        //public void OnRestartButtonClicked()
        //{
        //    ReloadCurrentScene();
        //}

        //#endregion

        //private void OpenTimeOutPanel()
        //{
        //    startPanel.SetActive(true);
        //    storePanel.SetActive(true);
        //}
        //private void CloseTimeOutPanel()
        //{
        //    startPanel.SetActive(false);
        //    storePanel.SetActive(false);
        //}

        //private void GameSuccess()
        //{
        //    CloseTimeOutPanel();

        //    IncreaseLevel();

        //    StartCoroutine(PanelDelay(successPanel));

        //    Time.timeScale = 1.0f;
        //}
        //private void IncreaseLevel()
        //{
        //    levelIndex++;
        //    PlayerPrefs.SetInt("levelIndex", levelIndex);
        //}
        //private void GameFail()
        //{
        //    StartCoroutine(PanelDelay(failPanel));
        //}
        //private IEnumerator PanelDelay(GameObject panel)
        //{
        //    yield return _wfs;

        //    panel.SetActive(true);
        //}

        //public void ReloadCurrentScene()
        //{
        //    StartCoroutine(DelayedRestart());
        //}
        //IEnumerator DelayedRestart()
        //{
        //    DOTween.KillAll();
        //    _ckyEvents.GameRestartEvent();

        //    yield return null;

        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }
}
using UnityEngine;

namespace cky.Managers
{
    public class LevelManager : SingletonNonPersistent<LevelManager>
    {
        public LevelSettings[] levels;
        public LevelSettings levelSettings;

        const string pPrefsLevelIndex = "levelIndex";
        int _levelIndex;

        private void Start()
        {
            _levelIndex = PlayerPrefs.GetInt(pPrefsLevelIndex);
            levelSettings = levels[_levelIndex % levels.Length];

            EventManager.GameSuccess += OnGameSuccess;
        }

        private void OnDestroy()
        {
            EventManager.GameSuccess -= OnGameSuccess;
        }

        private void OnGameSuccess()
        {
            _levelIndex++;
            PlayerPrefs.SetInt(pPrefsLevelIndex, _levelIndex);
        }
    }
}
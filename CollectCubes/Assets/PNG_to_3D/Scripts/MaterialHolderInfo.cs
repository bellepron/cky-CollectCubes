using UnityEngine;

namespace cky.PngTo3D.Builder
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Build/Material Holder Info")]
    public class MaterialHolderInfo : ScriptableObject
    {
        [Header("       Cached Materials")]
        public Material[] cachedMats;
        [SerializeField] private int index;

        public void AtStart()
        {
            index = 0;
            SetAllMatsAplhasZero();
        }
        public int AddNewColor(Color newColor)
        {
            index++;
            cachedMats[index].color = newColor;

            return index;
        }
        public Material GetMaterialWithIndex(int i)
        {
            return cachedMats[i];
        }
        public void SetAllMatsAplhasZero()
        {
            int length = cachedMats.Length;
            for (int i = 0; i < length; i++)
            {
                cachedMats[i].color = new Color(1, 1, 1, 0);
            }
        }
        private void OnApplicationQuit()
        {
            index = 0;
            SetAllMatsAplhasZero();
        }
    }
}
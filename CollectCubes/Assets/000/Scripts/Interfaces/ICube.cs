using UnityEngine;

namespace cky.Interfaces
{
    public interface ICube
    {
        public bool IsBelongToSomeone { get; set; }
        public bool IsInteractive { get; set; }
        void SetFree();
        void SetBelonging(int layer);
        void SetStoraged(Material mat, int layer, Vector3 storagePosition);
    }
}
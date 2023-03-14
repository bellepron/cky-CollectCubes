using UnityEngine;
using cky.Helpers;
using cky.Managers;
using cky.Interfaces;

namespace cky.Storages
{
    public abstract class StorageAbstract : MonoBehaviour
    {
        protected Material storageMat;
        protected int storageLayer;
        protected EventManager eventManager;
        [SerializeField] Transform[] storagePoses;
        int _storageSize;

        private void Start()
        {
            eventManager = FindObjectOfType<EventManager>();
            storageLayer = LayerHelper.NOTINTERACTIVECUBE;
            _storageSize = storagePoses.Length;

            SetInfo();
        }

        protected abstract void SetInfo();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICube>(out var iCube))
            {
                if (iCube.IsInteractive)
                {
                    iCube.SetStoraged(storageMat, storageLayer, storagePoses[Random.Range(0, _storageSize)].position);

                    AddScore();
                }
            }
        }

        protected abstract void AddScore();
    }
}
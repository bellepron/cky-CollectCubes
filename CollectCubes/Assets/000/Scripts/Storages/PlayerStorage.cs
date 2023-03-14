using cky.Managers;

namespace cky.Storages
{
    public class PlayerStorage : StorageAbstract
    {
        protected override void SetInfo() =>
            storageMat = CubeMaterialManager.Instance.playerStorageCubeMat;

        protected override void AddScore() =>
            eventManager.AddPlayerScoreEvent(1);
    }
}
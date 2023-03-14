using cky.Managers;

namespace cky.Storages
{
    public class OpponentStorage : StorageAbstract
    {
        protected override void SetInfo() =>
            storageMat = CubeMaterialManager.Instance.opponentStorageCubeMat;

        protected override void AddScore() =>
            eventManager.AddOpponentScoreEvent(1);
    }
}
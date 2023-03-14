using UnityEngine;
using cky.Helpers;
using cky.Interfaces;

namespace cky.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICube>(out var iCube))
            {
                iCube.SetBelonging(LayerHelper.PLAYERSCUBE);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ICube>(out var iCube))
            {
                iCube.SetFree();
            }
        }
    }
}
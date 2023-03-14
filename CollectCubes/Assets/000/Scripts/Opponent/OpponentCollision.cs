using UnityEngine;
using cky.Helpers;
using cky.Interfaces;

namespace cky.Opponents
{
    public class OpponentCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICube>(out var iCube))
            {
                iCube.SetBelonging(LayerHelper.OPPONENTSCUBE);
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
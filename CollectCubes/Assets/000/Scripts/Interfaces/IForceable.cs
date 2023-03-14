using UnityEngine;

namespace cky.Interfaces
{
    public interface IForceable
    {
        void AddForce(Vector3 forceVector, float forcePower);
    }
}
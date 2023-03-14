using cky.Helpers;
using cky.Interfaces;
using cky.Managers;
using cky.Pooling;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BuilderBox : MonoBehaviour, ICube, IPooledObject, IForceable
{
    public bool IsBelongToSomeone { get; set; } = false;
    public bool IsInteractive { get; set; } = true;

    MeshRenderer _mr;
    Rigidbody _rb;

    PoolManager _poolManager;

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();

        _poolManager = PoolManager.Instance;
    }

    void IPooledObject.OnObjectSpawn()
    {
        IsBelongToSomeone = false;
        IsInteractive = true;
        gameObject.layer = LayerHelper.FREECUBE;
    }

    void ICube.SetFree()
    {
        if (IsInteractive == false) return;

        gameObject.layer = LayerHelper.FREECUBE;
        IsBelongToSomeone = false;
    }

    void ICube.SetBelonging(int layer)
    {
        if (IsInteractive == false) return;

        gameObject.layer = layer;
        IsBelongToSomeone = true;

        EventManager.Instance.CheckAiTargetEvent();
    }

    void ICube.SetStoraged(Material mat, int layer, Vector3 storagePosition)
    {
        IsInteractive = false;
        gameObject.layer = layer;
        _mr.material = mat;

        JumpTo(storagePosition);

        DOVirtual.DelayedCall(2.5f, () => _poolManager.Despawn(gameObject));
    }

    private void JumpTo(Vector3 targetPos)
    {
        _rb.velocity = Vector3.zero;
        transform.DOJump(targetPos, 3, 1, 0.35f);
    }

    void IForceable.AddForce(Vector3 forceVector, float forcePower)
    {
        _rb.AddForce(forceVector * forcePower, ForceMode.Impulse);
    }
}
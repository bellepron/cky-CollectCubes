using UnityEngine;

public interface INpc
{
    void TargetIsGone();
    GameObject GetTargetObject();
    void ArrivedToTheTarget();
    void ArrivedToTheStorage();
}
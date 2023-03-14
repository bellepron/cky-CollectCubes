using UnityEngine;

public class SingletonNonPersistent<T> : MonoBehaviour where T : SingletonNonPersistent<T>
{
    static SingletonNonPersistent<T> instance;
    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public static T Instance
    {
        get => (T)instance;
    }
}
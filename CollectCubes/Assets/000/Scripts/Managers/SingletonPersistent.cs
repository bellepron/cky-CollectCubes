using UnityEngine;

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T Instance;
    private void Awake()
    {
        OnPerAwake();

        if (instance == null)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public static T instance
    {
        get => (T)Instance;
    }

    protected virtual void OnPerAwake() { }
}
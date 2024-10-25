using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = GameObject.Find(typeof(T).Name);
                if(go == null)
                {
                    go = new GameObject(typeof(T).Name);
                    go.AddComponent<T>();
                    instance = go.GetComponent<T>();
                }
                else
                {
                    instance = go.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    public void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
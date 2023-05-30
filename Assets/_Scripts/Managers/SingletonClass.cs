using UnityEngine;
using _Scripts.NetworkScript;

public class NetworkMonoSingleton<T> : NetworkMonoBehaviour where T : Component
{
    #region Properties
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                {
                    Debug.LogWarning($"There is more than one {typeof(T).Name} in the scene. {objs.Length - 1} objects deleted.");

                    for (int i = 1; i < objs.Length; i++)
                        Destroy(objs[i].gameObject);
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }

        private set { _instance = value; }
    }
    #endregion

    #region Methods
    public virtual void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
    #endregion
}
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Properties
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                {
                    Debug.LogWarning($"There is more than one {typeof(T).Name} in the scene. {objs.Length - 1} objects deleted.");

                    for (int i = 1; i < objs.Length; i++)
                        Destroy(objs[i].gameObject);
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }

        private set { _instance = value; }
    }
    #endregion

    #region Methods
    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
    #endregion
}

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    #region Properties
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                {
                    Debug.LogWarning($"There is more than one {typeof(T).Name} in the scene. {objs.Length - 1} objects deleted.");

                    for (int i = 1; i < objs.Length; i++)
                        Destroy(objs[i].gameObject);
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }

        private set { _instance = value; }
    }
    #endregion

    #region Builts_In
    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(this);
    }
    #endregion
}

public class OnlinePersistentSingleton<T> : NetworkMonoBehaviour where T : Component
{
    #region Properties
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                {
                    Debug.LogWarning($"There is more than one {typeof(T).Name} in the scene. {objs.Length - 1} objects deleted.");

                    for (int i = 1; i < objs.Length; i++)
                        Destroy(objs[i].gameObject);
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }

        private set { _instance = value; }
    }
    #endregion

    #region Builts_In
    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(this);
    }
    #endregion
}

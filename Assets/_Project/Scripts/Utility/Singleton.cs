using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T instance
    {
        get
        {
            // Check if the instance is null (not yet set)
            if (_instance == null)
            {
                // Try to find the existing instance in the scene
                _instance = FindObjectOfType<T>();

                // If no instance exists in the scene, create a new GameObject and add the component
                if (_instance == null)
                {
                    GameObject obj = new GameObject
                    {
                        name = typeof(T).Name
                    };
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Ensure only one instance of the singleton exists
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject); // Optional: Keep the GameObject alive between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
}
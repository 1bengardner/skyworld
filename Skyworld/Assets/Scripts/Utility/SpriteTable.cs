using System.Collections.Generic;
using UnityEngine;

public class SpriteTable : MonoBehaviour {

    [SerializeField] IdSprite[] sprites;
    [System.Serializable]
    struct IdSprite
    {
        public string id;
        public Sprite sprite;
    }
    Dictionary<string, Sprite> lookup = new Dictionary<string, Sprite>();

    // Singleton
    public static SpriteTable Instance
    {
        get
        {
            instance = instance ?? FindObjectOfType<SpriteTable>();
            return instance;
        }
    }
    private static SpriteTable instance = null;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogWarning("Destroying duplicate SpriteTable.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (sprites.Length == 0)
        {
            Debug.LogWarning("SpriteTable is empty.");
        }
        foreach (IdSprite i in sprites)
        {
            lookup.Add(i.id, i.sprite);
        }
    }

    public Sprite Get(string id)
    {
        return lookup[id];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioSource[] sources;

    public static SoundManager Instance;

    void Awake()
    {
        // This only allows one instance of GameStateManager to exist in any scene
        // This is to avoid the need for GetComponent Calls. Use GameStateManager.Instance instead.
        if (Instance == null) {
            Instance = this;
        }else {
            Destroy(this);
        }

        GetSources();
        SetNames();
    }

    void GetSources()
    {
        if (sources.Length > 0) return;
        sources = new AudioSource[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            sources[i] = transform.GetChild(i).GetComponent<AudioSource>();
        }
    }

    void SetNames()
    {
        for (int i = 0; i < sources.Length; ++i)
        {
            sources[i].gameObject.name = sources[i].clip.name.ToLower();
        }
    }

    public void PlayClip(string name)
    {
        name = name.ToLower();
        for (int i = 0; i < sources.Length; ++i)
        {
            if (sources[i].gameObject.name == name)
            {
                sources[i].Play();
                return;
            }
        }
        print("Cannot Play: " + name);
    }

}

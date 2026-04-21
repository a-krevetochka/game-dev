using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct KeySoundPair
{
    public string key;
    public RandomSound sound;
}

public class DJ : MonoBehaviour
{
    private static DJ instance;

    [SerializeField] private KeySoundPair[] _soundsArr;

    private Dictionary<string, RandomSound> _sounds = new Dictionary<string, RandomSound>();

    public static void Play(string key)
    {
        if (instance._sounds.ContainsKey(key))
            instance._sounds[key].PlayRandom();
        else
            Debug.LogWarning($"DJ have not a {key} sound");
    }

    public void DeathSound()
	{
        Play("scream");
    }

	void Start()
    {
        instance = this;

        foreach (KeySoundPair pair in _soundsArr)
        {
            _sounds.Add(pair.key, pair.sound);
        }
    }
}

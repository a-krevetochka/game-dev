using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private int _levelIndex;

    void Start()
    {
        int index = PlayerPrefs.GetInt("level", 0);


        if (index < _levelIndex)
            PlayerPrefs.SetInt("level", _levelIndex);

        Destroy(this);
    }
}

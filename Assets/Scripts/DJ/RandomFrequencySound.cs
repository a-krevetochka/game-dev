using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFrequencySound : RandomSound
{
	[SerializeField] private int _sourcesPoolLenght = 5;

	private List<AudioSource> _sources = new List<AudioSource>();
	private int _lastUsedSourseId = 0;
	private float _lastUsedTime = 0;

	private void Start()
	{
		_sources.Add(_source);
		_source.clip = _clips[0];

		for (int i = 1; i < _sourcesPoolLenght; i++)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			_sources.Add(source);
			source.playOnAwake = false;
			source.clip = _clips[0];
			source.volume = _source.volume;
		}
	}

	public override void PlayRandom()
	{
		if (_sources[_lastUsedSourseId].clip.length > Time.time - _lastUsedTime)
			_lastUsedSourseId = (_lastUsedSourseId + 1) % _sources.Count;

		_lastUsedTime = Time.time;

		_sources[_lastUsedSourseId].pitch = Random.Range(0.95f, 1.05f);
		_sources[_lastUsedSourseId].clip = _clips[Random.Range(0, _clips.Length)];
		_sources[_lastUsedSourseId].Play();
	}
}

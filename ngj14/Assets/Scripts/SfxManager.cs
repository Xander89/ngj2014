using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SfxManager : MonoBehaviour {

	[Range(0.0f,1.0f)]
	float sfxVolume = 1.0f;

	[System.Serializable]
	class ActiveSource
	{
		public AudioSource source;
		public bool busy;
	}

	private List<ActiveSource> sources;
//	private Dictionary<string, AudioClip> clips;

	public AudioSource ambient;
	public AudioClip main_ambient;

	public void PlayAmbientGame()
	{
		ambient.clip = main_ambient;
		ambient.Play();
	}

	// Use this for initialization
	void Start () {
		AudioSource[] o_sources = GetComponentsInChildren<AudioSource>();
		sources = new List<ActiveSource>();
		foreach(AudioSource si in o_sources)
		{
			ActiveSource a = new ActiveSource();
			a.busy = false;
			a.source = si;
			a.source.volume = sfxVolume;
			sources.Add(a);
		}
		//StartCoroutine(Test());
	}
	
	// Update is called once per frame
	public void PlaySfx (string name) {
		AudioClip a = Resources.Load<AudioClip>(name);	
		if(a == null)
		{
			Debug.Log(name);
			Debug.Log("BILABILABILLABILAL!");
		}
		ActiveSource s = getFirstNotBusyAudioSouce();
		if(s != null)
		{
			s.busy = true;
			s.source.clip = a;
			s.source.Play();
			StartCoroutine(debusy(s));
		}
		else
		{
			Debug.Log("[Sound] Everything is busy, not playing " + name + " Audio file");
		}
	}

	private IEnumerator debusy(ActiveSource s) 
	{
		yield return new WaitForSeconds(s.source.clip.length);
		s.busy = false;
	}
 
	private ActiveSource getFirstNotBusyAudioSouce()
	{
		foreach(ActiveSource s in sources)
		{
			if(!s.busy)
			{
				return s;
			}
		}
		return null;
	}

	private IEnumerator Test()
	{
		while(true)
		{
			PlaySfx("sfx_mockup");
			yield return new WaitForSeconds(Random.Range(0.0f,1.0f));
		}
	}
}

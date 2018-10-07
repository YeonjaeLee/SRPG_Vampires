using UnityEngine;

public static class Sound  {

	static Sound() {
		soundObject=new GameObject("Sound");
		GameObject.DontDestroyOnLoad(soundObject);
	//	Debug.Log (FileIO.GetKeyData ("IsBgSound"));
		if (FileIO.IsKeyData ("IsBgSound")) {
			bBg = FileIO.GetKeyData<bool> ("IsBgSound");
		} else
			bBg = true;
		if (FileIO.IsKeyData ("IsEffectSound")) {
			bEffect = FileIO.GetKeyData<bool> ("IsEffectSound");
		} else
			bEffect = true;
		

	}

	static GameObject soundObject;


	public static void SourceController(System.Action<AudioSource> action,string str=null) {
		foreach(AudioSource t in soundObject.GetComponents<AudioSource>()) {
			if(str==null || t.clip.name==str) {
				action(t);
			}
		}

	}

    static float _volume;
    static float _Bgvolume;
    public static float Volume {
		get
        {
            return _volume;
        }

		set
        {
            _Bgvolume = BgVolume;
            SourceController(audio => { if (audio != bgMusic) audio.volume = value; });
            _volume = value;
        }

	}

    public static float BgVolume
    {
        get
        {
            return _Bgvolume;
        }

        set
        {
            _volume = Volume;
            SourceController(audio => { if (audio == bgMusic) audio.volume = value; });
            _Bgvolume = value;
        }

    }

    static float _pitch=1f;
	public static float Pitch {
		get {
			return _pitch;
		}
		set {
			_pitch = value;
			SourceController(audio=>audio.pitch=value);
		}
	}

	static bool _bMute=false;
	public static bool IsMute {
		get {return _bMute;}
		set {
			
			_bMute=value;
			
			SourceController(audio=>audio.mute=value);
			
		}
	}

	static bool _bBg=true;
	public static bool bBg {
		get {return _bBg;}
		set {

			_bBg=value;

			if (bgMusic)
				bgMusic.mute = !value;
		

		}
	}

	static bool _bEffect=true;
	public static bool bEffect {
		get {return _bEffect;}
		set {

			_bEffect=value;

			SourceController(audio=>{if(audio!=bgMusic)audio.mute=!value;});

		}
	}


	public static void Stop() {
        if (!bgMusic)
			SourceController(t=> {
				if(t.loop)
					Component.Destroy(t);
				else t.Stop();
			});

	}

	public static void Stop(string str) {
		SourceController(t=> {
			if(t.loop)
				Component.DestroyImmediate(t);
			else t.Stop();
		},str);

	}

	static AudioSource bgMusic=null;

    public static string GetBgName()
    {
        if(bgMusic == null)  return null;
        return bgMusic.name;
    }

	public static void StopBg() {
        if (bgMusic) {
			Component.Destroy (bgMusic);
			bgMusic = null;
		}

	}

	public static void SwapBg(string str) {
		if(bgMusic && bgMusic.name.Equals(str)) return;
		soundObject.AddTween (new TweenSoundFade (0, 1f).Next (
			new TweenSoundFade (1, 1), new TweenDelay (0).EndEvent (
				() => {
					
					PlayBg(str);
				})
			)
		);

	}

	public static void FadeSound(float vol, float delay, System.Action action)
	{
		
		soundObject.AddTween(new TweenSoundFade(vol, delay).EndEvent(action));
	}

    public static void FadeEffect(float vol, float delay, System.Action action)
    {
		
		soundObject.AddTween(new TweenEffectFade(vol, delay).EndEvent(action));
    }

    public static void PauseBg()
	{
		if (bgMusic)
		{
			bgMusic.Pause();
		}
	}

	public static void ResumeBg()
	{
		if (bgMusic)
		{
			bgMusic.Play();
		}
	}

	static void AsyncPlay(ref AudioSource ad, AudioClip clip, bool bLoop=false) {
		
			ad.clip = clip;
			ad.loop = bLoop;
			ad.Play ();
			ad.mute = Sound.IsMute | !Sound.bBg;
			ad.volume = BgVolume;
		
	}

	public static AudioSource PlayBg(string str, bool bAssetBundle = true, bool bLoop = true) {
        if (string.IsNullOrEmpty(str)) return null;
#if UNITY_EDITOR
		AudioClip clip = null;

		//if (bAssetBundle)
		//{
		//	int curslotIndex = UserInfoManager.instance.currentSlotIndex + 1;
		//	clip = AssetBundleManager.a.GetAsset<AudioClip>("Sounds/" + str + ".mp3") as AudioClip;
		//	if (!clip)
		//		clip = AssetBundleManager.a.GetAsset<AudioClip>("Sounds/" + str + ".wav") as AudioClip;
		//}
		//else
		//	clip=Resources.Load("Sounds/"+str) as AudioClip;

        clip = Resources.Load("Sounds/" + str) as AudioClip;
        if (clip == null) {
			Debug.LogError ("Can't Find SoundClip = " + str);
			return null;
		}


		if (!bgMusic)
			bgMusic =soundObject.AddComponent<AudioSource>();

		bgMusic.clip = clip;
		bgMusic.loop= bLoop;
		bgMusic.Play();
		bgMusic.mute=Sound.IsMute|!Sound.bBg;
		bgMusic.name=str;
        bgMusic.volume = BgVolume;
#else 
		if (bAssetBundle)
		{
			AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".mp3").completed+=(a) =>{
				AudioClip ac= ((AssetBundleRequest)a).asset as AudioClip;
				if(ac!=null) {
					if (!bgMusic)
						bgMusic =soundObject.AddComponent<AudioSource>();
					AsyncPlay(ref bgMusic, ac,bLoop);
				} else {
					AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".wav").completed+=(b) =>{
						ac= ((AssetBundleRequest)b).asset as AudioClip;
						if(ac!=null) {
							if (!bgMusic)
								bgMusic =soundObject.AddComponent<AudioSource>();
							AsyncPlay(ref bgMusic, ac,bLoop);
						}
					};

				}
			};
		}
		else {
			
			Resources.LoadAsync<AudioClip>("Sounds/"+str).completed+=(a) =>{
				AudioClip ac= ((ResourceRequest)a).asset as AudioClip;
				if(ac!=null) {
					if (!bgMusic)
						bgMusic =soundObject.AddComponent<AudioSource>();
					AsyncPlay(ref bgMusic, ac,bLoop);
				} 
			};
		}
		

#endif
        return bgMusic;

	}



    static AudioSource audiosource = null;

    public static string GetAudioName()
    {
        if (audiosource == null) return null;
        return audiosource.name;
    }

    public static AudioSource Play(string str,bool bAssetBundle=false, bool bLoop=false) {
        
		if (string.IsNullOrEmpty(str)) return null;
#if UNITY_EDITOR
        AudioClip clip = null;
        
   //     if (bAssetBundle)
   //     {
   //         int curslotIndex = UserInfoManager.instance.currentSlotIndex + 1;
			//clip = AssetBundleManager.a.GetAsset<AudioClip>("Sounds/" + str + ".mp3") as AudioClip;
			//if (!clip)
			//	clip = AssetBundleManager.a.GetAsset<AudioClip>("Sounds/" + str + ".wav") as AudioClip;
   //     }
   //     else
			//clip = Resources.Load("Sounds/" + str) as AudioClip;

        clip = Resources.Load("Sounds/" + str) as AudioClip;
        if (clip == null) {
			Debug.LogError ("Can't Find SoundClip = " + str);
			return null;
		}
		audiosource =soundObject.AddComponent<AudioSource>();
		audiosource.clip = clip;
        audiosource.loop=bLoop;
		audiosource.pitch = Pitch;
		audiosource.Play();
        audiosource.volume = Volume;
        
        audiosource.mute=Sound.IsMute|!Sound.bEffect;
		if(!bLoop) {
			Component.Destroy(audiosource,audiosource.clip.length);
		}
#else
		if (bAssetBundle)
		{
			AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".mp3").completed+=(a) =>{
				AudioClip ac= ((AssetBundleRequest)a).asset as AudioClip;
				if(ac!=null) {
					audiosource =soundObject.AddComponent<AudioSource>();
					AsyncPlay(ref audiosource, ac,bLoop);
				} else {
					AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".wav").completed+=(b) =>{
						ac= ((AssetBundleRequest)b).asset as AudioClip;
						if(ac!=null) {
							audiosource =soundObject.AddComponent<AudioSource>();
							AsyncPlay(ref audiosource, ac,bLoop);
						}
					};

				}
			};
		}
		else {

			Resources.LoadAsync<AudioClip>("Sounds/"+str).completed+=(a) =>{
				AudioClip ac=  ((ResourceRequest)a).asset as AudioClip;
				if(ac!=null) {
					audiosource =soundObject.AddComponent<AudioSource>();
					AsyncPlay(ref audiosource, ac,bLoop);
				} 
			};
		}

#endif
		return audiosource;
	
	}

}




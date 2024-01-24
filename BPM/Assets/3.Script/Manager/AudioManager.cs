using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //클래스를 직렬화
public class Sound
{
    public string name; //곡명
    public AudioClip clip; //곡

}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    #region 싱글톤
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public AudioSource[] audioSourceEffs; //효과음 배열
    public AudioSource audioSourceBgm;

    public string[] playSoundName;
    public string[] playBGMName;

    [SerializeField]
    public Sound[] effectSounds = null;
    public Sound[] bgmSounds = null;

    private void Start()
    {
        playSoundName = new string[audioSourceEffs.Length];
        //playBGMName = new string[audioSourceBgm.Length];
    }

    //브금
    public void PlayBGM(string BGMname)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (BGMname == bgmSounds[i].name)
            {
                audioSourceBgm.clip = bgmSounds[i].clip;
                audioSourceBgm.Play();
            }
        }
    }

    public void StopAllBGM()
    {
        audioSourceBgm.Stop();
    }

    //------------------------------------------------효과음
    public void PlaySE(string SEname)
    {
        for (int i =0; i < effectSounds.Length; i++)
        {
            if(SEname == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffs.Length; j++)
                {
                    if (!audioSourceEffs[j].isPlaying) //재생되지 않는 곡이 있다면
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffs[j].clip = effectSounds[i].clip;
                        audioSourceEffs[j].Play();
                        return; //for문 끝
                    }
                }

                return;
            }
        }

        Debug.Log(SEname + " 사운드가 SoundManager에 등록되지 않았습니다.");

    }

    public void StopAllSE() //모든 효과음 stop
    {
        for (int i = 0; i < audioSourceEffs.Length; i++)
        {
            audioSourceEffs[i].Stop();
        }
    }

    public void StopSE(string _SEname) //일부 SE만 stop
    {
        for( int i = 0; i < audioSourceEffs.Length; i++)
        {
            if (playSoundName[i] == _SEname)
            {
                audioSourceEffs[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인 " + _SEname + " SE가 없습니다.");
        
    }
}

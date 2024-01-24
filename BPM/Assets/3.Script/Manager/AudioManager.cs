using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //Ŭ������ ����ȭ
public class Sound
{
    public string name; //���
    public AudioClip clip; //��

}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    #region �̱���
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

    public AudioSource[] audioSourceEffs; //ȿ���� �迭
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

    //���
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

    //------------------------------------------------ȿ����
    public void PlaySE(string SEname)
    {
        for (int i =0; i < effectSounds.Length; i++)
        {
            if(SEname == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffs.Length; j++)
                {
                    if (!audioSourceEffs[j].isPlaying) //������� �ʴ� ���� �ִٸ�
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffs[j].clip = effectSounds[i].clip;
                        audioSourceEffs[j].Play();
                        return; //for�� ��
                    }
                }

                return;
            }
        }

        Debug.Log(SEname + " ���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");

    }

    public void StopAllSE() //��� ȿ���� stop
    {
        for (int i = 0; i < audioSourceEffs.Length; i++)
        {
            audioSourceEffs[i].Stop();
        }
    }

    public void StopSE(string _SEname) //�Ϻ� SE�� stop
    {
        for( int i = 0; i < audioSourceEffs.Length; i++)
        {
            if (playSoundName[i] == _SEname)
            {
                audioSourceEffs[i].Stop();
                return;
            }
        }
        Debug.Log("��� ���� " + _SEname + " SE�� �����ϴ�.");
        
    }
}

using UnityEngine;
using System.Linq;

public enum SELabel
{
    UltraStart,
    UltraNow,
    UltraEnd,
    GetOBJ,
    OBJLifeTimeEnd,
    GameStart,
    TimeUp,
    TimeWarning,
    DramRoll,
    ResultAppear,
    CountDown,
    Click
}

[System.Serializable]
public class SEData
{
    public AudioClip clip;
    public SELabel label;
}

public enum BGMLabel
{
    Title,
    Game,
}

[System.Serializable]
public class BGMData
{
    public AudioClip clip;
    public BGMLabel label;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource SESource;
    public AudioSource BGMSource;

    public static AudioManager instance;
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

    public SEData[] seDatas;
    public BGMData[] bgmDatas;

    public void PlaySE(SELabel label, bool oneShot = true)
    {
        var seData = seDatas.FirstOrDefault(data => data.label == label);
        if (seData != null)
        {
            if (oneShot)
            {
                SESource.PlayOneShot(seData.clip);
            }
            else
            {
                SESource.clip = seData.clip;
                SESource.Play();
            }
        }
        else
        {
            Debug.LogWarning("SELabel " + label + " not found!");
        }
    }

    public void PlayBGM(BGMLabel label)
    {
        var bgmData = bgmDatas.FirstOrDefault(data => data.label == label);
        if (bgmData != null)
        {
            BGMSource.clip = bgmData.clip;
            BGMSource.Play();
        }
        else
        {
            Debug.LogWarning("BGMLabel " + label + " not found!");
        }
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }
}

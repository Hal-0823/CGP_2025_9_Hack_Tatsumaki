using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private OBJSpawner objSpawnerPrefab;
    [SerializeField] private Tatsumaki tatsumakiPrefab;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UltraGauge ultraGauge;
    [SerializeField] private Result resultPanel;
    [SerializeField] private TextMeshProUGUI countDownText;

    private StatusDomain statusDomain;
    private OBJSpawner objSpawnerInstance;
    private Tatsumaki tatsumakiInstance;

    IEnumerator Start()
    {
        statusDomain = new StatusDomain();
        tatsumakiInstance = Instantiate(tatsumakiPrefab, Vector3.zero, Quaternion.identity);
        tatsumakiInstance.SetUp(statusDomain.PlayerSpeed, statusDomain.PlayerSize);
        tatsumakiInstance.onSuckedIn.AddListener(OnOBJCollected);

        objSpawnerInstance = Instantiate(objSpawnerPrefab);
        objSpawnerInstance.SetUp(statusDomain.OBJSizeRange, statusDomain.FieldSize, statusDomain.SpawnIntervalRange, statusDomain.OBJLifeTime);

        ultraGauge.SetUp(statusDomain.MaxUltraGauge);

        resultPanel.gameObject.SetActive(false);

        countDownText.text = "";

        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(CountDownRoutine());

        objSpawnerInstance.StartSpawning();
        StartCoroutine(TimerRoutine());
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlayBGM(BGMLabel.Game);
    }

    private IEnumerator CountDownRoutine()
    {
        int count = 3;
        AudioManager.instance.PlaySE(SELabel.CountDown, false);
        while (count > 0)
        {
            countDownText.text = count.ToString();
            countDownText.transform.localScale = Vector3.zero;
            countDownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack);
            //AudioManager.instance.PlaySE(SELabel.DramRoll, false);
            yield return new WaitForSeconds(1.0f);
            count--;
        }
        countDownText.text = "Start!";
        countDownText.transform.localScale = Vector3.zero;
        countDownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.0f);
        countDownText.transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.5f);
        countDownText.gameObject.SetActive(false);
    }

    private IEnumerator TimerRoutine()
    {
        while (statusDomain.Time > 0)
        {
            yield return new WaitForSeconds(1.0f);
            statusDomain.Time--;
            Debug.Log($"Time Remaining: {statusDomain.Time} seconds");
            timerText.text = "Time" + "\n" + statusDomain.Time.ToString("F0");

            if (statusDomain.Time <= 10)
            {
                timerText.color = Color.red;
                timerText.transform.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo);

                scoreText.DOFade(0, 0.5f).SetEase(Ease.InOutSine);
                //AudioManager.instance.PlaySE(SELabel.TimeWarning);
            }
        }

        objSpawnerInstance.StopSpawning();
        tatsumakiInstance.Disappear();
        AudioManager.instance.PlaySE(SELabel.TimeUp);

        resultPanel.gameObject.SetActive(true);
        resultPanel.SetUp(statusDomain.Score);
    }

    private void OnOBJCollected(OBJ obj)
    {
        statusDomain.UltraGauge = ultraGauge.SetUltraGaugeValue(statusDomain.UltraGauge + 1);

        if (ultraGauge.IsUltraReady())
        {
            tatsumakiInstance.StartCoroutine(tatsumakiInstance.ULTRA());
            ultraGauge.OnActiveUltra();
            statusDomain.UltraGauge = 0;
        }

        statusDomain.Score++;
        scoreText.text = "吸い込んだ数：" + statusDomain.Score.ToString();
    }
}
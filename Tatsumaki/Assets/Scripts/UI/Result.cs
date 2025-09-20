using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button retryButton;

    private int finalScore;

    public void SetUp(int finalScore)
    {
        this.finalScore = finalScore;
        scoreText.text = "吸い込んだ数：" + finalScore.ToString();
        scoreText.transform.localScale = Vector3.zero;
        this.gameObject.transform.localScale = Vector3.zero;

        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        yield return new WaitForSeconds(3.0f);
        AudioManager.instance.PlaySE(SELabel.DramRoll, false);
        DOTween.Sequence()
        .Append(this.gameObject.transform.DOScale(1.0f, 1.0f).SetEase(Ease.OutBack))
        .AppendInterval(4.0f)
        .AppendCallback(() =>
        {
            scoreText.transform.DOScale(1.0f, 0.5f).SetEase(Ease.InOutSine);
            AudioManager.instance.PlaySE(SELabel.ResultAppear, false);
        });
    }

    public void OnRetryButtonPressed()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySE(SELabel.Click);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}

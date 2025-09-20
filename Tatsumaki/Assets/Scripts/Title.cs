using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class Title : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI instructionText;
    private bool isStarted = false;

    private void Start()
    {
        AudioManager.instance.PlayBGM(BGMLabel.Title);
    }

    private void Update()
    {
        var current = Mouse.current;

        if (current.leftButton.wasPressedThisFrame && !isStarted)
        {
            AudioManager.instance.PlaySE(SELabel.Click);
            isStarted = true;
            DOTween.Sequence()
            .Append(titleText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .Join(instructionText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                LoadGameScene();
            });
        }
    }

    private void LoadGameScene()
    {
        AudioManager.instance.StopBGM();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
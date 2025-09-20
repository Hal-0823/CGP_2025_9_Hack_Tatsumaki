using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Events;

public class Tatsumaki : MonoBehaviour
{
    public UnityEvent<OBJ> onSuckedIn;
    public LayerMask targetLayer;
    private Camera mainCamera;

    [SerializeField] private float speed;
    [SerializeField] private float size;

    public bool isUltraActive { get; private set; } = false;
    private float ultraBonus { get { return isUltraActive ? 1.8f : 1.0f; } }

    public void SetUp(float speed = default, float size = default)
    {
        this.speed = speed == default ? this.speed : speed;
        this.size = size == default ? this.size : size;

        this.transform.DOScale(Vector3.one * this.size, 3.0f).From(0.0f).SetEase(Ease.OutBack);
    }

    public void Disappear()
    {
        this.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Destroy(this.gameObject);
        });
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        var current = Mouse.current;

        Ray ray = mainCamera.ScreenPointToRay(current.position.ReadValue());
        RaycastHit hit;

        // LayerMask (targetLayer) を指定してRayを飛ばす
        // targetLayerに設定されたレイヤーにしか当たらなくなる
        if (Physics.Raycast(ray, out hit, 100f, targetLayer))
        {
            Vector3 worldPosition = hit.point;
            Debug.Log("クリックした地面の座標: " + worldPosition);

            worldPosition.y = this.gameObject.transform.position.y; // y座標を固定
            MoveTo(worldPosition, speed);
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
    }

    private void MoveTo(Vector3 destination, float speed)
    {
        var distance = Vector3.Distance(this.gameObject.transform.position, destination);
        this.gameObject.transform.LookAt(destination);
        this.gameObject.transform.position += this.gameObject.transform.forward * speed * ultraBonus * Time.deltaTime;
    }

    public void SuckIn(OBJ obj)
    {
        AudioManager.instance.PlaySE(SELabel.GetOBJ);
        DOTween.Sequence()
        .Append(obj.transform.DOMove(this.transform.position, 0.4f).SetEase(Ease.OutQuart))
        .Join(obj.transform.DOScale(Vector3.zero, 0.5f)).SetEase(Ease.InExpo)
        .OnComplete(() =>
        {
            onSuckedIn?.Invoke(obj);
            Destroy(obj.gameObject);
        });
    }

    public IEnumerator ULTRA()
    {
        if (isUltraActive) yield break; // すでにウルトが発動中なら何もしない
        isUltraActive = true;
        float ultraDuration = 5.0f; // ウルトの持続時間

        Vector3 originalScale = this.transform.localScale;
        Vector3 ultraScale = originalScale * ultraBonus; // ウルト時のサイズ


        // ウルト開始
        AudioManager.instance.PlaySE(SELabel.UltraNow, false);
        this.transform.DOScale(ultraScale, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(ultraDuration);

        AudioManager.instance.PlaySE(SELabel.UltraEnd, false);
        // ウルト終了
        this.transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isUltraActive = false;
        });
    }
}
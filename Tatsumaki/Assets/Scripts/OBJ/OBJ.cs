using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class OBJ : MonoBehaviour
{
    public OBJAttribute Attribute { get; private set; }
    public float Size { get; private set; }

    public void SetUp(OBJAttribute attribute, Material material, float size, float lifeTime)
    {
        this.Attribute = attribute;
        GetComponent<MeshRenderer>().material = material;
        this.Size = size;
        transform.localScale = Vector3.one * size;
        Invoke(nameof(OnLifeTimeElapsed), lifeTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var tatsumaki = other.GetComponent<Tatsumaki>();
            if (tatsumaki != null)
            {
                tatsumaki.SuckIn(this);
            }
        }
    }

    private void OnLifeTimeElapsed()
    {
        AudioManager.instance.PlaySE(SELabel.OBJLifeTimeEnd);
        DOTween.Sequence()
        .Append(transform.DORotate(new Vector3(0, 720f, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear))
        .Join(transform.DOMoveY(10f, 1.0f).SetEase(Ease.InExpo))
        .OnComplete(() => Destroy(this.gameObject));
    }
}

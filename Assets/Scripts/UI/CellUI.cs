using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CellUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private AspectRatioFitter _imageAspectRationFitter;
    [SerializeField] private Button _checkKeyButton;
    private Func<bool> _checkKey;

    public void Show(Sprite sprite, Func<bool> checkKey)
    {
        _image.sprite = sprite;
        _imageAspectRationFitter.aspectRatio = sprite.rect.width / sprite.rect.height;
        _checkKey = checkKey;
    }

    private void Awake()
    {
        _checkKeyButton.onClick.AddListener(() =>
        {
            if (!_checkKey())
                _image.transform.DOShakePosition(.5f, new Vector3(10f, 0f, 0f), 10).OnComplete(() =>
                {
                    _image.transform.DOLocalMove(Vector3.zero, .5f);
                }); ;
        });
    }
}

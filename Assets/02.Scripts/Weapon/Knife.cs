using UnityEngine;
using DG.Tweening;

public class Knife : BaseMelee
{
    private float _attackDuration = 0.3f;
    private float _sweepDistance = 1f;
    private Vector3 _originalWorldPosition;
    private Vector3 _originalLocalPosition;

    private void Start()
    {
        _originalLocalPosition = transform.localPosition;
        PlayerEventManager.Instance.OnSwordAttack += Attack;
    }

    public override void AttackAnimation()
    {
        _isAnimation = true;

        Sequence attackSequence = DOTween.Sequence();

        //if (CameraManager.I.FPSCamera.enabled)
        //{
        //    _originalWorldPosition = transform.position;
        //    attackSequence.Append(transform.DOMove(
        //        _originalWorldPosition + transform.TransformDirection(Vector3.left) * _sweepDistance,
        //        _attackDuration / 4
        //    ).SetEase(Ease.OutQuad));

        //    attackSequence.Append(transform.DOMove(
        //        _originalWorldPosition + transform.TransformDirection(Vector3.right) * _sweepDistance,
        //        _attackDuration / 2
        //    ).SetEase(Ease.InQuad));

        //    attackSequence.Append(transform.DOMove(
        //        _originalWorldPosition,
        //        _attackDuration / 4
        //    ).SetEase(Ease.InQuad));
        //}

        //else
        //{
             _originalLocalPosition = transform.localPosition;
            attackSequence.Append(transform.DOLocalMove(
                transform.localPosition + Vector3.left * _sweepDistance,
                _attackDuration / 4
            ).SetEase(Ease.OutQuad));

            attackSequence.Append(transform.DOLocalMove(
                transform.localPosition + Vector3.right * _sweepDistance,
                _attackDuration / 2
            ).SetEase(Ease.InQuad));

            attackSequence.Append(transform.DOLocalMove(
                _originalLocalPosition,
                _attackDuration / 4
            ).SetEase(Ease.InQuad));
        //}



        attackSequence.OnComplete(() => {
            _isAnimation = false;
        });
    }
}

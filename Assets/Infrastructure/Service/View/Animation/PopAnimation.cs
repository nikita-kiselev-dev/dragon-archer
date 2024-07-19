using DG.Tweening;
using UnityEngine;

namespace Infrastructure.Service.View.Animation
{
    public class PopAnimation : MonoBehaviour, IAnimation
    {
        private const float HalfLoopDuration = 1.5f;
        private readonly Vector3 _endScale = new(1.2f, 1.2f, 1.2f);

        private Sequence _animation;
        
        private void OnEnable()
        {
            _animation = Animate().Play();
        }

        private void OnDestroy()
        {
            _animation.Kill();
        }

        private Sequence Animate()
        {
            var sequence = DOTween.Sequence();
            
            sequence
                .Append(transform.DOScale(_endScale, HalfLoopDuration))
                .Append(transform.DOScale(Vector3.one, HalfLoopDuration))
                .SetEase(Ease.InSine)
                .SetLoops(-1);
            
            return sequence;
        }
    }
}
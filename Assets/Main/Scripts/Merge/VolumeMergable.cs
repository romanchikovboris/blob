using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BlobGame
{
    [RequireComponent(typeof(VolumeCalculator))]
    public class VolumeMergable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private VolumeCalculator _myVolumeCalculator;

        [SerializeField]
        private Collider _myCollider;

        [SerializeField]
        private Rigidbody _myRigidbody;

        private bool _isMerged;
        private GameConfig _gameConfig;

        private void OnValidate()
        {
            if (TryGetComponent<VolumeCalculator>(out var volumeCalculator))
            {
                _myVolumeCalculator = volumeCalculator;
            }
            
            if (TryGetComponent<Collider>(out var coll))
            {
                _myCollider = coll;
            }
            
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                _myRigidbody = rb;
            }
        }

        [Inject]
        private void Construct(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public void Interact(MonoBehaviour interactionInvoker)
        {
            if (CanBeMergedWith(interactionInvoker, out var volume))
            {
                MergeVolumeWith(volume, interactionInvoker);
                Debug.Log($"{gameObject.name} merged with {interactionInvoker.name}!");
            }
        }

        private bool CanBeMergedWith(MonoBehaviour interactor, out float volume)
        {
            if (interactor.TryGetComponent<VolumeCalculator>(out var interactorVolumeCalculator) && !_isMerged)
            {
                var myVolume = _myVolumeCalculator.GetVolume();
                if (interactorVolumeCalculator.GetVolume() >= myVolume)
                {
                    volume = _myVolumeCalculator.GetVolume();
                    return true;
                }
            }

            volume = 0f;
            return false;
        }

        private void MergeVolumeWith(float volume, MonoBehaviour interactor)
        {
            if (interactor.TryGetComponent<VolumeMerger>(out var merger))
            {
                _isMerged = true;
                merger.MergeWithVolume(volume);
                
                _myCollider.enabled = false;
            
                if(_myRigidbody != null)
                    _myRigidbody.isKinematic = true;

                var animateSequence = AnimateMerged();
                animateSequence.OnComplete(() =>
                {
                    Destroy(gameObject, 3f);
                });
            }
            else
            {
                Debug.LogError($"Can be merged with {interactor.gameObject.name}, Add Merger script to it!");
            }
        }

        private Sequence AnimateMerged()
        {
            var sequence = DOTween.Sequence();

            var animationUpOffset = transform.position + Vector3.up * _gameConfig.MergeAnimationYOffset;
            var moveTween = transform.DOMove(animationUpOffset, _gameConfig.MergeAnimationTime);
            var scaleTween = transform.DOScale(0f, _gameConfig.MergeAnimationTime);

            sequence.Append(moveTween).Join(scaleTween);

            return sequence;
        }
    }
}
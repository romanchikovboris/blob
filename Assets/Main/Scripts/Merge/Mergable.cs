using System;
using UnityEngine;

namespace BlobGame
{
    [RequireComponent(typeof(VolumeCalculator))]
    public class Mergable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private VolumeCalculator _myVolumeCalculator;

        [SerializeField]
        private Collider _myCollider;

        [SerializeField]
        private Rigidbody _myRigidbody;

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

        public void Interact(MonoBehaviour interactionInvoker)
        {
            if (CanBeMergedWith(interactionInvoker))
            {
                MergeWith(interactionInvoker);
                Debug.Log($"{gameObject.name} merged with {interactionInvoker.name}!");
            }
        }

        private bool CanBeMergedWith(MonoBehaviour interactor)
        {
            if (interactor.TryGetComponent<VolumeCalculator>(out var interactorVolumeCalculator))
            {
                if (interactorVolumeCalculator.GetVolume() >= _myVolumeCalculator.GetVolume())
                {
                    return true;
                }
            }

            return false;
        }

        private void MergeWith(MonoBehaviour interactor)
        {
            var currentScale = interactor.transform.localScale;
            var targetScale = currentScale * 1.2f;
            interactor.transform.localScale = targetScale;

            _myCollider.enabled = false;
            
            if(_myRigidbody != null)
                _myRigidbody.isKinematic = true;
            
            Destroy(gameObject, 3f);
        }
    }
}
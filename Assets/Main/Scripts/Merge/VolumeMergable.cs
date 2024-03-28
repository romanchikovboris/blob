using System;
using UnityEngine;

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
            
                //animation?
                Destroy(gameObject, 3f);
            }
            else
            {
                Debug.LogError($"Can be merged with {interactor.gameObject.name}, Add Merger script to it!");
            }
        }
        
        
    }
}
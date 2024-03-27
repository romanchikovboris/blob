using UnityEngine;

namespace BlobGame
{
    public interface IInteractable
    {
        void Interact(MonoBehaviour interactionInvoker);
    }
}
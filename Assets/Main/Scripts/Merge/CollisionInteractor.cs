using Obi;
using UnityEngine;

namespace BlobGame
{
    public class CollisionInteractor : MonoBehaviour
    {
        [SerializeField]
        private ObiSolver _solver;
        void OnEnable()
        {
            _solver.OnCollision += Solver_OnCollision;
        }

        private void OnDisable()
        {
            _solver.OnCollision -= Solver_OnCollision;
        }
        
        private void Solver_OnCollision(ObiSolver s, ObiSolver.ObiCollisionEventArgs e)
        {
            var world = ObiColliderWorld.GetInstance();
            foreach (Oni.Contact contact in e.contacts)
            {
                // look for actual contacts only:
                if (contact.distance > 0.01)
                {
                    var col = world.colliderHandles[contact.bodyB].owner;

                    if (col.gameObject.TryGetComponent<IInteractable>(out var interactable))
                    {
                        interactable.Interact(this);
                    }
                }
            }
        }
    }
}
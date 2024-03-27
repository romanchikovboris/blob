using Obi;
using UnityEngine;
using Zenject;

namespace BlobGame
{
    public class SoftBodyMovable : MonoBehaviour, IMovable
    {
        [SerializeField]
        private ObiSoftbody _softBody;

        private GameConfig gameConfig;
        private bool onGround = false;

        [Inject]
        private void Construct(GameConfig config)
        {
            gameConfig = config;
        }

        private void Start()
        {
            _softBody.solver.OnCollision += Solver_OnCollision;
        }

        private void OnDestroy()
        {
            _softBody.solver.OnCollision -= Solver_OnCollision;
        }
        
        public void Move(Vector3 dir)
        {
            // apply ground/air movement:
            float effectiveAcceleration = gameConfig.Acceleration;

            if (!onGround)
                effectiveAcceleration *= gameConfig.AirControl;

            _softBody.AddForce(dir * effectiveAcceleration, ForceMode.Acceleration);

            // jump:
            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                onGround = false;
                _softBody.AddForce(Vector3.up * gameConfig.JumpPower, ForceMode.VelocityChange);
            }
        }
        
        private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
        {
            onGround = false;

            var world = ObiColliderWorld.GetInstance();
            foreach (Oni.Contact contact in e.contacts)
            {
                // look for actual contacts only:
                {
                    var col = world.colliderHandles[contact.bodyB].owner;

                    if (col != null)
                    {
                        onGround = true;
                        return;
                    }
                }
            }
        }
    }
    

}
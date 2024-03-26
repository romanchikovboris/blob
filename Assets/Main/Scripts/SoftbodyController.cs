using System.Collections;
using System.Collections.Generic;
using BlobGame;
using UnityEngine;
using Obi;
using Zenject;

[RequireComponent(typeof(ObiSoftbody))]
public class SoftbodyController : MonoBehaviour
{
    //TODO: move to player movement settings
    public float acceleration = 80;
    public float jumpPower = 1;

    [Range(0,1)]
    public float airControl = 0.3f;
    
    [SerializeField]
    private Transform _camera;
    
    private ObiSoftbody softbody;
    private bool onGround = false;
    private IInputService _inputService;

    [Inject]
    private void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    // Start is called before the first frame update
    void Start()
    {
        softbody = GetComponent<ObiSoftbody>();
        softbody.solver.OnCollision += Solver_OnCollision;
    }

    private void OnDestroy()
    {
        softbody.solver.OnCollision -= Solver_OnCollision;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera != null)
        {
            Vector3 direction = Vector3.zero;
            if (_inputService.Axis.sqrMagnitude > Mathf.Epsilon)
            {
                direction = _camera.transform.TransformDirection(_inputService.Axis) * acceleration;
                direction.y = 0;
                direction.Normalize();
            }

            /*// Determine movement direction:
            if (Input.GetKey(KeyCode.W))
            {
                direction += referenceFrame.forward * acceleration;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += -referenceFrame.right * acceleration;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += -referenceFrame.forward * acceleration;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += referenceFrame.right * acceleration;
            }

            // flatten out the direction so that it's parallel to the ground:
            direction.y = 0;*/

            // apply ground/air movement:
            float effectiveAcceleration = acceleration;

            if (!onGround)
                effectiveAcceleration *= airControl;

            softbody.AddForce(direction.normalized * effectiveAcceleration, ForceMode.Acceleration);

            // jump:
            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                onGround = false;
                softbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            }
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

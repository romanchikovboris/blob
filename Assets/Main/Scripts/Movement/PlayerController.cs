using BlobGame;
using UnityEngine;
using Obi;
using Zenject;

[RequireComponent(typeof(ObiSoftbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;

    private IInputService _inputService;
    private IMovable _movable;

    [Inject]
    private void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera != null)
        {
            Vector3 direction = Vector3.zero;
            if (_inputService.Axis.sqrMagnitude > Mathf.Epsilon)
            {
                direction = _camera.transform.TransformDirection(_inputService.Axis);
                direction.y = 0;
                direction.Normalize();
            }
            
            _movable.Move(direction);
        }
    }
}

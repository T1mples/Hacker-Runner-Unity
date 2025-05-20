using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private VictoryManager _victoryManagerScript;
    [SerializeField] private StaticJoystick _staticJoystick;

    [SerializeField] private Transform _leftArm;
    [SerializeField] private Transform _rightArm;
    [SerializeField] private Transform _rightLeg;
    [SerializeField] private Transform _leftLeg;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _resetSpeed;
    [SerializeField] private float _maxTurnAngle;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _targetRotationSpeed;

    [SerializeField] private AudioSource _audioSource;

    private float _animationTimer;

    private Vector3 _initialMousePosition;
    private Vector3 _targetPosition = new Vector3(0, 0.052f, 292.33f);

    private Vector3 _previousPosition;
    private bool _isMoving;
    private bool _moveToTarget = false;
    private bool _rotateAtTarget = false;
    private bool _smoothReset = false;
    private bool _rotateRightArm = false;

    private float _leftLegTargetAngle;
    private float _rightLegTargetAngle;


    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 1)
        {
            MoveOnPC();
        } else
        {
            MoveOnMobile();
        }
    }

    private void MoveOnPC()
    {
        if (GameManager.IsGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _initialMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                MoveTowardsMouse();
            }

            if (transform.position != _previousPosition)
            {
                _isMoving = true;
                _previousPosition = transform.position;
            }
            else
            {
                _isMoving = false;
                SmoothResetAnimation();
            }

            if (_isMoving)
            {
                AnimateMovement();
            }

            LimitPosition();
        }
        else if (_moveToTarget && !PlayerDeathManager.IsDeath)
        {
            MoveToTarget();
        }
        else if (_rotateAtTarget)
        {
            RotateAtTarget();
        }
        else if (_smoothReset)
        {
            SmoothResetAfterRotation();
        }
    }

    private void MoveOnMobile()
    {
        if (GameManager.IsGameStarted)
        {
            MoveWithJoystick();

            if (transform.position != _previousPosition)
            {
                _isMoving = true;
                _previousPosition = transform.position;
            }
            else
            {
                _isMoving = false;
                SmoothResetAnimation();
            }

            if (_isMoving)
            {
                AnimateMovement();
            }

            LimitPosition();
        }
        else if (_moveToTarget && !PlayerDeathManager.IsDeath)
        {
            MoveToTarget();
        }
        else if (_rotateAtTarget)
        {
            RotateAtTarget();
        }
        else if (_smoothReset)
        {
            SmoothResetAfterRotation();
        }
    }

    private void MoveTowardsMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            Vector3 direction = (targetPoint - transform.position).normalized;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            targetAngle = Mathf.Clamp(targetAngle, -_maxTurnAngle, _maxTurnAngle);
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }
    }

    private void MoveWithJoystick()
    {
        Vector3 direction = new Vector3(_staticJoystick.InputDirection.x, 0, _staticJoystick.InputDirection.y);
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            targetAngle = Mathf.Clamp(targetAngle, -_maxTurnAngle, _maxTurnAngle);

            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * _rotationSpeed);
            Quaternion targetRotation = Quaternion.Euler(0, smoothAngle, 0);

            transform.rotation = targetRotation;
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }
    }

    private void LimitPosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -4.093f, 4.093f);
        transform.position = position;
    }

    private void AnimateMovement()
    {
        _animationTimer += Time.deltaTime;

        float armTargetAngle = Mathf.Lerp(60, 120, Mathf.PingPong(_animationTimer * 2, 1));
        _rightLegTargetAngle = Mathf.Lerp(-35, 35, Mathf.PingPong(_animationTimer * 2, 1));
        _leftLegTargetAngle = Mathf.Lerp(35, -35, Mathf.PingPong(_animationTimer * 2, 1));

        _leftArm.localRotation = Quaternion.Euler(armTargetAngle, 0, 0);
        _rightLeg.localRotation = Quaternion.Euler(_rightLegTargetAngle, 0, 0);
        _leftLeg.localRotation = Quaternion.Euler(_leftLegTargetAngle, 0, 0);

        CheckLegPositions();
    }

    private void CheckLegPositions()
    {
        if (_leftLegTargetAngle >= 30f || _leftLegTargetAngle <= -30f)
        {
            _audioSource.Play();
        }

        if (_rightLegTargetAngle >= 30f || _rightLegTargetAngle <= -30f)
        {
            _audioSource.Play();
        }
    }

    private void SmoothResetAnimation()
    {
        _animationTimer += Time.deltaTime;

        _leftArm.localRotation = Quaternion.Lerp(_leftArm.localRotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * _resetSpeed);
        _rightLeg.localRotation = Quaternion.Lerp(_rightLeg.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);
        _leftLeg.localRotation = Quaternion.Lerp(_leftLeg.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);
    }

    private void MoveToTarget()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _targetPosition);

        if (distance > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
            AnimateMovement();
        }
        else
        {
            _moveToTarget = false;
            _rotateAtTarget = true;
        }
    }

    private void RotateAtTarget()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 180, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _targetRotationSpeed * Time.deltaTime);

        if (!_rotateRightArm)
        {
            _rotateRightArm = true;
        }

        if (_rotateRightArm)
        {
            RotateRightArm();
        }

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            _rotateAtTarget = false;
            _smoothReset = true;
        }
        else
        {
            AnimateMovement();
        }
    }

    private void RotateRightArm()
    {
        Quaternion armTargetRotation = Quaternion.Euler(90, 0, 0);
        _rightArm.localRotation = Quaternion.RotateTowards(_rightArm.localRotation, armTargetRotation, _targetRotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(_rightArm.localRotation, armTargetRotation) < 1f)
        {
            _rotateRightArm = false;
        }
    }

    private void SmoothResetAfterRotation()
    {
        _animationTimer += Time.deltaTime;

        _leftArm.localRotation = Quaternion.Lerp(_leftArm.localRotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * _resetSpeed);
        _rightLeg.localRotation = Quaternion.Lerp(_rightLeg.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);
        _leftLeg.localRotation = Quaternion.Lerp(_leftLeg.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);

        if (Quaternion.Angle(_leftArm.localRotation, Quaternion.Euler(90, 0, 0)) < 0.1f &&
            Quaternion.Angle(_rightLeg.localRotation, Quaternion.Euler(0, 0, 0)) < 0.1f &&
            Quaternion.Angle(_leftLeg.localRotation, Quaternion.Euler(0, 0, 0)) < 0.1f)
        {
            _smoothReset = false;
            _victoryManagerScript.OpenVictory();
        }
    }

    public void OnFinishTrigger()
    {
        GameManager.IsGameStarted = false;
        _moveToTarget = true;
    }
}

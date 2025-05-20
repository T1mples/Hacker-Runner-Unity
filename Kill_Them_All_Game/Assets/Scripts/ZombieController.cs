using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private Animator _zombieAnimator;
    [SerializeField] private GameObject _meshZombiePrefab;

    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private DamageController _canvasController;
    private AudioClip _soundClip;

    private bool _isChasingPlayer = false;
    private bool _isInPunchTrigger = false;
    private bool _isCoroutineRunning = false;

    public FollowZombie FollowScript { get; private set; }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _canvasController = FindAnyObjectByType<DamageController>();
        _player = FindAnyObjectByType<PlayerController>().transform;

        _soundClip = Resources.Load<AudioClip>("ZombieIn");

        SpawnMeshZombie();
    }

    private void Update()
    {
        SetDestinationToPlayer();
        LimitPosition();

        if (PlayerDeathManager.IsDeath)
        {
            gameObject.SetActive(false);
        }
    }

    private void SetDestinationToPlayer()
    {
        if (_isChasingPlayer)
        {
            _navMeshAgent.SetDestination(_player.position);
        }
    }

    private void LimitPosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -4.093f, 4.093f);
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTriggerForWalk"))
        {
            PlaySound();
            _isChasingPlayer = true;
            _zombieAnimator.SetBool("Run", true);
        }
        else if (other.CompareTag("PlayerTriggerForPunch"))
        {
            _zombieAnimator.SetBool("Run", false);
            _zombieAnimator.SetBool("Punch", true);
            _isInPunchTrigger = true;

            if (!_isCoroutineRunning)
            {
                StartCoroutine(FadeAndDamageCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerTriggerForWalkExit"))
        {
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("PlayerTriggerForPunch"))
        {
            _zombieAnimator.SetBool("Run", true);
            _zombieAnimator.SetBool("Punch", false);
            _isInPunchTrigger = false;
        }
    }

    private IEnumerator FadeAndDamageCoroutine()
    {
        _isCoroutineRunning = true;

        while (_isInPunchTrigger)
        {
            yield return new WaitForSeconds(_canvasController.TimeBeforeFadeDamage);
            if (_isInPunchTrigger)
            {
                _canvasController.StartFadeCoroutine();
                _canvasController.UpdateHealth();
            }
        }

        _isCoroutineRunning = false;
    }

    private void SpawnMeshZombie()
    {
        GameObject meshZombie = Instantiate(_meshZombiePrefab);

        FollowScript = meshZombie.GetComponent<FollowZombie>();

        if (FollowScript != null)
        {
            FollowScript.SetFollowTarget(transform, new Vector3(0, 1.32f, -0.13f));
        }
    }

    private void PlaySound()
    {
        if (SoundManager.IsSoundOff)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = _soundClip;
            FocusManager.Instance.RegisterAudioSource(audioSource);
            audioSource.Play();
            StartCoroutine(RemoveAudioSourceAfterPlay(audioSource, _soundClip.length));
        }
    }

    private IEnumerator RemoveAudioSourceAfterPlay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        FocusManager.Instance.RemoveAudioSource(audioSource);
        Destroy(audioSource);
    }
}

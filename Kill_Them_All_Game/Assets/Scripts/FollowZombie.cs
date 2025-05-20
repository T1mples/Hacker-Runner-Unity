using UnityEngine;

public class FollowZombie : MonoBehaviour
{
    private Transform _zombieTransform;
    private Vector3 _offset;
    private MeshRenderer _meshRenderer;
    private ScoreManager _scoreManagerScript;


    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _scoreManagerScript = FindAnyObjectByType<ScoreManager>();
    }

    private void Update()
    {
        SetDestinationToZombie();
    }

    private void SetDestinationToZombie()
    {
        if (_zombieTransform != null)
        {
            transform.position = _zombieTransform.position + _offset;
        }
    }

    public void SetFollowTarget(Transform zombieTransform, Vector3 offset)
    {
        _zombieTransform = zombieTransform;
        _offset = offset;

        if (!CuttingManager.Instance.KesobjList.Contains(gameObject))
        {
            CuttingManager.Instance.KesobjList.Add(gameObject);
        }
    }

    public void MakeVisible()
    {
        if (_meshRenderer != null)
        {
            Color color = _meshRenderer.material.color;
            color.a = 1.0f;
            _meshRenderer.material.color = color;
        }
    }

    public void HideZombie()
    {
        if (_zombieTransform != null)
        {
            _zombieTransform.gameObject.SetActive(false);
            _scoreManagerScript.AddScore(1);
        }
    }
}

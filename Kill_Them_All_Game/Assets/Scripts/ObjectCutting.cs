using UnityEngine;
using EzySlice;
using System.Collections;

public class ObjectCutting : MonoBehaviour
{
    [SerializeField] private Material _materialOfCuttingObjects;
    [SerializeField] private float _forceStrength;

    private void Update()
    {
        LimitPosition();
    }

    private void LimitPosition()
    {
        foreach (GameObject obj in CuttingManager.Instance.KesobjList)
        {
            if (obj != null)
            {
                Vector3 position = obj.transform.position;
                position.x = Mathf.Clamp(position.x, -4.093f, 4.093f);
                obj.transform.position = position;
            }
        }
    }

    private void AddComponentsAndToList(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        obj.layer = LayerMask.NameToLayer("Cut");
        CuttingManager.Instance.KesobjList.Add(obj);

        Vector3 forceDirection = transform.forward;
        rb.AddForce(forceDirection * _forceStrength, ForceMode.Impulse);

        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 1)
        {
            StartCoroutine(RemoveObjectAfterTime(obj, 10f));
        } else {
            StartCoroutine(RemoveObjectAfterTime(obj, 3f));
        }
    }

    private void SetMaterialOpacity(GameObject obj, float opacity)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = opacity;
            material.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cut"))
        {
            _materialOfCuttingObjects = other.GetComponent<MeshRenderer>().material;
            if (!CuttingManager.Instance.KesobjList.Contains(other.gameObject))
            {
                CuttingManager.Instance.KesobjList.Add(other.gameObject);
            }
        }
    }

    private IEnumerator RemoveObjectAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        CuttingManager.Instance.KesobjList.Remove(obj);
        Destroy(obj);
    }

    public void CutObjects()
    {
        for (int i = CuttingManager.Instance.KesobjList.Count - 1; i >= 0; i--)
        {
            GameObject kesobj = CuttingManager.Instance.KesobjList[i];
            if (kesobj != null)
            {
                if (Vector3.Distance(kesobj.transform.position, transform.position) <= 2f)
                {
                    AudioManager.Instance.PlayAudioCut();

                    SlicedHull kesilen = Kes(kesobj, _materialOfCuttingObjects);
                    if (kesilen != null)
                    {
                        GameObject kesilenUst = kesilen.CreateUpperHull(kesobj, _materialOfCuttingObjects);
                        if (kesilenUst != null)
                        {
                            AddComponentsAndToList(kesilenUst);
                            SetMaterialOpacity(kesilenUst, 1.0f);
                        }

                        GameObject kesilenAlt = kesilen.CreateLowerHull(kesobj, _materialOfCuttingObjects);
                        if (kesilenAlt != null)
                        {
                            AddComponentsAndToList(kesilenAlt);
                            SetMaterialOpacity(kesilenAlt, 1.0f);
                        }

                        FollowZombie followZombie = kesobj.GetComponent<FollowZombie>();
                        if (followZombie != null)
                        {
                            followZombie.HideZombie();
                        }

                        CuttingManager.Instance.KesobjList.RemoveAt(i);
                        Destroy(kesobj);
                    }
                }
            }
        }
    }

    public SlicedHull Kes(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }
}

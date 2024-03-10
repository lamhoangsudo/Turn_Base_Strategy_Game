using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(Material material)
    {
        meshRenderer.material = material;
        Debug.Log(meshRenderer.material);
        gameObject.SetActive(true);
    }
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}

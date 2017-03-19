using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEffect : MonoBehaviour, IHighlightable {

    GameObject highlight;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public Material edgeMaterial;

    public void OnHighlighted()
    {
        highlight = new GameObject(transform.name + "_HighlightEffect");
        highlight.transform.parent = transform;
        highlight.transform.localPosition = Vector3.zero;
        highlight.transform.localScale = Vector3.one;
        highlight.transform.rotation = Quaternion.identity;

        meshFilter = highlight.AddComponent<MeshFilter>();
        meshRenderer = highlight.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.material = edgeMaterial;
    }

    public void StoppedHighlighting()
    {
        Destroy(highlight);
    }
}

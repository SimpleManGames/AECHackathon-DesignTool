using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableMesh : MonoBehaviour
{

    #region Variables

    #region Debug

#if UNITY_EDITOR

    public DEBUG_LEVEL debug = DEBUG_LEVEL.None;

#endif

    #endregion

    #region Constants

    // The grid has been created with 24x24 vertices all 0.1 meters apart
    public const int VERTEX_ROW_COUNT = 24;
    public const int VERTEX_COL_COUNT = 24;

    #endregion

    #region MeshInfo

    // Sub tris in use
    List<List<int>> subTris = new List<List<int>>();

    #endregion

    #region Components

    // Mesh information for vertex manipulation
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public Material invisibleMaterial;

    #endregion

    #endregion

    #region Functions

    #region Unity

    // Use this for initialization
    void Start ()
    {
        GetMesh();
	}

    #endregion

    #region PublicMeshCreationMethods

    public void AddSubMesh(Vector3[] verts)
    {
        Bounds aabb = new Bounds();

        aabb.center = GlobalMethods.AverageVector(verts);
        foreach (Vector3 v in verts) aabb.Encapsulate(v);

#if UNITY_EDITOR

        if (debug != DEBUG_LEVEL.None) Debug.Log("The AABB center is: " + aabb.center + ", the extents are: " + aabb.extents);

#endif

        subTris.Add(new List<int>());
        mesh.subMeshCount++;

        List<int> tris = new List<int>();
        for(int i = 0; i < mesh.triangles.Length; i+=3)
        {

#if UNITY_EDITOR

            if (debug == DEBUG_LEVEL.Advanced)
                Debug.Log(("Testing AABB against these three points: " + transform.TransformPoint(mesh.vertices[mesh.triangles[i]]) +
                           ", " + transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]) +
                           ", " + transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]])));

#endif

            if( aabb.Contains(transform.TransformPoint(mesh.vertices[mesh.triangles[i]]))   ||
                aabb.Contains(transform.TransformPoint(mesh.vertices[mesh.triangles[i+1]])) ||
                aabb.Contains(transform.TransformPoint(mesh.vertices[mesh.triangles[i+2]])) )
            {

#if UNITY_EDITOR

                if (debug == DEBUG_LEVEL.Advanced) Debug.Log("AABB Test returned true! Adding triangle.");

#endif
                tris.Add(mesh.triangles[i]);
                tris.Add(mesh.triangles[i+1]);
                tris.Add(mesh.triangles[i+2]);

                subTris[subTris.Count - 1].Add(mesh.triangles[i]);
                subTris[subTris.Count - 1].Add(mesh.triangles[i+1]);
                subTris[subTris.Count - 1].Add(mesh.triangles[i+2]);
            }
        }

        mesh.SetTriangles(tris.ToArray(), mesh.subMeshCount - 1);
        SetMainMeshTris(true);
    }

    public void RemoveSubMesh(int subMesh)
    {
        if (mesh.subMeshCount > 0)
        {
            subTris.RemoveAt(subMesh - 1);

            List<int> newMainTris = new List<int>(mesh.GetTriangles(0));
            newMainTris.AddRange(mesh.GetTriangles(subMesh));

            mesh.SetTriangles(newMainTris, 0);

            SetMainMeshTris(false);

            mesh.subMeshCount--;
        }
    }

    public void RemoveAllSubMeshes()
    {
        if (mesh.subMeshCount > 0)
        {
            for (int i = 1; i < mesh.subMeshCount; ++i)
            {
                subTris.RemoveAt(i - 1);

                List<int> newMainTris = new List<int>(mesh.GetTriangles(0));
                newMainTris.AddRange(mesh.GetTriangles(i));

                mesh.SetTriangles(newMainTris, 0);

                SetMainMeshTris(false);

                mesh.subMeshCount--;
            }
        }
    }

    #endregion

    #region PrivateMeshCreationMethods

    void SetMainMeshTris(bool add)
    {
        List<int> mainMeshTris = new List<int>();

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            bool skip = false;

            foreach (var j in subTris)
                for (int k = 0; k < j.Count; k += 3)
                    if (mesh.triangles[i] == j[k] && mesh.triangles[i + 1] == j[k + 1] && mesh.triangles[i + 2] == j[k + 2])
                    {
                        skip = true;
                    }

            if (!skip)
            {
                mainMeshTris.Add(mesh.triangles[i]);
                mainMeshTris.Add(mesh.triangles[i + 1]);
                mainMeshTris.Add(mesh.triangles[i + 2]);
            }

        }

        mesh.SetTriangles(mainMeshTris.ToArray(), 0);

        List<Material> mats = new List<Material>(meshRenderer.materials);
        if (add) mats.Add(new Material(invisibleMaterial));
        else mats.RemoveAt(mats.Count - 1);

        meshRenderer.materials = mats.ToArray();
    }

    #endregion

    #region Init

    void GetMesh()
    { meshRenderer = GetComponent<MeshRenderer>(); meshFilter = GetComponent<MeshFilter>(); mesh = meshFilter.mesh; }

#endregion

#endregion

}

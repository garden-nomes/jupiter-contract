using UnityEditor;
using UnityEngine;

public class Starfield : MonoBehaviour
{
    public float distance = 10_000;
    public int count = 100;
    public Transform follow;

    private void Start()
    {

        var points = GenerateStars(distance, count);
        var mesh = CreatePointMesh(points);

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void LateUpdate()
    {
        transform.position = follow.position;
    }

    private Vector3[] GenerateStars(float distance, int count)
    {
        var stars = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            var pos = Random.onUnitSphere * distance;
            stars[i] = pos;
        }

        return stars;
    }

    private Mesh CreatePointMesh(Vector3[] points)
    {
        var mesh = new Mesh();
        mesh.vertices = points;

        int[] indices = new int[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            indices[i] = i;
        }
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        return mesh;
    }
}

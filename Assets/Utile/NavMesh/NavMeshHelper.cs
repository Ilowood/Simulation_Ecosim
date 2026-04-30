using UnityEngine;
using UnityEngine.AI;

namespace Untils
{
    public class NavMeshHelper
    {
        public static Vector3 GetRandomPoint()
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            if (navMeshData.indices.Length == 0 || navMeshData.vertices.Length == 0)
                return Vector3.zero;

            int triangleIndex = Random.Range(0, navMeshData.indices.Length / 3);

            var v1 = navMeshData.vertices[navMeshData.indices[triangleIndex * 3]];
            var v2 = navMeshData.vertices[navMeshData.indices[triangleIndex * 3 + 1]];
            var v3 = navMeshData.vertices[navMeshData.indices[triangleIndex * 3 + 2]];

            var u = Random.value;
            var v = Random.value;
            
            if (u + v > 1f)
            {
                u = 1f - u;
                v = 1f - v;
            }

            var randomPoint = v1 + u * (v2 - v1) + v * (v3 - v1);
            return randomPoint;
        }
    }
}

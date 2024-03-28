using UnityEngine;

namespace BlobGame
{
    //works only for uniform scaling!
    public class VolumeCalculator : MonoBehaviour
    {
        [SerializeField]
        private Mesh _mesh;

        private float _cachedVolume;

        private void OnValidate()
        {
            if (gameObject.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                _mesh = meshFilter.sharedMesh;
            }
            
            if (gameObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
            {
                _mesh = skinnedMeshRenderer.sharedMesh;
            }
        }

        [ContextMenu("Debug Volume")]
        public void DebugVolume()
        {
            var volume = GetVolume();
            Debug.Log($"Object {gameObject.name} volume is: {volume}");
        }
        
        public float GetVolume()
        {
            if (_cachedVolume == 0)
            {
                _cachedVolume = VolumeOfMesh(_mesh);
            }

            //calculate volume with respect of own + parent scale
            var parentScale = transform.parent? transform.parent.transform.localScale.x : 1f;
            return _cachedVolume * Mathf.Abs(transform.localScale.x) * parentScale;
        }

        private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;

            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        private float VolumeOfMesh(Mesh mesh)
        {
            float volume = 0;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }
            return Mathf.Abs(volume);
        }
    }
}
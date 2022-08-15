using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sebastian
{
    public class MapDisplay : MonoBehaviour
    {
        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        //public Mesh meshToCollide;

        private void Start()
        {
            //AddMesh();
        }
        public void AddMesh()
        {
            MeshCollider collider = meshRenderer.gameObject.GetComponent<MeshCollider>();
            if (collider) Destroy(collider);
            meshRenderer.gameObject.AddComponent<MeshCollider>();
        }
        public void DrawTexture(Texture2D texture)
        {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }
        
        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        //public void DrawMesh(MeshData meshData, Texture2D texture, bool buildMeshCollider)
        //{
        //    meshFilter.sharedMesh = meshData.CreateMesh();
        //    meshRenderer.sharedMaterial.mainTexture = texture;
        //    AddMesh();
        //}
    }
}
using UnityEngine;

  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  public class S03_CustomPolygonMesh_Star : MonoBehaviour
  {
      void Start()
      {
          Vector3[] vertices = new Vector3[11];
          
          {
              // 중앙
              vertices[0]=new Vector3(0,0,0);
                  
              // 오각형
              for(int i=0; i<5; i++)
              {
                  float degree = (360f / 5) *i;
                  float radians = degree * Mathf.Deg2Rad;

                  float x = Mathf.Cos(radians) * 1f;
                  float y = Mathf.Sin(radians) * 1f;

                  vertices[i + 1] = new Vector3(x, y, 0);
              }
              
              // 외부 삼각형
              for(int i=0; i<5; i++)
              {
                  float degree = -(360/5)/2 - (360f / 5) * i;
                  float radians = degree * Mathf.Deg2Rad;
                  
                  float x = Mathf.Cos(radians) * 1.5f;
                  float y = Mathf.Sin(radians) * 1.5f;

                  vertices[i + 6] = new Vector3(x, y, 0);
              }
          };

          int[] triangles = new int[]
          {
              // 오각형
              0, 2, 1,
              0, 3, 2, 
              0, 4, 3,
              0, 5, 4,
              0, 1, 5,
              
              // 외부 삼각형
              1, 6, 5, 
              5, 7, 4, 
              4, 8, 3, 
              3, 9, 2,
              2, 10, 1,
          };

          Mesh mesh = new Mesh();
          mesh.vertices = vertices;
          mesh.triangles = triangles;
          mesh.RecalculateNormals();

          GetComponent<MeshFilter>().mesh = mesh;
          GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      }
  }
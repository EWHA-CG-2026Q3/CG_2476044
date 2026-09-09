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

          // TODO 2: 정점 3개씩 묶어 삼각형들을 구성하세요
          int[] triangles = new int[]
          {
              
          };

          Mesh mesh = new Mesh();
          mesh.vertices = vertices;
          mesh.triangles = triangles;
          mesh.RecalculateNormals();

          GetComponent<MeshFilter>().mesh = mesh;
          GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      }
  }
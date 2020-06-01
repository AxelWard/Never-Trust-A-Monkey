using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class ObstacleGenerator : MonoBehaviour
{

    MeshGenerator generator;
    Quad[,] quads;

    public GameObject fenceObject;
    public GameObject[] obstacles;
    public float emptyChance;

    void Start()
    {
        generator = GetComponent<MeshGenerator>();
        quads = generator.GetQuads();

        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for(int i = 5; i < quads.GetLength(0) - 5; i++)
        {
            for(int j = 5; j < quads.GetLength(1) - 5; j++)
            {
                // Generate obstacles;
                float generate = Random.Range(0f, 100f);
                if(generate > emptyChance)
                {
                    // Get obstacle to generate
                    int selection = Random.Range(0, obstacles.Length);

                    // Generate selected obstacle
                    Vector3 location = quads[i, j].vert0;
                    GameObject generated = Instantiate(obstacles[selection]);
                    generated.transform.position = location;
                    if(selection == 0)
                    {
                        generated.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f), 0f));
                    }
                }
                
            }
        }

        for (int i = 1; i < quads.GetLength(0); i++)
        {
            for (int j = 1; j < quads.GetLength(1); j++)
            {
                if(i == 1)
                {
                    Vector3 location = quads[i, j].vert0;
                    location.y = 0;
                    GameObject fence = Instantiate(fenceObject);
                    fence.transform.position = location;
                    fence.transform.rotation = Quaternion.Euler(-90, -90, 0);
                }
                else if (i == quads.GetLength(0) - 1)
                {
                    Vector3 location = quads[i, j].vert0;
                    location.y = 0;
                    GameObject fence = Instantiate(fenceObject);
                    fence.transform.position = location;
                    fence.transform.rotation = Quaternion.Euler(-90, 90, 0);
                }
                else if (j == 1)
                {
                    Vector3 location = quads[i, j].vert0;
                    location.y = 0;
                    GameObject fence = Instantiate(fenceObject);
                    fence.transform.position = location;
                    fence.transform.rotation = Quaternion.Euler(-90, 0, 0);

                }
                else if(j == quads.GetLength(1) - 1)
                {
                    Vector3 location = quads[i, j].vert0;
                    location.y = 0;
                    GameObject fence = Instantiate(fenceObject);
                    fence.transform.position = location;
                    fence.transform.rotation = Quaternion.Euler(-90, 180, 0);
                }
            }
        }
    }
}

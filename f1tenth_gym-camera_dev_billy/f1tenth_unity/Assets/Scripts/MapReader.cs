using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MapReader : MonoBehaviour
{
    [SerializeField]
    private Texture2D map;

    [SerializeField]
    private GameObject wallObject;
    [SerializeField]
    private GameObject groundObject;
    [SerializeField]
    private Material material;
    [SerializeField]
    private float resolution;

    int counterr;


    List<List<(int, int)>> wallCollections = new List<List<(int, int)>>();

    void dfs(int i, int j, ref bool[,] isVisited, ref Color[] pix) 
    {
        if (i<0 || j<0 || i>= map.height || j>= map.width) 
        {
            return;
        }
        if (i==map.height/2 || j==map.width/2) 
        {
            return;
        }
        
        Color c = pix[i*map.width + j];
        if (c.Equals(Color.white) || isVisited[i,j]) {
            return;
        }
  
        isVisited[i, j] = true;
        wallCollections[wallCollections.Count-1].Add((i,j));
  
        dfs(i+1, j, ref isVisited, ref pix);
        dfs(i, j+1, ref isVisited, ref pix);
        dfs(i, j-1, ref isVisited, ref pix);
        dfs(i-1, j, ref isVisited, ref pix);        
        // dfs(i+1, j+1, ref isVisited, ref pix);
        // dfs(i-1, j-1, ref isVisited, ref pix);
        // dfs(i+1, j-1, ref isVisited, ref pix);
        // dfs(i-1, j+1, ref isVisited, ref pix);   
        counterr++;  
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Color[] pix = map.GetPixels();

        float worldX = map.width*resolution;
        float worldZ = map.height*resolution;

        Vector3[] spawnPositions = new Vector3[pix.Length];
        Vector3 startingSpawnPosition = new Vector3(Mathf.Round(worldX/2),0f,-Mathf.Round(worldZ/2));
        Vector3 currentSpawnPosition = startingSpawnPosition;

        int counter = 0;
        int bcount=0, wcount=0;
        bool[ , ] isVisited = new bool[map.height, map.width];
        for (int x = 0; x<map.height; x++)
        {
            for (int z = 0; z<map.width; z++)
            {
                Color c = pix[x*map.width + z]; // get the current color
                if (x==z) {
                    c = Color.white;
                }
                
                if (!c.Equals(Color.white) && !isVisited[x, z]) // is wall
                { 
                    wallCollections.Add(new List<(int, int)>());
                    dfs(x, z, ref isVisited, ref pix);
                }
                spawnPositions[counter] = currentSpawnPosition;
                counter++;
                currentSpawnPosition.z = currentSpawnPosition.z+resolution;
            }
            currentSpawnPosition.z = startingSpawnPosition.z;
            currentSpawnPosition.x = currentSpawnPosition.x-resolution;
        }
        Debug.Log(counterr);

        Debug.Log(wallCollections[0].Count);
        Debug.Log(wallCollections[1].Count);

        // CombineInstance[] combine = new CombineInstance[objectList.Count];
        List<List<GameObject>> objectLists = new List<List<GameObject>>();
        
        // from wallCollections to objectCollections
        foreach (List<(int,int)> wallCollection in wallCollections) {
            objectLists.Add(new List<GameObject>());
            foreach((int,int) wall in wallCollection) {
                objectLists[objectLists.Count-1].Add(Instantiate(wallObject, spawnPositions[wall.Item1*map.width+wall.Item2], Quaternion.identity));
            }
        }
        
        GameObject objToSpawn;
        for (int i=0; i<wallCollections.Count; i++)
        {
            counter = 0;
            objToSpawn = new GameObject("Wall Collection");
            MeshFilter meshFilter = objToSpawn.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = objToSpawn.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            CombineInstance[] combine = new CombineInstance[objectLists[i].Count];
            foreach (GameObject objects in objectLists[i]) {

                meshFilter =  (MeshFilter)objects.GetComponent("MeshFilter");
                combine[counter].mesh = meshFilter.sharedMesh;
                combine[counter].transform =  meshFilter.transform.localToWorldMatrix;
                meshFilter.gameObject.SetActive(false);
                counter++;
            }
        
            objToSpawn.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            objToSpawn.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            objToSpawn.transform.gameObject.SetActive(true);
        }
                 
         
        // for (int i=0; i<spawnPositions.Length; i=i+1)
        // {
        //     Color c = pix[i];
        //     if (c.Equals(Color.black))
        //     {
        //         objectList.Add(Instantiate(wallObject, spawnPositions[i], Quaternion.identity));
        //         // objectList.Add(Instantiate(groundObject, spawnPositions[i], Quaternion.identity));
        //     }
        //     // if (c.Equals(Color.white))
        //     // {
        //     //     objectList.Add(Instantiate(groundObject, spawnPositions[i], Quaternion.identity));
        //     // }
        //     // objectList.Add(Instantiate(groundObject, spawnPositions[i], Quaternion.identity));

        // }
       
        // CombineInstance[] combine = new CombineInstance[objectList.Count];
        // MeshFilter meshFilters;
        // Debug.Log(wallCollections.Count);
        // // Debug.Log(objectList.Count);
        // // Debug.Log(counter);
        // for (int i=0; i< objectList.Count; i++) {
        //     meshFilters =  (MeshFilter)objectList[i].GetComponent("MeshFilter");
        //     combine[i].mesh = meshFilters.sharedMesh;
        //     combine[i].transform =  meshFilters.transform.localToWorldMatrix;
        //     meshFilters.gameObject.SetActive(false);
        // }
        
        // transform.GetComponent<MeshFilter>().mesh = new Mesh();
        // transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        // transform.gameObject.SetActive(true);

        // foreach (Vector3 pos in spawnPositions)
        // {
        //     Color c = pix[counter];

        //     // if (c.Equals(Color.white))
        //     // {
        //     //     Instantiate(groundObject, pos, Quaternion.identity);
        //     // }
        //     if (c.Equals(Color.black))
        //     {
        //         Instantiate(wallObject, pos, Quaternion.identity);
        //     }

        //     counter++;
        // }

        // for(int i = 0; i< objectList.Count; i++){
        //     PrefabUtility.SaveAsPrefabAsset(objectList[i], "Assets/Prefabs/levine.prefab");
        // }


    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

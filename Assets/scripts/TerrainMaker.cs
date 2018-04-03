
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class TerrainMaker : MonoBehaviour, UniRx.IObserver<UniRx.Tuple<float, float, float>> {

    public Terrain terrain;

    Terrain pTerrain;

    private UniRx.Subject<UniRx.Tuple<float, float, float>> mainCharacterPosStream;

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(Tuple<float, float, float> value)
    {

        Debug.Log("Received: " + value.Item1.ToString() + ", " + value.Item2.ToString() + ", " + value.Item3.ToString());

    }
 
    // Use this for initialization
    void Start () {

       mainCharacterPosStream = ScriptReactor.Instance.getCharacterStream();
       mainCharacterPosStream.SubscribeOn(Scheduler.ThreadPool).Subscribe(this);

       mainCharacterPosStream.OnNext(Tuple.Create(1.0f, 2.0f, 3.0f));
       //////////////////////////////////////////

       TerrainData td = this.terrain.terrainData;

        float terrainWidth = td.size.x;
        float terrainHeight = td.size.y;

        Vector3 vec = this.terrain.GetPosition();

        float x = vec.x;
        float y = vec.y;
        float z = vec.z;

        // need to change resolution before setting the size
        int cx = this.terrain.terrainData.heightmapHeight;
        int rx = this.terrain.terrainData.heightmapWidth;
   //     int resolution = this.terrain.terrainData.detailResolution;

        int baseMapResolution = this.terrain.terrainData.baseMapResolution;

        TerrainData _genTD = new TerrainData();
        _genTD.heightmapResolution = this.terrain.terrainData.heightmapResolution;
        _genTD.SetDetailResolution(rx, cx);
        
        _genTD.size = new Vector3(td.size.x, td.size.y, td.size.z);
        
        GameObject tGO = Terrain.CreateTerrainGameObject(_genTD);


        this.pTerrain = tGO.GetComponent<Terrain>();
        this.pTerrain.transform.position += new Vector3(td.size.x, 0, 0);

        // Read terrain from file 
      
        int h = this.terrain.terrainData.heightmapHeight;
        int w = this.terrain.terrainData.heightmapWidth;
        float[,] data = new float[h, w];

        data[0, 0] = 0.05f;
        data[0, 1] = 0.02f;
        data[0, 2] = 0.02f;
        data[0, 4] = 0.1f;
        data[1, 0] = 0.021f;

        // read file

        // 1.
        using (System.IO.BinaryReader b = new BinaryReader(
            File.Open("terrainData.raw", FileMode.Open)))
        {
            // 2.
            // Position and length variables.
         
            // 2A.
            // Use BaseStream.
            int length = (int)b.BaseStream.Length;
           
            for (int r = 0; r < h; r++) {
                for (int c = 0; c < w; c++) {

                    data[r, c] = (float)b.ReadUInt16() / 0xFFFF;
                } 
            } 
 
        }

        this.pTerrain.terrainData.SetHeights(0, 0, data);
        this.pTerrain.Flush(); 

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

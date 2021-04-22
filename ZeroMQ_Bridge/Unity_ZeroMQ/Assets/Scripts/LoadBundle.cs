using UnityEngine;
using System.Collections;
using System.IO;

public class LoadBundle : MonoBehaviour
{
    void Start()
    {
        var textureBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "Vehicles/2c5df195-b10b-4b98-883f-48ae92dbb852_vehicle_textures"));
        
        var vehicleBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "Vehicles/2c5df195-b10b-4b98-883f-48ae92dbb852_vehicle_main_linux"));

        if (vehicleBundle == null || textureBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        
        textureBundle.LoadAllAssets();
        var prefab = vehicleBundle.LoadAsset<GameObject>("F1TenthCar");
        Instantiate(prefab);
        

        textureBundle.Unload(false);
        vehicleBundle.Unload(false);

    }
}

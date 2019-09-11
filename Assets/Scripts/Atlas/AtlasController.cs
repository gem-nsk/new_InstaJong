using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasController : MonoBehaviour
{
    public Texture2D[] Sprites;

    public Rect[] rect;
    public Material mat;

    public List<Material> _CreatedMaterials = new List<Material>();

    #region Singleton
    public static AtlasController instance;
    private void Awake()
    {
        instance = this;
        Pack();
        CreateMaterials();
    }
    #endregion;
  
    public void CreateMaterials()
    {
        foreach(Rect _rect in rect)
        {
            Material _mat = new Material(mat);

            _mat.SetTextureScale("_MainTex", new Vector2(_rect.width, _rect.height));
            _mat.SetTextureOffset("_MainTex", new Vector2(_rect.x, _rect.y));


            _CreatedMaterials.Add(_mat);
        }
    }
    //returning offset by image id
    public Material GetMaterialById(int id)
    {
        return _CreatedMaterials[id - 1];
    }
    

    public void Pack()
    {
        Texture2D atlas = new Texture2D(2048, 2048);
        rect = atlas.PackTextures(Sprites, 2, 4096);
        mat.SetTexture("_MainTex", atlas);
    }
}

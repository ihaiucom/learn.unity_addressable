using System;
using UnityEngine.AddressableAssets;
using UnityEngine;

[Serializable]
public class AssetReferenceMaterial : AssetReferenceT<Material>
{
    public AssetReferenceMaterial(string guid) : base(guid)
    {
    }
}
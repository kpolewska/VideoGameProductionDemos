using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class ScreenPositionEffect : MonoBehaviour
{

    public Shader shader;
    private Material _material;

    protected Material imageEffectMaterial
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }
            return _material;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shader != null)
        {
            Graphics.Blit(source, destination, imageEffectMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }

    void OnDisable()
    {
        if (_material)
        {
            DestroyImmediate(_material);
        }
    }
}

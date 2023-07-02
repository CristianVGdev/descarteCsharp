using System;
using System.Collections.Generic;
using UnrealEngine.Core;
using UnrealEngine.Engine;
using UnrealEngine.Runtime;

public class RenderVerification : Actor
{
    public Material PixelArtMaterial;
    public Texture2D VerificationTexture;

    private void Start()
    {
        // Crear una textura de verificación de render
        VerificationTexture = new Texture2D(512, 512);
        Color32[] pixels = new Color32[VerificationTexture.Width * VerificationTexture.Height];
        for (int i = 0; i < pixels.Length; i++)
        {
            bool isDarkPixel = (i / 8) % 2 == 0;
            pixels[i] = isDarkPixel ? Color.black : Color.white;
        }
        VerificationTexture.SetPixels32(pixels);
        VerificationTexture.Apply();

        // Crear un material dinámico con el shader de pixel art
        MaterialInstanceDynamic dynamicMaterial = new MaterialInstanceDynamic(PixelArtMaterial);
        dynamicMaterial.SetTextureParameterValue("TextureParam", VerificationTexture);

        // Asignar el material al actor para que se renderice
        MeshComponent.SetMaterial(0, dynamicMaterial);
    }
}

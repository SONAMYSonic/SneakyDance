
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class SpriteSheetSlicer
{
    [MenuItem("Tools/Slice OIIA Sprite Sheet")]
    public static void SliceSheet()
    {
        string path = "Assets/Resources/oiia_spin_sheet.png";
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError("Cannot find texture importer at: " + path);
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 360;
        importer.filterMode = FilterMode.Bilinear;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        int cols = 6;
        int rows = 4;
        int frameW = 360;
        int frameH = 360;
        int totalFrames = 24;

        List<SpriteMetaData> spriteData = new List<SpriteMetaData>();
        for (int i = 0; i < totalFrames; i++)
        {
            int col = i % cols;
            int row = i / cols;

            SpriteMetaData smd = new SpriteMetaData();
            smd.name = "oiia_spin_" + i.ToString("D2");
            smd.rect = new Rect(col * frameW, (rows - 1 - row) * frameH, frameW, frameH);
            smd.alignment = (int)SpriteAlignment.Center;
            smd.pivot = new Vector2(0.5f, 0.5f);
            spriteData.Add(smd);
        }

        importer.spritesheet = spriteData.ToArray();
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        Debug.Log($"Sliced {totalFrames} sprites from {path}");
    }

    [MenuItem("Tools/Fix OIIA Sprite Size")]
    public static void FixSpriteSize()
    {
        float ppu = 100f;

        // Fix spin sheet (keep existing slice data)
        string spinPath = "Assets/Resources/oiia_spin_sheet.png";
        TextureImporter spinImporter = AssetImporter.GetAtPath(spinPath) as TextureImporter;
        if (spinImporter != null)
        {
            spinImporter.spritePixelsPerUnit = ppu;
            EditorUtility.SetDirty(spinImporter);
            spinImporter.SaveAndReimport();
            Debug.Log($"Spin sheet PPU set to {ppu}");
        }

        // Fix idle sprite
        string idlePath = "Assets/Resources/oiia_idle.png";
        TextureImporter idleImporter = AssetImporter.GetAtPath(idlePath) as TextureImporter;
        if (idleImporter != null)
        {
            idleImporter.textureType = TextureImporterType.Sprite;
            idleImporter.spriteImportMode = SpriteImportMode.Single;
            idleImporter.spritePixelsPerUnit = ppu;
            idleImporter.filterMode = FilterMode.Bilinear;
            idleImporter.textureCompression = TextureImporterCompression.Uncompressed;
            EditorUtility.SetDirty(idleImporter);
            idleImporter.SaveAndReimport();
            Debug.Log($"Idle sprite PPU set to {ppu}");
        }
    }
}
#endif

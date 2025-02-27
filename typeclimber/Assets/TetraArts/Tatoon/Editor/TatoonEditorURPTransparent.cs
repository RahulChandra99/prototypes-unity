﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TatoonEditorURPTransparent : ShaderGUI
{


    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {


        Diffuse(materialEditor, properties);
        NormalMap(materialEditor, properties);
        Shadow(materialEditor, properties);
        Specular(materialEditor, properties);
        Rim(materialEditor, properties);
        Gradient(materialEditor, properties);
        //Outline(materialEditor, properties);

    }
    void Diffuse(MaterialEditor materialEditor, MaterialProperty[] properties)
    {

        MaterialProperty TextureDiffuse = ShaderGUI.FindProperty("_TextureDiffuse", properties);
        MaterialProperty DiffuseColor = ShaderGUI.FindProperty("_DiffuseColor", properties);
        MaterialProperty Opacity = ShaderGUI.FindProperty("_Opacity", properties);
       // MaterialProperty AlphaClipThreshold = ShaderGUI.FindProperty("_AlphaClipThreshold", properties);

        GUILayout.Label("MAIN COLOR AND TEXTURE", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(DiffuseColor, DiffuseColor.displayName);
        materialEditor.ShaderProperty(TextureDiffuse, TextureDiffuse.displayName);
        
      
        materialEditor.ShaderProperty(Opacity, Opacity.displayName);
        //materialEditor.ShaderProperty(AlphaClipThreshold, AlphaClipThreshold.displayName);
        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);
    }
    void NormalMap(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty UseNormalMap = ShaderGUI.FindProperty("_UseNormalMap", properties);
        MaterialProperty Normal = ShaderGUI.FindProperty("_Normal", properties);
        MaterialProperty NormalStrength = ShaderGUI.FindProperty("_NormalStrength", properties);

        GUILayout.Label("NORMAL MAP", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(UseNormalMap, UseNormalMap.displayName);

        if (UseNormalMap.floatValue == 1)
        {
            
            materialEditor.ShaderProperty(Normal, Normal.displayName);

            materialEditor.ShaderProperty(NormalStrength, NormalStrength.displayName);

        }
        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);
    }

    void Shadow(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty UseShadow = ShaderGUI.FindProperty("_UseShadow", properties);
        MaterialProperty ShadowColor = ShaderGUI.FindProperty("_ShadowColor", properties);
        MaterialProperty ShadowTexture = ShaderGUI.FindProperty("_ShadowTexture", properties);
        MaterialProperty ShadowTextureViewProjection = ShaderGUI.FindProperty("_ShadowTextureViewProjection", properties);
        MaterialProperty ShadowTextureTiling = ShaderGUI.FindProperty("_ShadowTextureTiling", properties);
        MaterialProperty ShadowTextureRotation = ShaderGUI.FindProperty("_ShadowTextureRotation", properties);
        MaterialProperty ShadowSize = ShaderGUI.FindProperty("_ShadowSize", properties);
        MaterialProperty ShadowBlend = ShaderGUI.FindProperty("_ShadowBlend", properties);
        MaterialProperty AttenuationPower = ShaderGUI.FindProperty("_AttenuationPower", properties);

        GUILayout.Label("SHADOWS", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(UseShadow, UseShadow.displayName);

        if (UseShadow.floatValue == 1)
        {

           
            materialEditor.ShaderProperty(ShadowTexture, ShadowTexture.displayName);
            materialEditor.ShaderProperty(ShadowTextureViewProjection, ShadowTextureViewProjection.displayName);
            materialEditor.ShaderProperty(ShadowTextureTiling, ShadowTextureTiling.displayName);
            materialEditor.ShaderProperty(ShadowTextureRotation, ShadowTextureRotation.displayName);
            materialEditor.ShaderProperty(ShadowColor, ShadowColor.displayName);

            materialEditor.ShaderProperty(ShadowSize, ShadowSize.displayName);
            materialEditor.ShaderProperty(ShadowBlend, ShadowBlend.displayName);
            materialEditor.ShaderProperty(AttenuationPower, AttenuationPower.displayName);
        }
        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);
    }

    void Specular(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty UseSpecular = FindProperty("_UseSpecular", properties);
        MaterialProperty SpecularMap = ShaderGUI.FindProperty("_SpecularMap", properties);
        MaterialProperty SpecularTextureViewProjextion = ShaderGUI.FindProperty("_SpecularTextureViewProjection", properties);
        MaterialProperty SpecularTextureTiling = ShaderGUI.FindProperty("_SpecularTextureTiling", properties);
        MaterialProperty SpecularTextureRotation = ShaderGUI.FindProperty("_SpecularTextureRotation", properties);
        MaterialProperty SpecularColor = ShaderGUI.FindProperty("_SpecularColor", properties);
        MaterialProperty SpecularLightColor = ShaderGUI.FindProperty("_SpecLightColor", properties);
        MaterialProperty SpecularLightIntensity = ShaderGUI.FindProperty("_SpecularLightIntensity", properties);
        MaterialProperty SpecularSize = ShaderGUI.FindProperty("_SpecularSize", properties);
        MaterialProperty SpecularBlend = ShaderGUI.FindProperty("_SpecularBlend", properties);

        GUILayout.Label("SPECULAR", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(UseSpecular, UseSpecular.displayName);

        if (UseSpecular.floatValue == 1)
        {
           
            materialEditor.ShaderProperty(SpecularMap, SpecularMap.displayName);
            materialEditor.ShaderProperty(SpecularTextureViewProjextion, SpecularTextureViewProjextion.displayName);
            materialEditor.ShaderProperty(SpecularTextureTiling, SpecularTextureTiling.displayName);
            materialEditor.ShaderProperty(SpecularTextureRotation, SpecularTextureRotation.displayName);
            materialEditor.ShaderProperty(SpecularColor, SpecularColor.displayName);
            materialEditor.ShaderProperty(SpecularLightColor, SpecularLightColor.displayName);
            if (SpecularLightColor.floatValue == 1)
            {
                materialEditor.ShaderProperty(SpecularLightIntensity, SpecularLightIntensity.displayName);
            }
            materialEditor.ShaderProperty(SpecularSize, SpecularSize.displayName);
            materialEditor.ShaderProperty(SpecularBlend, SpecularBlend.displayName);
        }



        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);
    }

    void Rim(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty UseRim = FindProperty("_UseRim", properties);
        MaterialProperty RimColor = FindProperty("_RimColor", properties);
        MaterialProperty RimLightColor = FindProperty("_RimLightColor", properties);
        MaterialProperty RimIntensity = FindProperty("_RimLightIntensity", properties);
        MaterialProperty RimTexture = FindProperty("_RimTexture", properties);
        MaterialProperty RimTextureViewProjection = FindProperty("_RimTextureViewProjection", properties);
        MaterialProperty RimTextureTiling = FindProperty("_RimTextureTiling", properties);
        MaterialProperty RimTextureRotation = FindProperty("_RimTextureRotation", properties);
        MaterialProperty RimPower = FindProperty("_RimSize", properties);
        MaterialProperty RimBlend = FindProperty("_RimBlend", properties);

        GUILayout.Label("RIM HIGHLIGHT", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(UseRim, UseRim.displayName);

        if (UseRim.floatValue == 1)
        {

           
            materialEditor.ShaderProperty(RimTexture, RimTexture.displayName);
            materialEditor.ShaderProperty(RimTextureViewProjection, RimTextureViewProjection.displayName);
            materialEditor.ShaderProperty(RimTextureTiling, RimTextureTiling.displayName);
            materialEditor.ShaderProperty(RimTextureRotation, RimTextureRotation.displayName);
            materialEditor.ShaderProperty(RimColor, RimColor.displayName);
            materialEditor.ShaderProperty(RimLightColor, RimLightColor.displayName);
            if (RimLightColor.floatValue == 1)
            {
                materialEditor.ShaderProperty(RimIntensity, RimIntensity.displayName);
            }
            materialEditor.ShaderProperty(RimPower, RimPower.displayName);
            materialEditor.ShaderProperty(RimBlend, RimBlend.displayName);

        }



        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);
    }

    void Gradient(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty UseGradient = FindProperty("_UseGradient", properties);
        MaterialProperty ColorA = FindProperty("_ColorA", properties);
        MaterialProperty ColorB = FindProperty("_ColorB", properties);
        MaterialProperty GradientSize = FindProperty("_GradientSize", properties);
        MaterialProperty GradientPosition = FindProperty("_GradientPosition", properties);
        MaterialProperty GradientRotation = FindProperty("_GradientRotation", properties);

        GUILayout.Label("GRADIENT", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(UseGradient, UseGradient.displayName);

        if (UseGradient.floatValue == 1)
        {
            materialEditor.ShaderProperty(ColorA, ColorA.displayName);
            materialEditor.ShaderProperty(ColorB, ColorB.displayName);
            materialEditor.ShaderProperty(GradientSize, GradientSize.displayName);
            materialEditor.ShaderProperty(GradientPosition, GradientPosition.displayName);
            materialEditor.ShaderProperty(GradientRotation, GradientRotation.displayName);

        }
        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);
    }

    /*void Outline(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty UseOutline = FindProperty("_UseOutline", properties);
        MaterialProperty UseOutlineFire = FindProperty("_UseOutlineFire", properties);
        MaterialProperty OutlineColor = FindProperty("_OutlineColor", properties);
        MaterialProperty OutlineSize = FindProperty("_OutlineSize", properties);
        MaterialProperty Texture = FindProperty("_Texture", properties);
        MaterialProperty Speed = FindProperty("_Speed", properties);
        MaterialProperty NoiseTextureScale = FindProperty("_NoiseTextureScale", properties);
        MaterialProperty OutlineColor1 = FindProperty("_OutlineColor1", properties);
        MaterialProperty OutlineColor2 = FindProperty("_OutlineColor2", properties);
        MaterialProperty PowerColor1 = FindProperty("_PowerColor1", properties);
        MaterialProperty PowerColor2 = FindProperty("_PowerColor2", properties);

        GUILayout.Label("OUTLINE", EditorStyles.boldLabel);

        materialEditor.ShaderProperty(UseOutline, UseOutline.displayName);
        if (UseOutline.floatValue == 1 && UseOutlineFire.floatValue == 0)
        {
            materialEditor.ShaderProperty(UseOutlineFire, UseOutlineFire.displayName);
            materialEditor.ShaderProperty(OutlineColor, OutlineColor.displayName);
            materialEditor.ShaderProperty(OutlineSize, OutlineSize.displayName);
        }
        if (UseOutline.floatValue == 1 && UseOutlineFire.floatValue == 1)
        {
            materialEditor.ShaderProperty(UseOutlineFire, UseOutlineFire.displayName);
            materialEditor.ShaderProperty(OutlineSize, OutlineSize.displayName);
            EditorGUILayout.BeginHorizontal();
            {

                GUILayout.Label("Noise Texture", FxStyles.labelStyle);
                materialEditor.TexturePropertySingleLine(FxStyles.textureLabel, Texture);
            }
            EditorGUILayout.EndHorizontal();
            materialEditor.ShaderProperty(Speed, Speed.displayName);
            materialEditor.ShaderProperty(NoiseTextureScale, NoiseTextureScale.displayName);
            materialEditor.ShaderProperty(OutlineColor1, OutlineColor1.displayName);
            materialEditor.ShaderProperty(OutlineColor2, OutlineColor2.displayName);
            materialEditor.ShaderProperty(PowerColor1, PowerColor1.displayName);
            materialEditor.ShaderProperty(PowerColor2, PowerColor2.displayName);
        }
        GUILayout.Label("_____________________________________________________________", EditorStyles.boldLabel);*/
}



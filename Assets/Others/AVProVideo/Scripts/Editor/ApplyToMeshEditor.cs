﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//-----------------------------------------------------------------------------
// Copyright 2015-2017 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ApplyToMesh))]
    public class ApplyToMeshEditor : UnityEditor.Editor
    {
        private SerializedProperty _propTextureOffset;
        private SerializedProperty _propTextureScale;
        private SerializedProperty _propMediaPlayer;
        private SerializedProperty _propRenderer;
        private SerializedProperty _propTexturePropertyName;
        private SerializedProperty _propDefaultTexture;
        private string[] _materialTextureProperties = new string[0];

        void OnEnable()
        {
            _propTextureOffset = serializedObject.FindProperty("_offset");
            _propTextureScale = serializedObject.FindProperty("_scale");
            _propMediaPlayer = serializedObject.FindProperty("_media");
            _propRenderer = serializedObject.FindProperty("_mesh");
            _propTexturePropertyName = serializedObject.FindProperty("_texturePropertyName");
            _propDefaultTexture = serializedObject.FindProperty("_defaultTexture");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_propRenderer == null)
            {
                return;
            }

            EditorGUILayout.PropertyField(_propMediaPlayer);
            EditorGUILayout.PropertyField(_propDefaultTexture);
            EditorGUILayout.PropertyField(_propRenderer);

            bool hasKeywords = false;
            int texturePropertyIndex = 0;
            if (_propRenderer.objectReferenceValue != null)
            {
                Renderer r = (Renderer)(_propRenderer.objectReferenceValue);

                Material[] materials = r.sharedMaterials;

                MaterialProperty[] matProps = MaterialEditor.GetMaterialProperties(materials);

                foreach (Material mat in materials)
                {
                    if (mat.shaderKeywords.Length > 0)
                    {
                        hasKeywords = true;
                        break;
                    }
                }

                List<string> items = new List<string>(8);
                foreach (MaterialProperty matProp in matProps)
                {
                    if (matProp.type == MaterialProperty.PropType.Texture)
                    {
                        if (matProp.name == _propTexturePropertyName.stringValue)
                        {
                            texturePropertyIndex = items.Count;
                        }
                        items.Add(matProp.name);
                    }
                }
                _materialTextureProperties = items.ToArray();
            }

            int newTexturePropertyIndex = EditorGUILayout.Popup("Texture Property", texturePropertyIndex, _materialTextureProperties);
            if (newTexturePropertyIndex != texturePropertyIndex)
            {
                _propTexturePropertyName.stringValue = _materialTextureProperties[newTexturePropertyIndex];
            }

            if (hasKeywords && _propTexturePropertyName.stringValue != "_MainTex")
            {
                EditorGUILayout.HelpBox("When using an uber shader you may need to enable the keywords on a material for certain texture slots to take effect.  You can sometimes achieve this (eg with Standard shader) by putting a dummy texture into the texture slot.", MessageType.Info);
            }

            EditorGUILayout.PropertyField(_propTextureOffset);
            EditorGUILayout.PropertyField(_propTextureScale);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
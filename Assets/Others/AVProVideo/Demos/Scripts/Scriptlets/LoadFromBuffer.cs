﻿using System.IO;
using UnityEngine;

//-----------------------------------------------------------------------------
// Copyright 2015-2017 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo.Demos
{
    /// <summary>
    /// Demonstration of how to load from a video from a byte array.
    /// It should be noted that only Windows using DirectShow API currently supports this feature.
    /// </summary> 
    public class LoadFromBuffer : MonoBehaviour
    {
        [SerializeField]
        private MediaPlayer _mp = null;

        [SerializeField]
        private string _filename = string.Empty;

        void Start()
        {
            if (_mp != null)
            {
                byte[] buffer = File.ReadAllBytes(_filename);

                if (buffer != null)
                {
                    _mp.OpenVideoFromBuffer(buffer);
                }
            }

            System.GC.Collect();
        }
    }
}


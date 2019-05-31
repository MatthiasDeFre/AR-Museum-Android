//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// A model for the lower left corner of the frame to place when an image is detected.
        /// </summary>
        public GameObject FrameLowerLeft;

        /// <summary>
        /// A model for the lower right corner of the frame to place when an image is detected.
        /// </summary>
        public GameObject FrameLowerRight;

        /// <summary>
        /// A model for the upper left corner of the frame to place when an image is detected.
        /// </summary>
        public GameObject FrameUpperLeft;

        /// <summary>
        /// A model for the upper right corner of the frame to place when an image is detected.
        /// </summary>
        public GameObject FrameUpperRight;
        public GameObject Cube;

        void Start()
        {
            Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var test = Resources.Load<GameObject>("Effect1");
           
            Cube.transform.parent = transform.parent;
            test.transform.parent = Cube.transform;
            test.transform.localPosition = Vector3.zero;
            test.transform.localRotation = new Quaternion();
            var psUpdater = test.GetComponent<PSMeshRendererUpdater>();

            try
            {
                psUpdater.UpdateMeshEffect(Cube);
            } catch(Exception e)
            {
                Debug.Log("ERROR " + e.StackTrace);
                Debug.Log("ERROR "+ e.Message);
            }
          
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                FrameLowerLeft.SetActive(false);
                FrameLowerRight.SetActive(false);
                FrameUpperLeft.SetActive(false);
                FrameUpperRight.SetActive(false);
                return;
            }
            
            float halfWidth = Image.ExtentX / 2;
            float halfHeight = Image.ExtentZ / 2;
            int x = 1000;
            int y = 1000;
            int locX = 500;
            int lockY = 500;
            float xTransitionFactor = x  / Image.ExtentX;
            float yTransitionFactor = y / Image.ExtentZ;
            float transitionedLockX = locX / xTransitionFactor;
            float transitionedLockY = lockY / yTransitionFactor;
            Vector3 movement = new Vector3(transitionedLockX,0, transitionedLockX);

            Cube.transform.localScale = ((halfWidth * Vector3.right) + (halfHeight * Vector3.forward)) - ((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement);
            Cube.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement + Cube.transform.localScale / 2;
            Cube.transform.localRotation = FrameLowerLeft.transform.localRotation;
            var distance = Vector3.Distance((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement, (halfWidth * Vector3.right) +(halfHeight * Vector3.forward));
            float newSize = distance / 50;
            Cube.transform.localScale = ((halfWidth * Vector3.right) + (halfHeight * Vector3.forward)) - ((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement);


            Debug.Log("CUBE S " + Cube.transform.localScale);
            Debug.Log("CUBE M " + Cube.transform.localPosition);
            Debug.Log("CUBE R " + Cube.transform.localRotation);
            Debug.Log("FRAME " + FrameLowerLeft.transform.localRotation);
            Debug.Log("MOVEMENT " + movement);
         
            FrameLowerLeft.transform.localPosition =
                (halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement;
            FrameLowerRight.transform.localPosition =
                (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
            FrameUpperLeft.transform.localPosition =
                (halfWidth * Vector3.left) + (halfHeight * Vector3.forward);
            FrameUpperRight.transform.localPosition =
                (halfWidth * Vector3.right) + (halfHeight * Vector3.forward);
            Debug.Log("TEST" + Image.ExtentX);
            Debug.Log("LOWER LEFT" + (halfWidth * Vector3.left) + (halfHeight * Vector3.back) + new Vector3(0.07F,0,0.07F));
            Debug.Log("LOWER RIGHT" + (halfWidth * Vector3.right) + (halfHeight * Vector3.back));
            Debug.Log("UPPER LEFT" + (halfWidth * Vector3.left) + (halfHeight * Vector3.forward));
            Debug.Log("UPPER RIGHT" + (halfWidth * Vector3.right) + (halfHeight * Vector3.forward));

            FrameLowerLeft.SetActive(true);
            FrameLowerRight.SetActive(true);
            FrameUpperLeft.SetActive(true);
            FrameUpperRight.SetActive(true);
        }
    }
}

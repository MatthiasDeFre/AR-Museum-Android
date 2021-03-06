// Copyright (c) 2015 - 2019 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Editor.Utils;
using Doozy.Engine.Utils;
using UnityEditor;

// ReSharper disable MemberCanBePrivate.Global

namespace Doozy.Editor.Internal
{
    [InitializeOnLoad]
    public class FoldersProcessor
    {
        static FoldersProcessor() { ExecuteProcessor(); }

        private static void ExecuteProcessor()
        {
            if (!ProcessorsSettings.Instance.RunFoldersProcessor) return;

            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            Run();
        }

        public static void Run() { DoozyPath.CreateMissingFolders(); }
    }
}
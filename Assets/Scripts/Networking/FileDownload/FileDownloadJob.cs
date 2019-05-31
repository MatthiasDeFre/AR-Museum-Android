using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public struct FileDownloadJob : IJob
{
    private AugmentedImageDatabase m_db;
    private Texture2D m_tex;

    public FileDownloadJob(AugmentedImageDatabase db, Texture2D tex)
    {
        m_db = db;
        m_tex = tex;
    }
    public void Execute()
    {
        m_db.AddImage("test", m_tex);
    }
}

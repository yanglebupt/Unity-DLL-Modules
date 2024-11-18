using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using YLCommon.Unity.Nav;

public class YLCommonEditor
{
  [MenuItem("YLCommon/Export NavMesh")]
  public static void ExportNavMesh()
  {
    NavMeshExporter.ExportAsFile(NavMesh.CalculateTriangulation(), Path.Combine(Application.dataPath, "Resources", "MapDatas", $"{SceneManager.GetActiveScene().name}.json"));
    AssetDatabase.Refresh();
  }
}
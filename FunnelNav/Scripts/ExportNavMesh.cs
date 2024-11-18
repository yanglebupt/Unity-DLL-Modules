using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AI;
using YLCommon.Nav;

namespace YLCommon.Unity.Nav
{
  public class NavMeshExporter
  {
    public static void ExportAsFile(NavConfig config, string filename)
    {
      string outstr = JsonConvert.SerializeObject(config);
      File.WriteAllText(filename, outstr);
    }

    public static void ExportAsFile(NavMeshTriangulation nmtris, string filename)
    {
      ExportAsFile(LoadFromNavMesh(nmtris), filename);
    }

    public static NavConfig LoadFromNavMesh(NavMeshTriangulation nmtris)
    {
      // 保存多边形顶点索引，需要将共边的三角面合并成一个多边形
      List<int[]> indices = new();
      // 已检查的顶点索引
      List<int> checkedIndices = new();
      int[] idxs = nmtris.indices;
      for (int i = 0, n = idxs.Length; i < n; i += 3)
      {
        int ei = -1, count = 0;
        for (int j = i; j < i + 3; j++)
        {
          if (checkedIndices.Contains(idxs[j])) count++;
          else ei = idxs[j];
        }

        // 有共边
        if (count == 2)
          checkedIndices.Add(ei);
        else
        {
          // 否则结束之前的多边形
          if (checkedIndices.Count > 0)
          {
            indices.Add(checkedIndices.ToArray());
            checkedIndices.Clear();
          }
          // 开启新一轮检测多边形
          for (int j = i; j < i + 3; j++)
            checkedIndices.Add(idxs[j]);
        }
      }
      // 否则结束之前的多边形
      if (checkedIndices.Count > 0)
        indices.Add(checkedIndices.ToArray());

      // 去除相同的顶点
      List<NavVector> vertices = new();
      Dictionary<int, int> indice_remap = new();
      Vector3[] vts = nmtris.vertices;
      int remap_idx = 0;
      for (int i = 0, n = vts.Length; i < n; i++)
      {
        Vector3 v = vts[i];
        NavVector pos = NavVector.Round(new NavVector(v.x, v.y, v.z), 2);

        bool exist = false;
        for (int j = 0, m = vertices.Count; j < m; j++)
        {
          if (vertices[j] == pos)
          {
            indice_remap[i] = j;
            exist = true;
            break;
          }
        }

        if (!exist)
        {
          vertices.Add(pos);
          indice_remap[i] = remap_idx;
          remap_idx++;
        }
      }

      // 重新映射索引
      for (int i = 0, n = indices.Count; i < n; i++)
      {
        for (int j = 0, m = indices[i].Length; j < m; j++)
        {
          indices[i][j] = indice_remap[indices[i][j]];
        }
      }

      // 邻边共线问题：两个多边形的邻边共线，但是一长一短，并且短边的点不在长边的顶点里面，那么也要将短边点插入到另一个多边形的这条长边里面
      // 这样两个多边形才有一条一样的边，后面计算时才能作为邻居
      for (int i = 0, n = indices.Count; i < n; i++)
      {
        int[] indice = indices[i];
        while (true)
        {
          int[] replace_indice = null;

          for (int j = 0, m = indice.Length, k = m - 1; j < m; k = j++)
          {
            int startIdx = indice[k], endIdx = indice[j];
            bool need_break = false;
            for (int l = 0, t = vertices.Count; l < t; l++)
            {
              if (l == startIdx || l == endIdx) continue;
              NavVector start = vertices[startIdx], end = vertices[endIdx], pos = vertices[l];
              if (NavVector.IsInLineXZ(pos, start, end))
              {
                // 需要将 l 插入到 k-j 之间
                replace_indice = new int[m + 1];
                int insert = k + 1;
                for (int e = 0; e < m + 1; e++)
                {
                  if (e < insert) replace_indice[e] = indice[e];
                  else if (e == insert) replace_indice[e] = l;
                  else replace_indice[e] = indice[e - 1];
                }
                need_break = true;
                break;
              }
            }
            if (need_break) break;
          }

          if (replace_indice != null) { indices[i] = replace_indice; indice = replace_indice; }
          else break;
        }
      }

      return new NavConfig()
      {
        vertices = vertices,
        indices = indices
      };
    }
  }
}

using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace YLCommon.Nav
{
  /// <summary>
  /// 最简单的暴力寻路，不确保最短路径，全部方向查找，运算量很大
  /// </summary>
  public abstract class BasePathFinder : IPathFinder
  {
    // 八个方向，上下左右距离更短，优先搜索
    public static readonly Vector2[] NEIGHBOR_DIRS = {
      new(-1,1),
      new(0,1),
      new(1,1),
      new(1,0),
      new(1,-1),
      new(0,-1),
      new(-1,-1),
      new(-1,0),
    };
    /// <summary>
    /// 等待检测邻居区块队列
    /// </summary>
    protected PriorityQueue<BlockLogic> waitDetectQueue = new(10);
    /// <summary>
    /// 全部邻居区块检测完成的区块
    /// </summary>
    protected List<BlockLogic> detected = new();

    #region 保存一些信息
    protected BlockMap blockMap;
    protected BlockLogic start;
    protected BlockLogic end;
    #endregion

    public List<BlockLogic> Search(BlockMap blockMap, BlockLogic start, BlockLogic end)
    {
      throw new System.NotImplementedException();
    }

    public async Task<List<BlockLogic>> Search(BlockMap blockMap, BlockLogic start, BlockLogic end, int delay)
    {
      this.blockMap = blockMap;
      this.start = start;
      this.end = end;

      waitDetectQueue.Clear();
      detected.Clear();

      start.distanceFromStart = 0;
      waitDetectQueue.Enqueue(start);

      while (waitDetectQueue.Count > 0)
      {
        // 取出一个待检测区块，检测其周围
        BlockLogic block = waitDetectQueue.Dequeue();

        // 是目标区块结束
        if (block == end) return GetFoundPath(end);

        // 不可行走区域不考虑
        if (!block.Workable) continue;

        // 检测周围
        for (int i = 0; i < NEIGHBOR_DIRS.Length; i++)
        {
          int nx = block.xIndex + (int)NEIGHBOR_DIRS[i].X;
          int ny = block.yIndex + (int)NEIGHBOR_DIRS[i].Y;
          if (blockMap.Inside(nx, ny))
          {
            BlockLogic neighbor = blockMap.blocks[nx, ny];
            if (!neighbor.Workable) continue;
            await Task.Delay(delay);
            DetectNeighbor(block, neighbor);
            // 是目标区块结束
            if (neighbor == end) return GetFoundPath(end);
          }
        }

        if (!detected.Contains(block))
          detected.Add(block);
      }

      return null;
    }

    /// <summary>
    /// 检测当前块的邻居块
    /// </summary>
    /// <param name="current">当前节点</param>
    /// <param name="neighbor">邻居节点</param>
    protected abstract void DetectNeighbor(BlockLogic current, BlockLogic neighbor);

    /// <summary>
    /// 从终点往前推路径到起点
    /// </summary>
    /// <param name="end">终点</param>
    /// <returns></returns>
    private List<BlockLogic> GetFoundPath(BlockLogic end)
    {
      List<BlockLogic> foundpath = new();
      while (end.preBlock != null)
      {
        foundpath.Insert(0, end);
        end = end.preBlock;
      }
      return foundpath;
    }
  }
}

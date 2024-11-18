namespace YLCommon.Nav
{
  /// <summary>
  /// Dijkstra 最短路径寻路，确保最短路径，全部方向查找，运算量很大，最耗时
  /// </summary>
  public class DJPathFinder : BasePathFinder
  {
    protected override void DetectNeighbor(BlockLogic current, BlockLogic neighbor)
    {
      // 需要重复检测该邻居，来更新最短距离
      if (detected.Contains(neighbor)) return;

      float n_dis = current.distanceFromStart + BlockLogic.GetDistance(current, neighbor);
      // 更新最短路径
      if (neighbor.IsInitial() || n_dis < neighbor.distanceFromStart)
      {
        neighbor.preBlock = current;
        neighbor.distanceFromStart = n_dis;
        // 更新优先级，已行走距离最短的优先级最高
        neighbor.priority = n_dis;
      }

      if (!waitDetectQueue.Contains(neighbor))
        waitDetectQueue.Enqueue(neighbor);

      neighbor.OnViewStateChanged?.Invoke(BlockViewState.Checking);
    }
  }
}
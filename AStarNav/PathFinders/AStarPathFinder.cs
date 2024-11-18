namespace YLCommon.Nav
{
  /// <summary>
  /// A* 路径寻路，不确保最短，也不会查找全部方向，最常用
  /// 耗时处于 Dijkstra 最短路径寻路 和 贪心路径寻路 之间
  /// </summary>
  public class AStarPathFinder : BasePathFinder
  {
    protected override void DetectNeighbor(BlockLogic current, BlockLogic neighbor)
    {
      // 需要重复检测该邻居，来更新最短距离
      if (detected.Contains(neighbor)) return;

      float pass_dis = current.distanceFromStart + BlockLogic.GetDistance(current, neighbor);
      float forward_dis = BlockLogic.GetDistance(neighbor, end);

      // 更新最短路径
      if (neighbor.IsInitial() || pass_dis < neighbor.distanceFromStart)
      {
        neighbor.preBlock = current;
        neighbor.distanceFromStart = pass_dis;
        // 更新优先级，距离终点越短 + 已行走距离越短 的优先级越高
        neighbor.priority = pass_dis + forward_dis;
      }

      if (!waitDetectQueue.Contains(neighbor))
        waitDetectQueue.Enqueue(neighbor);

      neighbor.OnViewStateChanged?.Invoke(BlockViewState.Checking);
    }
  }
}
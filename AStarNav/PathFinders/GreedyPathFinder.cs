namespace YLCommon.Nav
{
  /// <summary>
  /// 贪心路径寻路，不确保最短，但是只查找朝向目标的方向，运算量较小，适合障碍物少的开阔寻路，最快不耗时间
  /// 当障碍物较多的地图，这种方式得到的路径是绕了很多弯，不符合最短
  /// </summary>
  public class GreedyPathFinder : BasePathFinder
  {
    protected override void DetectNeighbor(BlockLogic current, BlockLogic neighbor)
    {
      // 不需要重复检测该邻居
      if (waitDetectQueue.Contains(neighbor) || detected.Contains(neighbor)) return;

      neighbor.preBlock = current;
      neighbor.distanceFromStart = current.distanceFromStart + BlockLogic.GetDistance(current, neighbor);
      // 更新优先级，距离终点越短的优先级越高
      neighbor.priority = BlockLogic.GetDistance(neighbor, end);
      waitDetectQueue.Enqueue(neighbor);

      neighbor.OnViewStateChanged?.Invoke(BlockViewState.Checking);
    }
  }
}
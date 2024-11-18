namespace YLCommon.Nav
{
  /// <summary>
  /// 最简单的暴力寻路，不确保最短路径，全部方向查找，运算量很大，最耗时
  /// 实际项目不会使用，这里是作为引子引出后面的内容
  /// </summary>
  public class BrutePathFinder : BasePathFinder
  {
    protected override void DetectNeighbor(BlockLogic current, BlockLogic neighbor)
    {
      // 不需要重复检测该邻居
      if (waitDetectQueue.Contains(neighbor) || detected.Contains(neighbor)) return;

      neighbor.preBlock = current;
      neighbor.distanceFromStart = current.distanceFromStart + BlockLogic.GetDistance(current, neighbor);
      // 更新优先级，其实暴力寻路不需要优先级的
      neighbor.priority = detected.Count;
      waitDetectQueue.Enqueue(neighbor);

      neighbor.OnViewStateChanged?.Invoke(BlockViewState.Checking);
    }
  }
}

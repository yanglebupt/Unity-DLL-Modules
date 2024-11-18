using System.Collections.Generic;
using System.Threading.Tasks;

namespace YLCommon.Nav
{
  public interface IPathFinder
  {
    /// <summary>
    /// 开始路径搜索
    /// </summary>
    /// <param name="start">搜索起点</param>
    /// <param name="end">搜索终点</param>
    /// <returns>搜索路径</returns>
    List<BlockLogic> Search(BlockMap blockMap, BlockLogic start, BlockLogic end);

    /// <summary>
    /// just for test, please use no async Search
    /// </summary>
    Task<List<BlockLogic>> Search(BlockMap blockMap, BlockLogic start, BlockLogic end, int delay);
  }
}


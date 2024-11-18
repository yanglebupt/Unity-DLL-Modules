using System;

namespace YLCommon.Nav
{
  /// <summary>
  /// 整个导航地图
  /// </summary>
  public class BlockMap
  {
    public int xCount;
    public int yCount;
    public BlockLogic[,] blocks;

    /// <summary>
    /// 当创建地图单元时，可用于外部可视化
    /// </summary>
    public Action<BlockLogic> OnCreateBlock;

    /// <summary>
    /// 初始化地图
    /// </summary>
    /// <param name="xCount"> x 方向个数 </param>
    /// <param name="yCount"> y 方向个数 </param>
    public void InitMap(int xCount, int yCount)
    {
      this.xCount = xCount;
      this.yCount = yCount;
      blocks = new BlockLogic[xCount, yCount];

      for (int i = 0; i < xCount; i++)
      {
        for (int j = 0; j < yCount; j++)
        {
          BlockLogic block = new BlockLogic(i, j);
          blocks[i, j] = block;
          OnCreateBlock?.Invoke(block);
        }
      }
    }

    /// <summary>
    /// 判断某个区块是否在地图里面
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool Inside(int x, int y)
    {
      return x >= 0 && x < xCount && y >= 0 && y < yCount;
    }
  }
}


using System;

namespace YLCommon.Nav
{
  public enum BlockViewState
  {
    /// <summary>
    /// 可行走
    /// </summary>
    Walk,
    /// <summary>
    /// 不可行走
    /// </summary>
    Block,
    /// <summary>
    /// 起始点
    /// </summary>
    StartPoint,
    /// <summary>
    /// 终点
    /// </summary>
    EndPoint,
    /// <summary>
    /// 正在检测
    /// </summary>
    Checking,
    /// <summary>
    /// 检测完成
    /// </summary>
    PathResult,
  }

  public class BlockLogic : IComparable<BlockLogic>
  {
    /// <summary>
    /// 区块 x 方向的编号
    /// </summary>
    public int xIndex;
    /// <summary>
    /// 区块 y 方向的编号
    /// </summary>
    public int yIndex;

    /// <summary>
    /// 区块是否可行走，默认为 true
    /// </summary>
    private bool _workable;

    public bool Workable
    {
      get => _workable;
      set
      {
        _workable = value;
        OnViewStateChanged?.Invoke(_workable ? BlockViewState.Walk : BlockViewState.Block);
      }
    }

    /// <summary>
    /// 从起点到当前块的距离
    /// </summary>
    public float distanceFromStart = float.PositiveInfinity;

    /// <summary>
    /// 当前区块的前一个查找区块
    /// </summary>
    public BlockLogic preBlock = null;

    /// <summary>
    /// 区块优先级，值越低，优先级越高
    /// </summary>
    public float priority;

    /// <summary>
    /// 区块状态改变
    /// </summary>
    public Action<BlockViewState> OnViewStateChanged;

    public BlockLogic(int xIndex, int yIndex)
    {
      this.xIndex = xIndex;
      this.yIndex = yIndex;
      _workable = true;
      OnViewStateChanged?.Invoke(BlockViewState.Walk);
    }


    /// <summary>
    /// 计算区块之间的距离
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float GetDistance(BlockLogic a, BlockLogic b)
    {
      float x = MathF.Abs(a.xIndex - b.xIndex);
      float y = MathF.Abs(a.yIndex - b.yIndex);
      float min = MathF.Min(x, y);
      float max = MathF.Max(x, y);
      return 1.4f * min + (max - min);
    }

    public override string ToString()
    {
      return $"({xIndex},{yIndex})";
    }

    public bool IsInitial()
    {
      return preBlock == null;
    }

    public int CompareTo(BlockLogic other)
    {
      if (priority < other.priority) return -1;
      else if (priority > other.priority) return 1;
      else return 0;
    }
  }
}

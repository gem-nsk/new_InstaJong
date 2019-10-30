public enum GameStrategy
{
    Normal = 1,
    Bottom,
    Top,
    Left,
    Right,
    LeftAndRight,
    TopAndBottom,
    XCenter,
    YCenter,
}


public class StrategyFactory
{


    public static IGameStrategy CreateInstance(GameStrategy strategy)
    {
        IGameStrategy gameStrategy = null;
        switch (strategy)
        {
            case GameStrategy.Normal:
                gameStrategy = new NormalStrategy();
                break;
            case GameStrategy.Bottom:
                gameStrategy = new BottomStrategy();
                break;
            case GameStrategy.Top:
                gameStrategy = new TopStrategy();
                break;
            case GameStrategy.Left:
                gameStrategy = new LeftStrategy();
                break;
            case GameStrategy.Right:
                gameStrategy = new RightStrategy();
                break;
            case GameStrategy.LeftAndRight:
                gameStrategy = new LeftAndRightStrategy();
                break;
            case GameStrategy.TopAndBottom:
                gameStrategy = new TopAndBottomStrategy();
                break;
                //case GameStrategy.XCenter:
                //    gameStrategy = new XCenterStrategy();
                //    break;
                //case GameStrategy.YCenter:
                //    gameStrategy = new YCenterStrategy();
                //    break;

        }
        return gameStrategy;
    }
}

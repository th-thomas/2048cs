namespace Game2048.Library;

internal struct Point : IEquatable<Point>
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    #region Equality
    public static bool operator ==(Point left, Point right) => left.Equals(right);

    public static bool operator !=(Point left, Point right) => !(left == right);


    public bool Equals(Point other) => (X == other.X) && (Y == other.Y);

    public override bool Equals(object obj) => obj is Point point && Equals(point);

    public override int GetHashCode() => (X, Y).GetHashCode();
    #endregion
}

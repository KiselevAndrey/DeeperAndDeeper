using UnityEngine;

public class Swiper
{
    public enum DirectionName { Vertical, Horizontal }//, Up, Down, Left, Right }

    public Vector2 startPressPos;
    public Vector2 endPressPos;

    public float detectSwipe = 0.3f;

    Vector2 swipeVector;

    public float Swipe(DirectionName direction)
    {
        swipeVector = (endPressPos - startPressPos);//.normalized;

        return direction switch
        {
            DirectionName.Vertical => swipeVector.y,
            DirectionName.Horizontal => swipeVector.x,
            _ => 0f,
        };
    }
}

using UnityEngine;

public class Swiper : MonoBehaviour
{
    public Vector2 startPressPos;
    public Vector2 endPressPos;

    public float detectSwipe = 0.3f;
    public enum DirectionName { Vertical, Horizontal }//, Up, Down, Left, Right }

    Vector2 swipeVector;
    
    public void Swipe()
    {
        swipeVector = endPressPos - startPressPos;
        swipeVector.Normalize();
        print(swipeVector);

        if (swipeVector.y > 0 && swipeVector.x > -detectSwipe && swipeVector.x < detectSwipe)
        {
            Debug.Log("up swipe");
        }
        if (swipeVector.y < 0 && swipeVector.x > -detectSwipe && swipeVector.x < detectSwipe)
        {
            Debug.Log("down swipe");
        }
        if (swipeVector.x < 0 && swipeVector.y > -detectSwipe && swipeVector.y < detectSwipe)
        {
            Debug.Log("left swipe");
        }
        if (swipeVector.x > 0 && swipeVector.y > -detectSwipe && swipeVector.y < detectSwipe)
        {
            Debug.Log("right swipe");
        }
    }

    public float Swipe(DirectionName direction)
    {
        swipeVector = (endPressPos - startPressPos).normalized;

        return direction switch
        {
            DirectionName.Vertical => swipeVector.y,
            DirectionName.Horizontal => swipeVector.x,
            _ => 0f,
        };
    }
}

using UnityEngine;

public class ColumnRotate : MonoBehaviour
{
    public float rotationSpeed;

    CurrentPlatform currentPlatform;
    Swiper swiper;

    enum CurrentPlatform { PC, Android }

    private void Start()
    {
        currentPlatform = CurrentPlatform.PC;
        swiper = new Swiper();
    }

    private void Update()
    {
        Rotate();
        if (Input.GetKeyUp(KeyCode.Space)) Time.timeScale *= 1.1f;
    }

    void Rotate()
    {
        switch (currentPlatform)
        {
            case CurrentPlatform.PC:
                if (Input.GetMouseButtonDown(0)) swiper.startPressPos = Input.mousePosition;
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = Input.mousePosition;
                    swiper.endPressPos = mousePos;

                    float swipeRotate = swiper.Swipe(Swiper.DirectionName.Horizontal);

                    if (swipeRotate == 0) break;

                    Vector3 rotate = Vector3.zero;
                    rotate.y = transform.rotation.eulerAngles.y + rotationSpeed * -swipeRotate;

                    transform.rotation = Quaternion.Euler(rotate);
                }
                break;
            case CurrentPlatform.Android:
                break;
            default:
                break;
        }
    }
}

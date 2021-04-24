using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnRotate : MonoBehaviour
{
    enum CurrentPlatform { PC, Android }

    public float rotationSpeed;

    CurrentPlatform _currentPlatform;
    Swiper _swiper;

    void Start()
    {
        _currentPlatform = CurrentPlatform.PC;
        _swiper = new Swiper();

        if (_currentPlatform == CurrentPlatform.PC)
        {
            Camera camera = Camera.main;
            _swiper.startPressPos = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    #region Rotate
    void Rotate()
    {
        if (Time.timeScale == 0f) return;

        switch (_currentPlatform)
        {
            case CurrentPlatform.PC:
                Vector2 mousePos = Input.mousePosition;
                _swiper.endPressPos = mousePos;

                float swipeRotate = _swiper.Swipe(Swiper.DirectionName.Horizontal);

                if (swipeRotate == 0) break;

                Vector3 rotate = Vector3.zero;
                rotate.y = transform.rotation.eulerAngles.y + rotationSpeed * -swipeRotate;

                transform.rotation = Quaternion.Euler(rotate);
                break;
            case CurrentPlatform.Android:
                break;
            default:
                break;
        }
    }
    #endregion
}

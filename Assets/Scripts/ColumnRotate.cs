using UnityEngine;

public class ColumnRotate : MonoBehaviour
{
    private enum CurrentPlatform { PC, Android }

    public static ColumnRotate singleton;

    public float rotationSpeed;
    [SerializeField] RectTransform arrow;

    [HideInInspector] public int speedBonusPurchased;

    private CurrentPlatform _currentPlatform;
    private Swiper _swiper;
    private bool _notPlaying;
    private float _maxSwiperLength;
    

    #region Awake Start Update
    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        _currentPlatform = CurrentPlatform.PC;
        _swiper = new Swiper();

        if (_currentPlatform == CurrentPlatform.PC)
        {
            Camera camera = Camera.main;
            _swiper.startPressPos = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);
        }

        _maxSwiperLength = Camera.main.pixelWidth / 2;
    }

    void Update()
    {
        Rotate();
    }
    #endregion

    #region Rotate
    void Rotate()
    {
        if (Time.timeScale == 0f || _notPlaying) return;

        switch (_currentPlatform)
        {
            case CurrentPlatform.PC:
                Vector2 mousePos = Input.mousePosition;
                _swiper.endPressPos = mousePos;

                float swipeRotate = _swiper.Swipe(Swiper.DirectionName.Horizontal);
                swipeRotate = Mathf.Min(Mathf.Abs(swipeRotate), _maxSwiperLength) * (swipeRotate < 0 ? -1 : 1);

                Vector3 arrowScale = Vector3.one;
                arrowScale.x = swipeRotate / 100;

                arrow.localScale = arrowScale;

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

    #region Save Load
    public void Save() => SaveSystem.SaveColumnRotate(this);

    public void Load(bool notPlaying = false)
    {
        SaveSystem.LoadColumnRotate()?.LoadData(ref singleton);
        
        _notPlaying = notPlaying;
    }
    #endregion

    #region Shoping
    public void BuyRotationSpeed()
    {
        rotationSpeed += .001f;
        speedBonusPurchased++;
    }
    #endregion
}

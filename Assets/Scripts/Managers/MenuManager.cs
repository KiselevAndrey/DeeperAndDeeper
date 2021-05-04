using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("How Play")]
    [SerializeField] GameObject howPlay;
    [SerializeField] Text howPlayText;

    PlayerManager _player;
    int _howPlayIndex;

    void Start()
    {
        _player = gameObject.AddComponent<PlayerManager>();
        _player.Load(true);
    }

    public void ResetPlayer(bool delRecord)
    {
        _player.ResetMe(delRecord);
    }

    #region HowPlay
    public void HowPlayNext()
    {
        _howPlayIndex++;
        HowPlay(_howPlayIndex);
    }

    public void HowPlay(int index)
    {
        _howPlayIndex = index;
        string[] texts = {
        "Target - go deeper",
        "White platforms do not cause damage",
        "Green platforms take 1 damage",
        "Red platforms take more than 1 damage depending on the level",
        "Platform is navigated with the mouse, moving it from the middle of the screen",
        "Thanks for music!\nSpook4 от PeriTune | http://peritune.com Attribution 4.0 International (CC BY 4.0) https://creativecommons.org/licenses/by/4.0 Music promoted by https://www.chosic.com/ "
        };

        print(index);
        if (index == texts.Length)
        {
            howPlay.SetActive(false);
            return;
        }
        else if (index == 0) howPlay.SetActive(true);
        
        howPlayText.text = texts[index];
    }
    #endregion
}

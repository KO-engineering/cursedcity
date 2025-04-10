using TMPro;
using UnityEngine;

public class KillCount : Singleton<KillCount>
{
    public TMP_Text killText;
    public int killCount;

    void Start()
    {
        killCount = PlayerPrefs.GetInt("KillCount", 0);
        UpdateKillText();
    }

    void Update()
    {
        killText.text = killCount.ToString();
        PlayerPrefs.SetInt("KillCount", killCount);
        PlayerPrefs.Save();
    }
    void UpdateKillText()
    {
        if (killText != null)
            killText.text = killCount.ToString();
    }
    public void ResetKillCount()
    {
        killCount = 0;
        PlayerPrefs.SetInt("KillCount", 0);
        PlayerPrefs.Save();
        UpdateKillText();
    }

}

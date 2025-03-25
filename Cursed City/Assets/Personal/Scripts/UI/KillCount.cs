using TMPro;

public class KillCount : Singleton<KillCount>
{
    public TMP_Text killText;
    public int killCount;

    void Start()
    {
        killCount = 0;
    }

    void Update()
    {
        killText.text = killCount.ToString();
    }

}

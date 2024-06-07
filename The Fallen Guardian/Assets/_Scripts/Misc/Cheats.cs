using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet PlayerReference;
    Player player;

    [SerializeField] PlayerStats stats;

    [SerializeField] TextMeshProUGUI timeText;

    public void OnPlayerJoin()
    {
        Invoke("GetPlayer", .5f);
    }

    void GetPlayer()
    {
        if (PlayerReference.items.Count > 0)
        {
            player = PlayerReference.GetItemIndex(0).GetComponent<Player>();
        }
    }

    public void OnGainEXPPressed()
    {
        player.GetComponent<LevelSystem>().GainExperienceFlatRate(10);
    }

    public void OnHealCheatPressed()
    {
        player.Heal(1, false);
    }

    public void OnDamageChatPressed()
    {
        player.TakeDamage(1);
    }

    public void OnTimeSlowPressed()
    {
        // Decrease the timescale by 0.10
        Time.timeScale -= 0.10f;

        // Ensure timescale does not go below a certain threshold, for example, 0.1
        if (Time.timeScale < 0.1f)
        {
            Time.timeScale = 0.1f;
        }

        timeText.text = "Time " + Time.timeScale.ToString("F2");
    }

    public void OnTimeSpeedPressed()
    {
        // Increase the timescale by 0.10
        Time.timeScale += 0.10f;

        // Ensure timescale does not go above a certain threshold
        if (Time.timeScale > 5f)
        {
            Time.timeScale = 5f;
        }

        timeText.text = "Time " + Time.timeScale.ToString("F2");
    }

    public void OnReloadScenePressed()
    {
        SceneManager.LoadScene(0);
    }
}

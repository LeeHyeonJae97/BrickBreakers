using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : AMode
{
    public NetworkManager networkManager;
    public AudioManager audioManager;
    public BallManager ballManager;
    public BrickManager brickManager;
    public Timer shootTimer;

    public GameObject dragCanvas;
    public Text levelText;
    public Text scoreText;
    public Text quantityUpText;
    public Text speedUpText;
    public GameObject gameOverCanvas;
    public Text winOrLoseText;
    public GameObject giveUpButton;
    public GameObject pauseButton;
    public GameObject exitGameButton;

    private int level;
    public int Level
    {
        get { return level; }

        set
        {
            level = value;
            levelText.text = level.ToString();
        }
    }
    private int score;
    public int Score
    {
        get { return score; }

        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }
    private int quantityUp;
    public int QuantityUp
    {
        get { return quantityUp; }

        set
        {
            quantityUp = value;
            quantityUpText.text = quantityUp.ToString();
        }
    }
    private int speedUp;
    public int SpeedUp
    {
        get { return speedUp; }

        set
        {
            speedUp = value;
            speedUpText.text = speedUp.ToString();
        }
    }

    public override void SetActive(bool value)
    {
        if (holder.activeInHierarchy == value) return;

        holder.SetActive(value);

        if (value)
        {
            status.text = "";
            if(GameManager.isSinglePlay)
            {
                giveUpButton.SetActive(false);
                pauseButton.SetActive(true);
                exitGameButton.SetActive(true);
            }
            else
            {
                giveUpButton.SetActive(true);
                pauseButton.SetActive(false);
                exitGameButton.SetActive(false);
            }

            audioManager.PlayBgm("Game");
        }
        else
        {
            ;
        }
    }

    public override void SetMode(ModeManager.SmallMode mode)
    {
        switch (mode)
        {
            case ModeManager.SmallMode.READYFORGAME:
                Level = 1;
                Score = 0;
                QuantityUp = 0;
                SpeedUp = 0;
                ballManager.Init();
                brickManager.NextLevel(level);
                Invoke(nameof(SetReadyForShootMode), 0.5f);
                gameOverCanvas.SetActive(false);
                break;
            case ModeManager.SmallMode.READYFORSHOOT:
                dragCanvas.SetActive(true);
                shootTimer.StartTimer(10);
                break;
            case ModeManager.SmallMode.SHOOT:
                dragCanvas.SetActive(false);
                shootTimer.StopTimer();
                break;
            case ModeManager.SmallMode.NEXTLEVEL:
                int qc = brickManager.quantityUpCount;
                int sc = brickManager.speedUpCount;
                int dc = brickManager.DestroyCount;
                Level += 1;
                Score += dc * dc;
                QuantityUp += qc;
                SpeedUp += sc;
                if (!GameManager.isSinglePlay) networkManager.SendAttackedEvent(dc);
                brickManager.NextLevel(level);
                ballManager.NextLevel(qc, sc);
                Invoke(nameof(SetReadyForShootMode), 0.5f);
                break;
            case ModeManager.SmallMode.WIN:
                shootTimer.StopTimer();
                ballManager.GameOver();
                brickManager.GameOver();
                if (!GameManager.isSinglePlay)
                {
                    gameOverCanvas.SetActive(true);
                    winOrLoseText.text = "WIN";
                }
                break;
            case ModeManager.SmallMode.LOSE:
                shootTimer.StopTimer();
                ballManager.GameOver();
                brickManager.GameOver();
                gameOverCanvas.SetActive(true);
                winOrLoseText.text = "LOSE";
                break;
        }
    }

    private void SetReadyForShootMode()
    {
        SetMode(ModeManager.SmallMode.READYFORSHOOT);
    }
}

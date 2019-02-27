using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    /// <summary>
    /// Frame of the clock.
    /// </summary>
    GameObject clockFrame;
    /// <summary>
    /// Clock hand of the clock.
    /// </summary>
    GameObject clockHand;
    /// <summary>
    /// Coroutine for timer.
    /// </summary>
    public static Coroutine timer;
    /// <summary>
    /// The initial time of tetrimino waiting time.
    /// </summary>
    public float initialTimeToFallTetrimino;
    /// <summary>
    /// Time tetrimino would wait until it falls.
    /// </summary>
    public float timeToFallTetrimino;
    /// <summary>
    /// Time tetris waits to fall.
    /// </summary>
    public float tetriminoWaitedTime;
    /// <summary>
    /// Time tetris has created.
    /// </summary>
    public float tetriminoCreatedTime;
    /// <summary>
    /// Stack of the clock's speed.
    /// Each stack would decrease time to fall tetrimino 3 seconds.
    /// </summary>
    public int clockSpeedStack = 0;
    int clockPenalty = 2;

    /// <summary>
    /// Resets the clock.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetClock()
    {
        float previousZRotation = clockHand.transform.eulerAngles.z;
        float startTime = Time.time, endTime = startTime + 0.5f;
        GameManager.Instance.tetrisAlert.GetComponent<Animator>().SetBool("isAlertOn", false);
        while (Time.time < endTime)
        {
            yield return null;
            if(tetriminoWaitedTime < timeToFallTetrimino)
                clockHand.transform.eulerAngles = new Vector3(0, 0, previousZRotation - previousZRotation * (Time.time - startTime) / (endTime - startTime));
        }
        clockHand.transform.eulerAngles = Vector3.zero;
        timeToFallTetrimino = initialTimeToFallTetrimino - clockPenalty * clockSpeedStack;
        clockFrame.GetComponent<Image>().color = new Color(1, 1 - (float)20 * clockSpeedStack / 256, 1 - (float)20 * clockSpeedStack / 256);
        tetriminoWaitedTime = 0;
    }

    /// <summary>
    /// Display how much time is it remain to fall current tetrimino.
    /// </summary>
    /// <returns></returns>
    public IEnumerator CountTetriminoWaitingTime()
    {
        yield return null;
        while (!MapManager.isTetriminoFalling)
        {
            yield return null;
            while (GameManager.gameState == GameState.Portal)
            {
                tetriminoCreatedTime += Time.deltaTime;
                yield return null;
            }
            tetriminoWaitedTime = Time.time - tetriminoCreatedTime;
            clockHand.transform.eulerAngles = new Vector3(0, 0, -360 * tetriminoWaitedTime / timeToFallTetrimino);
            if (timeToFallTetrimino - tetriminoWaitedTime <= 3)
                GameManager.Instance.tetrisAlert.GetComponent<Animator>().SetBool("isAlertOn", true);
        }
        if(tetriminoWaitedTime >= timeToFallTetrimino)
            clockHand.transform.eulerAngles = Vector3.zero;
    }

    // Use this for initialization
    void Start () {
        timeToFallTetrimino = initialTimeToFallTetrimino;
        clockFrame = transform.Find("ClockFrame").gameObject;
        clockHand = transform.Find("ClockHand").gameObject;
    }

    // Update is called once per frame
    void Update () {
        
    }
}

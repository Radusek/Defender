using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    void EndNextWaveAnimation()
    {
        GameManager.Instance.nextWaveAnimationPlaying = false;
        GameManager.Instance.nextWaveObject.SetActive(false);
    }

    void MidNextWaveUpdate()
    {
        GameManager.Instance.rbs.Remove(GameManager.Instance.player);
        GameManager.Instance.DestroyAllAircrafts();
        GameManager.Instance.rbs.Add(GameManager.Instance.player);
    }
}

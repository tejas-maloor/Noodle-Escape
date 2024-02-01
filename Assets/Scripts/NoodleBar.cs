using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoodleBar : MonoBehaviour
{
    [SerializeField] Image image;

    public  void UpdateBar(float current, float max)
    {
        image.fillAmount = current / max;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Distance;

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Transform player;

    void Update()
    {


        score.text = player.position.z.ToString("0");
    }
}

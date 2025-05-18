using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerController player;
    public Transform alvo;     // O objeto que a câmera deve seguir (ex: o jogador)
    public float suavizacao = 0.125f;
     public Vector3 deslocamento; // Offset da câmera

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void LateUpdate()
    {
        if (!player.touchBlockSide)
        {
            Vector3 posicaoDesejada = alvo.position + deslocamento;
            Vector3 posicaoSuavizada = Vector3.Lerp(transform.position, posicaoDesejada, suavizacao);
            transform.position = posicaoSuavizada;
        }
    }
}

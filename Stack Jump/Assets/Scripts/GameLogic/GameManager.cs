using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject PainelDefeat;

    public TextMeshProUGUI pontuacaoText;
    private float t;
    public float tCurrent = 6f;
    public float yCurrent = 0.5f;

    public bool playerSucced = true; 
    public bool isGameOver = false;

    public bool isInstanciate = false;
    public int blockCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
       t = tCurrent;
    }

    void InstanciateBlock()
    {
        if (!isInstanciate)
        {
                GameObject block = Instantiate(blockPrefab, new Vector3(0f, yCurrent, 0f), Quaternion.identity);
                //block.transform.SetParent(this.transform);
                block.GetComponent<BlockController>();

            isInstanciate = true;
        }
    }

    void TimerController()
    {

    }

    void FixedUpdate()
    {
        TimerController();

        if (playerSucced)
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
                print("t: " + t);
            }
            else
            {
                isInstanciate = false;
                
                InstanciateBlock();
                t = tCurrent;
                playerSucced = false;
                Debug.Log("Instanciate Block");
            }
        }
    }

    public void GetScussedPayer(bool pSucced, bool isOver)
    {
        playerSucced = pSucced;
        isGameOver = isOver;

        if (playerSucced)
        {
            blockCount++;
            yCurrent++;

            pontuacaoText.text = blockCount.ToString();
            
            Debug.Log("Player Scussed");
            //playerSucced = false;
        }

        if (isGameOver)
        {
            Invoke("SeePainelDefeat", 1f);
        }
    }

    void SeePainelDefeat()
    {
        PainelDefeat.SetActive(true);
    }
}

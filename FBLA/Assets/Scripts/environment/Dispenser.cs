using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public GameObject block;
    public GameObject spawnPos;
    public float startGrav;
    GameObject dispBlock;

    public void Dispense()
    {
        if (dispBlock != null)
            Destroy(dispBlock);
        GameObject newBlock = Instantiate(block);
        newBlock.transform.position = spawnPos.transform.position;
        newBlock.GetComponent<Block>().gravity = startGrav;
        dispBlock = newBlock;
    }
}

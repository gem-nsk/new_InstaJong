using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoText : MonoBehaviour
{

    public string Key;
    public string EncryptedKey;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EncryptedKey = EncryptStringSample.Crypto.Encrypt(Key, "InstaJong");
        }
    }
}

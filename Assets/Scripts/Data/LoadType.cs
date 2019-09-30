using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface LoadType
{

    void GetPosts();

    int GetProgress();

    void Load(int args);
    IEnumerator Loading(string id);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrustEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("ShowEffect");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("asd");
	}

    IEnumerator ShowEffect()
    {
        GetComponent<ParticleSystem>().Play(true);
        yield return new WaitForSeconds(3f);
        GetComponent<ParticleSystem>().Stop();
        gameObject.SetActive(false);
    }
}

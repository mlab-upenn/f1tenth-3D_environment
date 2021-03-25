using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SwitchMethod : MonoBehaviour
{
    [SerializeField]
    private GameObject secondCar;
    private bool isEnable;
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(Switch);
		isEnable = true;
        secondCar.SetActive(isEnable);
	}

	private void Switch(){
        isEnable ^= true;
		if (isEnable) {
            GameObject.Find("Main Camera").GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
            secondCar.SetActive(isEnable);
            GameObject.Find("Main Camera2").GetComponent<Camera>().enabled = isEnable;
        } else {
            GameObject.Find("Main Camera").GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
            GameObject.Find("Main Camera2").GetComponent<Camera>().enabled = isEnable;
            secondCar.SetActive(isEnable);
        }
	}
}

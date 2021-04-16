using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteDropItem : MonoBehaviour
{
    Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnUIButtonClick());
    }
    public void OnUIButtonClick()
    {
        Destroy(transform.parent.gameObject);
    }
}

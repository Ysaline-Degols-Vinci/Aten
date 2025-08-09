using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipController : MonoBehaviour
{

    public static ItemTooltipController Instance;

    public GameObject tooltipObject;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        HideTooltip();
    }

    private void Update()
    {
        if (tooltipObject.activeSelf)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipObject.transform.parent as RectTransform,
                Input.mousePosition,
                null, // camera null = Screen Space Overlay
                out position);
            tooltipObject.transform.localPosition = position;
        }
    }

    public void ShowTooltip(string itemName, string text)
    {
        tooltipObject.SetActive(true);
        this.itemName.text = itemName;
        description.text = text;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}

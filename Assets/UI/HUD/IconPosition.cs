using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconPosition : MonoBehaviour
{
    private Vector3 iconPos;
    private float vertPos;
    private Image buttonImage;
    private Color originalColour;
    public Material selectedMaterial;
    // Start is called before the first frame update
    void Start()
    {
        iconPos = transform.localPosition;
        vertPos = iconPos.y;
        buttonImage = GetComponent<Image>();
        originalColour = buttonImage.color;
    }

    //-150, -190, -270
    //At close to top make size increase
    void Update()
    {
        iconPos = transform.localPosition;
        vertPos = iconPos.y;

        if(vertPos>-330)
        {
            //set associated material as shooting material
            GameObject.Find("Pointer").GetComponent<StaffMovement>().selectedMaterial = selectedMaterial;
            Debug.Log("itemswap");
        }

        if (vertPos > -355)
        {
            //Currently selected item, fully visible when available
            buttonImage.color = new Color(originalColour.r, originalColour.g, originalColour.b, 1.0f);
        }
        if (vertPos >= -435 && vertPos < -355)
        {
            //currently next and previous items, partially visible
            buttonImage.color = new Color(originalColour.r, originalColour.g, originalColour.b, 0.5f);
        }
        if (vertPos < -435)
        {
            //Below screen, hide
            buttonImage.color = new Color(originalColour.r, originalColour.g, originalColour.b, 0.0f);

        }
    }
}

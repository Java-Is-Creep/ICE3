﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrousel : MonoBehaviour
{


    public RectTransform[] images;
    public RectTransform view_window;

    private bool canSwipe;
    private float image_width;
    private float lerpTimer;
    private float lerpPosition;
    private float mousePositionStartX;
    private float mousePositionEndX;
    private float dragAmount;
    private float screenPosition;
    private float lastScreenPosition;
    /// <summary>
    /// Space between images.
    /// </summary>
    public float image_gap = 30;

    public int swipeThrustHold = 30;
    [HideInInspector]
    /// <summary>
    /// The index of the current image on display.
    /// </summary>
    public int current_index;

    public Text titel;

    #region mono
    // Use this for initialization
    void Start()
    {
        image_width = view_window.rect.width;
        for (int i = 1; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(((image_width + image_gap) * i), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        titel.text = current_index.ToString();

        lerpTimer = lerpTimer + Time.deltaTime;

        if (lerpTimer < 0.333f)
        {
            screenPosition = Mathf.Lerp(lastScreenPosition, lerpPosition * -1, lerpTimer * 3);
            lastScreenPosition = screenPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            canSwipe = true;
            mousePositionStartX = Input.mousePosition.x;
        }


        if (Input.GetMouseButton(0))
        {
            if (canSwipe)
            {
                mousePositionEndX = Input.mousePosition.x;
                dragAmount = mousePositionEndX - mousePositionStartX;
                screenPosition = lastScreenPosition + dragAmount;
            }
        }

        if (Mathf.Abs(dragAmount) > swipeThrustHold && canSwipe)
        {
            canSwipe = false;
            lastScreenPosition = screenPosition;
            if (current_index < images.Length)
                OnSwipeComplete();
            else if (current_index == images.Length && dragAmount < 0)
                lerpTimer = 0;
            else if (current_index == images.Length && dragAmount > 0)
                OnSwipeComplete();
        }

        /*for (int i = 0; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(screenPosition + ((image_width + image_gap) * i), 0);
        }*/
        for (int i = 0; i < images.Length; i++)
        {

            images[i].anchoredPosition = new Vector2(screenPosition + ((image_width + image_gap) * i), 0);

            if (i == current_index)
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5);
                Color temp = images[i].GetComponent<Image>().color;
                images[i].GetComponent<Image>().color = new Color(temp.r, temp.g, temp.b, 1);
            }
            else
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5);
                Color temp = images[i].GetComponent<Image>().color;
                images[i].GetComponent<Image>().color = new Color(temp.r, temp.g, temp.b, 0.5f);
            }
        }
    }

    #endregion


    #region private methods
    private void OnSwipeComplete()
    {
        lastScreenPosition = screenPosition;

        if (dragAmount > 0)
        {
            if (dragAmount >= swipeThrustHold)
            {
                if (current_index == 0)
                {
                    lerpTimer = 0; lerpPosition = 0;
                }
                else
                {
                    current_index--;
                    lerpTimer = 0;
                    if (current_index < 0)
                        current_index = 0;
                    lerpPosition = (image_width + image_gap) * current_index;
                }
            }
            else
            {
                lerpTimer = 0;
            }
        }
        else if (dragAmount < 0)
        {
            if (Mathf.Abs(dragAmount) >= swipeThrustHold)
            {
                if (current_index == images.Length - 1)
                {
                    lerpTimer = 0;
                    lerpPosition = (image_width + image_gap) * current_index;
                }
                else
                {
                    lerpTimer = 0;
                    current_index++;
                    lerpPosition = (image_width + image_gap) * current_index;
                }
            }
            else
            {
                lerpTimer = 0;
            }
        }
        dragAmount = 0;
    }
    #endregion



    #region public methods
    public void GoToIndex(int value)
    {
        current_index = value;
        lerpTimer = 0;
        lerpPosition = (image_width + image_gap) * current_index;
        screenPosition = lerpPosition * -1;
        lastScreenPosition = screenPosition;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(screenPosition + ((image_width + image_gap) * i), 0);

            if (i == current_index)
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5);
                Color temp = images[i].GetComponent<Image>().color;
                images[i].GetComponent<Image>().color = new Color(temp.r, temp.g, temp.b, 1);
            }
            else
            {
                images[i].localScale = Vector3.Lerp(images[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5);
                Color temp = images[i].GetComponent<Image>().color;
                images[i].GetComponent<Image>().color = new Color(temp.r, temp.g, temp.b, 0.5f);
            }
        }
    }

    public void GoToIndexSmooth(int value)
    {
        current_index = value;
        lerpTimer = 0;
        lerpPosition = (image_width + image_gap) * current_index;
    }
    #endregion

    public void rightArrow()
    {
        if (current_index + 1 < images.Length)
            GoToIndexSmooth(current_index + 1);
    }

    public void leftArrow()
    {
        if (current_index - 1 >= 0)
            GoToIndexSmooth(current_index - 1);
    }
}
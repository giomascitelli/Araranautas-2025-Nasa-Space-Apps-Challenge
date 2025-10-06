using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBottomMenu : MonoBehaviour
{
    [Header("Painel inferior")]
    public GameObject bottomMenu;

    [Header("Botão que sobe/desce")]
    public RectTransform buttonRect;

    [Header("Distância que o botão sobe quando o menu abre")]
    public float moveUpDistance = 150f;

    private bool isOpen = false;
    private Vector2 originalPosition;

    void Start()
    {
        // Guarda a posição inicial do botão
        originalPosition = buttonRect.anchoredPosition;

        // Garante que o menu comece fechado
        bottomMenu.SetActive(false);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        bottomMenu.SetActive(isOpen);

        // Move o botão para cima ou volta à posição original
        if (isOpen)
            buttonRect.anchoredPosition = originalPosition + new Vector2(0, moveUpDistance);
        else
            buttonRect.anchoredPosition = originalPosition;
    }
}

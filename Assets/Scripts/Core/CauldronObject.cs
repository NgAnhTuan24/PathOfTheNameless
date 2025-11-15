using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronObject : MonoBehaviour
{
    [SerializeField] private float interactDistance = 1.5f;

    private Transform player;
    private PlayerHealth playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null || (playerHealth != null && playerHealth.IsDead))
            return;

        OpenCauldronUI();
    }

    void OpenCauldronUI()
    {
        float dis = Vector2.Distance(player.position, transform.position);

        if (dis <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!UI_Manager.Instance.alchemyPanel.activeSelf)
                    UI_Manager.Instance.OpenUI(UI_Manager.Instance.alchemyPanel);
                else
                    UI_Manager.Instance.alchemyPanel.SetActive(false);
            }
        }
        else
        {
            if (UI_Manager.Instance.alchemyPanel.activeSelf)
                UI_Manager.Instance.alchemyPanel.SetActive(false);

            if (Input.GetKeyDown(KeyCode.F))
                Debug.Log("Quá xa lò luyện kim!");
        }
    }

}

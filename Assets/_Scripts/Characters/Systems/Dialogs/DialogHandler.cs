using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Limbo.DialogSystem
{
    public class DialogHandler : MonoBehaviour
    {
        public DialogNode StartingDialog;

        [Header("UI References")]
        public GameObject DialogCanvas;
        public TMP_Text LineText;
        public Transform OptionsParent;
        public Button OptionButtonPrefab;

        private DialogNode currentNode;

        public bool IsPlayerAtDialog = false;

        private GameObject _player;

        private void Awake()
        {
            _player = GameManager.Instance.GetPlayerGameObject();
            GlobalEventsManager.OnTryStartDialog.AddListener(TryStartDialog);
        }

        public void TryStartDialog(DialogNode startingNode)
        {
            if (!IsPlayerAtDialog)
            {
                StartingDialog = startingNode;
                StartDialog();
            }
            else return;
        }

        void StartDialog()
        {
            IsPlayerAtDialog = true;
            DialogCanvas.SetActive(true);

            currentNode = StartingDialog;
            ShowDialogNode(currentNode);
        }

        public void ShowDialogNode(DialogNode node)
        {
            LineText.text = node.LineText;

            // Clear previous buttons
            foreach (Transform child in OptionsParent)
                Destroy(child.gameObject);

            // Create new option buttons
            foreach (var option in node.Options)
            {
                var btn = Instantiate(OptionButtonPrefab, OptionsParent);
                btn.GetComponentInChildren<TMP_Text>().text = option.OptionText;
                btn.onClick.AddListener(() => OnOptionSelected(option));
            }
        }

        void OnOptionSelected(DialogOptionVariant option)
        {
            option.Result?.ApplyResult(_player);

            if (option.NextNode != null)
                ShowDialogNode(option.NextNode);
            else
                EndDialog();
        }

        void EndDialog()
        {
            LineText.text = "";
            foreach (Transform child in OptionsParent)
                Destroy(child.gameObject);
            Debug.Log("Dialog ended.");

            DialogCanvas.SetActive(false);
            StartingDialog = null;
            IsPlayerAtDialog = false;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
using TMPro;

public class UI_InputWindow : MonoBehaviour
{
    [SerializeField] private Heads_Update headsUpdateWindow;

    private TMP_InputField[] inputFields;
    private Button_UI[] right_btns;
    private Button_UI[] left_btns;
    private Button_UI run_btn;
    private string[] codeLines;
    private int[] tabCount;

    public void Awake() {    

        headsUpdateWindow.loadHeads();
        inputFields = new TMP_InputField[8];
        right_btns = new Button_UI[8];
        left_btns = new Button_UI[8];

        for (int i = 0; i < 8; i++) {
            inputFields[i] = transform.Find("inputField" + i).GetComponent<TMP_InputField>();

            inputFields[i].interactable = true;            right_btns[i] = transform.Find("right_btn" + i).GetComponent<Button_UI>();
            left_btns[i] = transform.Find("left_btn" + i).GetComponent<Button_UI>();
        }

        codeLines = new string[8];
        int[] tabCount = {0, 0, 0, 0, 0, 0, 0, 0};
        this.tabCount = tabCount;

        run_btn = transform.Find("run_btn").GetComponent<Button_UI>();

        Show("testing", "1234567890", (string inputText) => {}, () => {});
        // Hide();
    }

    public void Show(string inputString, string validCharacters, Action<string> onRun, Action onClick) {
        gameObject.SetActive(true);

        for (int i = 0; i < 8; i++) {
            inputFields[i].characterLimit = 40;
        }

        // inputField.onValidateInput = (string text, int charIndex, char addedChar) => {
        //     return ValidateCharacters(validCharacters, addedChar);
        // };
        // inputField.text = inputString;

        run_btn.ClickFunc = () => {
            onRun(inputFields[0].text);
            for (int i = 0; i < 8; i++) {
                codeLines[i] = inputFields[i].text;
                inputFields[i].interactable = false;

                right_btns[i].ClickFunc = null;
                left_btns[i].ClickFunc = null;

                // Debug.Log(codeLines[i]);
                // Debug.Log(tabCount[i]);
            }

            run_btn.ClickFunc = null;            headsUpdateWindow.getAttacked();
        };

        right_btns[0].ClickFunc = () => {
            RectTransform rectTransform = inputFields[0].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[0].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[0].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[0] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[0]++;
            }
        };

        right_btns[1].ClickFunc = () => {
            RectTransform rectTransform = inputFields[1].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[1].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[1].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[1] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[1]++;
            }
        };

        right_btns[2].ClickFunc = () => {
            RectTransform rectTransform = inputFields[2].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[2].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[2].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[2] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[2]++;
            }
        };

        right_btns[3].ClickFunc = () => {
            RectTransform rectTransform = inputFields[3].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[3].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[3].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[3] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[3]++;
            }
        };

        right_btns[4].ClickFunc = () => {
            RectTransform rectTransform = inputFields[4].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[4].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[4].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[4] < 2) {
               rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
               btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
               btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
               tabCount[4]++;
            }
        };

        right_btns[5].ClickFunc = () => {
            RectTransform rectTransform = inputFields[5].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[5].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[5].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[5] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[5]++;
            }
        };

        right_btns[6].ClickFunc = () => {
            RectTransform rectTransform = inputFields[6].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[6].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[6].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[6] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[6]++;
            }
        };

        right_btns[7].ClickFunc = () => {
            RectTransform rectTransform = inputFields[7].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[7].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[7].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[7] < 2) {
                rectTransform.anchoredPosition = new Vector3(position.x + 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x + 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x + 30f, btn_posL.y, 0);
                tabCount[7]++;
            }
        };

        left_btns[0].ClickFunc = () => {
            RectTransform rectTransform = inputFields[0].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[0].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[0].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[0] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[0]--;
            }
        };
        
        left_btns[1].ClickFunc = () => {
            RectTransform rectTransform = inputFields[1].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[1].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[1].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[1] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[1]--;
            }
        };

        left_btns[2].ClickFunc = () => {
            RectTransform rectTransform = inputFields[2].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[2].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[2].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[2] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[2]--;
            }
        };

        left_btns[3].ClickFunc = () => {
            RectTransform rectTransform = inputFields[3].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[3].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[3].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[3] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[3]--;
            }
        };

        left_btns[4].ClickFunc = () => {
            RectTransform rectTransform = inputFields[4].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[4].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[4].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[4] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[4]--;
            }
        };

        left_btns[5].ClickFunc = () => {
            RectTransform rectTransform = inputFields[5].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[5].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[5].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[5] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[5]--;
            }
        };

        left_btns[6].ClickFunc = () => {
            RectTransform rectTransform = inputFields[6].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[6].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[6].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[6] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[6]--;
            }
        };

        left_btns[7].ClickFunc = () => {
            RectTransform rectTransform = inputFields[7].GetComponent<RectTransform>();
            Vector3 position = rectTransform.anchoredPosition;
            RectTransform btn_transR = right_btns[7].GetComponent<RectTransform>();
            Vector3 btn_posR = btn_transR.anchoredPosition;
            RectTransform btn_transL = left_btns[7].GetComponent<RectTransform>();
            Vector3 btn_posL = btn_transL.anchoredPosition;
            if (tabCount[7] > 0) {
                rectTransform.anchoredPosition = new Vector3(position.x - 30f, position.y, 0);
                btn_transR.anchoredPosition = new Vector3(btn_posR.x - 30f, btn_posR.y, 0);
                btn_transL.anchoredPosition = new Vector3(btn_posL.x - 30f, btn_posL.y, 0);
                tabCount[7]--;
            }
        };
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private char ValidateCharacters(string validCharacters, char addedChar) {
        if (validCharacters.IndexOf(addedChar) != 1) {
            return addedChar;
        } else {
            return '\0';
        }

    }

    public string[] getCodeLines() {
        return codeLines;
    }
    public int[] getTabCount() {
        return tabCount;
    }
}

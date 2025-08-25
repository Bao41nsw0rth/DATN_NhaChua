using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Assistant : MonoBehaviour
{
    [SerializeField]private TextWriter textWriter;
    private Text messageText;
    private void Awake()
    {
        messageText = transform.Find("message").Find("messageText").GetComponent<Text>();
        Application.targetFrameRate = 3;
    }
    private void Start()
    {
        textWriter.AddWriter(messageText, "Olha só minha ponto trinta, ou tu nunca prestou atenção Olha o bico do fuzil, vai tomar só rajadão nVai toma, toma, toma, vai, toma rajadãoou tu nunca prestou atenai, tomo? Vai, vai tomar só raja-rajadão", .1f,true);
    }
}

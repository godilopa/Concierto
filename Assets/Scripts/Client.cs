using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
  [SerializeField]
  private int actualId = 1;

  [SerializeField]
  private GameObject firstCanvas;

  [SerializeField]
  private GameObject graciasPorVotar;

  [SerializeField]
  private GameObject[] votacion1;

  [SerializeField]
  private GameObject[] votacion2;

  [SerializeField]
  private GameObject[] votacion3;

  private GameObject nextCanvas;

  private GameObject nextCanvasTransport;

  private int isActivate;

  private bool answering;

  private const string readingActivate = "http://phpfiles.tenlladoclarinet.com/readingActivate.php";
  private const string voteTransport = "http://phpfiles.tenlladoclarinet.com/votarTransporte.php";
  private const string readingVotosResult = "http://phpfiles.tenlladoclarinet.com/readingVotosResult.php";
  private const string readingActualId = "http://phpfiles.tenlladoclarinet.com/readingActualId.php";
  private const string voteCountry = "http://phpfiles.tenlladoclarinet.com/votarPais.php";

  private Coroutine cor;

  private float timer = 0;

  private bool alreadyAnswered = false;

  private bool waitingForAnswer = false;

  private UnityWebRequest activateReadingWeb;

  private int firstTransportId = 0;

  public void VoteTrasportQuestion(int id)
  {
    WWWForm form = new WWWForm();
    form.AddField("unityTransporteId", id);
    form.AddField("unityPreguntaId", actualId);
    WWW questionsInfo = new WWW(voteTransport, form);

    alreadyAnswered = true;

    if (isActivate == 0)
    {
      if (graciasPorVotar.activeSelf == false)
      {
        nextCanvasTransport.SetActive(false);
        graciasPorVotar.SetActive(true);
        waitingForAnswer = true;
        answering = false;
      }
    }
  }

  public void VoteCountryQuestion(int id)
  {
    WWWForm form = new WWWForm();
    form.AddField("unityPaisId", id);
    form.AddField("unityPreguntaId", actualId);
    WWW questionsInfo = new WWW(voteCountry, form);

    nextCanvas.SetActive(false);
    nextCanvasTransport.SetActive(true);
  }

  IEnumerator ReadActivate()
  {
    WWWForm activateForm = new WWWForm();
    activateForm.AddField("unityId", actualId);
    activateReadingWeb = UnityWebRequest.Post(readingActivate, activateForm);
    yield return activateReadingWeb.SendWebRequest();
    isActivate = int.Parse(activateReadingWeb.downloadHandler.text);
    //Si esta activo y ya se ha contestado la pregunta, al mirar a la siguiente estara inactiva y 
    //alreadyAnswered se pone a falso para que los botones no vuelvan a salir hasta que isActive este a true
    if (isActivate == 0)
      alreadyAnswered = false;
  }

  IEnumerator ReadAnswer()
  {
    WWWForm form = new WWWForm();
    form.AddField("unityId", actualId);
    WWW preguntasInfo = new WWW(readingVotosResult, form);
    yield return preguntasInfo;
    string[] info = preguntasInfo.text.Split(';');

    int countryId = GetDataValue(info[1], "PaisId:");
    int transportId = GetDataValue(info[0], "TransporteId:");
    firstTransportId = GetDataValue(info[2], "PrimerTransporteId:");

    if (countryId != 0 && transportId != 0)
    {
      //Seleccionamos los siguientes canvas
      if (actualId == 1)
      {
        if (countryId == 1)
          nextCanvas = votacion2[0];
        else if (countryId == 2)
          nextCanvas = votacion2[1];
        else if (countryId == 3)
          nextCanvas = votacion2[2];

        firstTransportId = transportId;

        if (transportId == 1)
          nextCanvasTransport = votacion2[3];
        else if (transportId == 2)
          nextCanvasTransport = votacion2[4];
        else if (transportId == 3)
          nextCanvasTransport = votacion2[5];
        else if (transportId == 4)
          nextCanvasTransport = votacion2[6];
      }
      else if (actualId == 2)
      {
        if (firstTransportId == 1)
        {
          if (transportId == 2)
            nextCanvasTransport = votacion3[0];
          else if (transportId == 3)
            nextCanvasTransport = votacion3[1];
          else if (transportId == 4)
            nextCanvasTransport = votacion3[2];
        }

        if (firstTransportId == 2)
        {
          if (transportId == 1)
            nextCanvasTransport = votacion3[0];
          else if (transportId == 3)
            nextCanvasTransport = votacion3[3];
          else if (transportId == 4)
            nextCanvasTransport = votacion3[4];
        }

        if (firstTransportId == 3)
        {
          if (transportId == 1)
            nextCanvasTransport = votacion3[1];
          else if (transportId == 2)
            nextCanvasTransport = votacion3[3];
          else if (transportId == 4)
            nextCanvasTransport = votacion3[5];
        }

        if (firstTransportId == 4)
        {
          if (transportId == 1)
            nextCanvasTransport = votacion3[2];
          else if (transportId == 2)
            nextCanvasTransport = votacion3[4];
          else if (transportId == 3)
            nextCanvasTransport = votacion3[5];
        }
      }

      waitingForAnswer = false;
      actualId++;
    }
  }

  //1 Argentina, canvas d india e israel
  //2 India, canvas de los otro 2
  //3 Israel, same

  //1 Avion
  //2 Coche
  //3 Nave espacial
  //4 Tren

  private int GetDataValue(string data, string field)
  {
    string value = data.Substring(data.IndexOf(field) + field.Length);

    if (value.Contains("|"))
      value = value.Remove(value.IndexOf("|"));

    return int.Parse(value);
  }

  private void DeactiveButton(Button[] buttons, int id)
  {
    for (int i = 0; i < buttons.Length; i++)
    {
      if (string.Compare(buttons[i].gameObject.name, id.ToString()) == 0)
      {
        if (buttons[i].IsInteractable() == true)
          buttons[i].interactable = false;

        return;
      }
    }
  }

  //private void AddDebugText(string t)
  //{
  //  debugText.text = string.Format("{0}\n{1}", debugText.text, t);
  //}

  private void Awake()
  {
    activateReadingWeb = new UnityWebRequest();
    nextCanvas = votacion1[0];
    nextCanvasTransport = votacion1[1];
  }

  IEnumerator Start()
  {
    WWW readingWeb = new WWW(readingActualId);
    yield return readingWeb;
    actualId = int.Parse(readingWeb.text);

    if (actualId == 4)
      enabled = false;

    if (actualId != 1)
    {
      waitingForAnswer = true;
      actualId--;//Porque e nreadasnwer va a incrementarlo
    }
  }

  private void Update()
  {
    if (waitingForAnswer == true)
    {
      timer += Time.deltaTime;

      if (timer > 1)
      {
        if (cor != null)
          StopCoroutine(cor);

        cor = StartCoroutine(ReadAnswer());
        timer = 0;
      }
    }
    else
    {
      timer += Time.deltaTime;

      if (timer > 1)
      {
        if (cor != null)
          StopCoroutine(cor);

        cor = StartCoroutine(ReadActivate());
        timer = 0;
      }


      if (isActivate != 0 && alreadyAnswered == false)
      {
        if (graciasPorVotar.activeSelf == true)
          graciasPorVotar.SetActive(false);

        if (firstCanvas.activeSelf == true)
          firstCanvas.SetActive(false);

        if (actualId != 3)
        {
          if (nextCanvas.activeSelf == false && answering == false)
            nextCanvas.SetActive(true);
        }
        else if (nextCanvasTransport.activeSelf == false && answering == false)
          nextCanvasTransport.SetActive(true);

        answering = true;
      }
      else if (isActivate != 0 && alreadyAnswered == true)
      {
        if (graciasPorVotar.activeSelf == false)
        {
          nextCanvasTransport.SetActive(false);
          graciasPorVotar.SetActive(true);
          waitingForAnswer = true;
          answering = false;
          isActivate = 0;
        }
      }
    }
  }
}

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum TransportId { Default, A, B, C, D }

public enum CountryId { Default, A, B, C, D }

public class Admin : MonoBehaviour
{
  public Text debugText;

  private List<TransportId> transportsId = new List<TransportId>();

  private List<CountryId> countriesId = new List<CountryId>();

  [SerializeField]
  private int sizeBar = 10;

  [SerializeField]
  private int[] transportsWinner = new int[2];

  [SerializeField]
  private int[] countriesWinner = new int[1];

  [SerializeField]
  private GameObject[] transportsGraphic;

  [SerializeField]
  private GameObject[] countriesGraphic;

  private string[] info;

  [SerializeField]
  private int actualId = 0;

  private int idBestTransport = 0;

  private int idBestCountry = 0;

  private const string readingVotos = "http://phpfiles.tenlladoclarinet.com/readingVotos.php";
  private const string writingVotosResult = "http://phpfiles.tenlladoclarinet.com/writingVotosResult.php";
  private const string activate = "http://phpfiles.tenlladoclarinet.com/activate.php";
  private const string readingActualId = "http://phpfiles.tenlladoclarinet.com/readingActualId.php";

  public void ShowVotos()
  {
    StartCoroutine(ReadingVotos());
  }

  IEnumerator ReadingVotos()
  {
    WWWForm form = new WWWForm();
    form.AddField("unityVotosId", actualId);
    WWW preguntasInfo = new WWW(readingVotos, form);
    yield return preguntasInfo;
    info = preguntasInfo.text.Split(';');

    CountVotes(0, false, "IdTransporte:");

    if(actualId != 3)
      CountVotes(4, true, "IdPais:");

    //Actualizamos los resultados y luego aumentamos el id
    StartCoroutine(WritingVotosResult());
    actualId++;
  }

  IEnumerator WritingVotosResult()
  {
    WWWForm form = new WWWForm();
    form.AddField("unityId", actualId);
    form.AddField("transporteId", idBestTransport);
    form.AddField("paisId", idBestCountry);
    WWW preguntasInfo = new WWW(writingVotosResult, form);
    yield return preguntasInfo;
  }

  private int GetDataValue(string data, string field)
  {
    string value = data.Substring(data.IndexOf(field) + field.Length);

    if (value.Contains("|"))
      value = value.Remove(value.IndexOf("|"));

    return int.Parse(value);
  }

  public void ActivateQuestion()
  {
    WWWForm form = new WWWForm();
    form.AddField("valor", 1);
    form.AddField("unityId", actualId);
    WWW preguntasInfo = new WWW(activate, form);
  }

   public void DoAddDebugText(string t)
  {
    debugText.text = string.Format("{0}\n{1}", debugText.text, t);
  }

  public void CloseQuestion()
  {
    WWWForm form = new WWWForm();
    form.AddField("valor", 0);
    form.AddField("unityId", actualId);
    WWW preguntasInfo = new WWW(activate, form);
  }

  //StartIndex 0 a 3 transporte 5 a 8 pais
  private void CountVotes(int startIndex, bool isCountry, string id)
  {
    int count = 0;
    int until = 0;

    if (isCountry == true)
    {
      count = countriesId.Count;

      if (actualId != 3)
        until = 3;
    }
    else
    {
      count = transportsId.Count;
      until = 4;
    }

    float totales = 0;
    List<float> votos = new List<float>();
    int maxIdResult = 0;
    int lastResult = 0;

    for (int i = startIndex; i < startIndex + until; i++)
    {
      int checker = GetDataValue(info[i], id);

      if (isCountry == true)
      {
        if (CheckCountryIdOnTheList(checker) == false)
          continue;
      }
      else
      { 
        if (CheckTransportIdOnTheList(checker) == false)
          continue;
      }

      float value = GetDataValue(info[i], "Votos:");

      votos.Add(value);
      totales += value;

      if ((int)value >= lastResult)
      {
        maxIdResult = i % 4 + 1;
        lastResult = (int)value;
      }
    }


    for (int i = 0; i < count; i++)
    {
      string name = string.Empty;

      if (isCountry == false)
      {
        if (TransportName((int)transportsId[i]) == TransportName(maxIdResult))
        {
          votos[i] = votos[i] + 1;
          Debug.Log(TransportName((int)transportsId[i]) + TransportName(maxIdResult - 1) + maxIdResult);
        }

        name = TransportName((int)transportsId[i]);
        transportsGraphic[i].GetComponentInChildren<Text>().text = string.Format("{0}: {1}", name, votos[i]);
        transportsGraphic[i].GetComponentInChildren<Image>().rectTransform.sizeDelta= new Vector2(transportsGraphic[i].GetComponentInChildren<Image>().rectTransform.sizeDelta.x, sizeBar * votos[i]);
        transportsGraphic[i].GetComponentInChildren<Image>().color = TransportColor((int)transportsId[i]);
        transportsGraphic[i].SetActive(true);
      }
      else
      {
        if (CountryName((int)countriesId[i]) == CountryName((maxIdResult)))
        {
          votos[i] = votos[i] + 1;
          Debug.Log(CountryName((int)transportsId[i]) + CountryName(maxIdResult - 1) + maxIdResult);
        }

        name = CountryName((int)countriesId[i]);
        countriesGraphic[i].GetComponentInChildren<Text>().text = string.Format("{0}: {1}", name, votos[i]);
        countriesGraphic[i].GetComponentInChildren<Image>().rectTransform.sizeDelta = new Vector2(countriesGraphic[i].GetComponentInChildren<Image>().rectTransform.sizeDelta.x, sizeBar * votos[i]);
        countriesGraphic[i].GetComponentInChildren<Image>().color = CountryColor((int)countriesId[i]);
        countriesGraphic[i].SetActive(true);
      }
    }

    if (isCountry == true)
    {
      idBestCountry = maxIdResult;
      RemoveFromCountryList(idBestCountry);
    }
    else
    {
      idBestTransport = maxIdResult;
      RemoveFromTransportList(idBestTransport);
    }
  }

  private bool CheckTransportIdOnTheList(int id)
  {
    for (int i = 0; i < transportsId.Count; i++)
    {
      if (id == (int)transportsId[i])
        return true;
    }

    return false;
  }

  private bool CheckCountryIdOnTheList(int id)
  {
    for (int i = 0; i < countriesId.Count; i++)
    {
      if (id == (int)countriesId[i])
        return true;
    }

    return false;
  }

  private void RemoveFromTransportList(int id)
  {
    switch (id)
    {
      case 1:
        transportsId.Remove(TransportId.A);
        break;
      case 2:
        transportsId.Remove(TransportId.B);
        break;
      case 3:
        transportsId.Remove(TransportId.C);
        break;
      case 4:
        transportsId.Remove(TransportId.D);
        break;
    }
  }

  private void RemoveFromCountryList(int id)
  {
    switch (id)
    {
      case 1:
        countriesId.Remove(CountryId.A);
        break;
      case 2:
        countriesId.Remove(CountryId.B);
        break;
      case 3:
        countriesId.Remove(CountryId.C);
        break;
    }
  }

  private string TransportName(int id)
  {
    string name = string.Empty;

    switch (id)
    {
      case 1:
        name = "Avión";
        break;
      case 2:
        name = "Coche";
        break;
      case 3:
        name = "Nave espacial";
        break;
      case 4:
        name = "Tren";
        break;
    }

    return name;
  }

  private Color TransportColor(int id)
  {
    Color color = new Color();

    switch (id)
    {
      case 1:
        ColorUtility.TryParseHtmlString("#63BDC8FF", out color);
        break;
      case 2:
        ColorUtility.TryParseHtmlString("#D1A017FF", out color);
        break;
      case 3:
        ColorUtility.TryParseHtmlString("#DC3E33FF", out color);
        break;
      case 4:
        ColorUtility.TryParseHtmlString("#547D36FF", out color);
        break;
    }

    return color;
  }

  private string CountryName(int id)
  {
    string name = string.Empty;

    switch (id)
    {
      case 1:
        name = "Argentina";
        break;
      case 2:
        name = "India";
        break;
      case 3:
        name = "Israel";
        break;
    }

    return name;
  }

  private Color CountryColor(int id)
  {
    Color color = new Color();

    switch (id)
    {
      case 1:
        ColorUtility.TryParseHtmlString("#A73F35FF", out color);
        break;
      case 2:
        ColorUtility.TryParseHtmlString("#FFF852FF", out color);
        break;
      case 3:
        ColorUtility.TryParseHtmlString("#9E437AFF", out color);
        break;
    }

    return color;
  }

  IEnumerator Start()
  {
    if (transportsWinner[0] != 1 && transportsWinner[1] != 1)
      transportsId.Add(TransportId.A);

    if (transportsWinner[0] != 2 && transportsWinner[1] != 2)
      transportsId.Add(TransportId.B);

    if (transportsWinner[0] != 3 && transportsWinner[1] != 3)
      transportsId.Add(TransportId.C);

    if (transportsWinner[0] != 4 && transportsWinner[1] != 4)
      transportsId.Add(TransportId.D);

    if (countriesWinner[0] != 1 && countriesWinner[1] !=1)
      countriesId.Add(CountryId.A);

    if (countriesWinner[0] != 2 && countriesWinner[1] != 2)
      countriesId.Add(CountryId.B);

    if (countriesWinner[0] != 3 && countriesWinner[1] != 3)
      countriesId.Add(CountryId.C);

    WWW readingWeb = new WWW(readingActualId);
    yield return readingWeb;

    actualId = int.Parse(readingWeb.text);

    if (actualId == 3)
      countriesId.Clear();
  }
}


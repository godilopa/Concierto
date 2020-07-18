using System.Collections;
using UnityEngine;

public class StressOut : MonoBehaviour
{
  private const string voteCountry = "http://phpfiles.tenlladoclarinet.com/votarPais.php";
  private bool next = true;
  private const int iterationNumber = 100;

  IEnumerator VoteCountryQuestion(int id)
  {
    WWWForm form = new WWWForm();
    form.AddField("unityPaisId", id);
    form.AddField("unityPreguntaId", 2);
    WWW questionsInfo = new WWW(voteCountry, form);
    yield return questionsInfo;
    next = true;
  }

  void Update()
  {
    for (int i = 0; i < iterationNumber; i++)
    {
      if (next == true)
      {
        next = false;
        Debug.Log(i);
        StartCoroutine(VoteCountryQuestion(2));
      }
    }
  }
}
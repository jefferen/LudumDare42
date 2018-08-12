using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFunctionality : MonoBehaviour
{
    float index = 0;
    public bool fall, voidSucker, begone; // enum perhaps?
    public GameObject voidExplo;
    HexagonLvlBuilder myRef;

    IEnumerator Fall(float speed)
    {
        index = Time.deltaTime * speed;

        while (index < 10)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - index, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }

    void PillarBegone()
    {
        Debug.Log("Begone thot");
        StartCoroutine("Fall", 2.5f); // do make a visual clue that it will fall! No time, sorry
    }

    public void StopEveryThing() // keep getting error!
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    public void Amargedon(HexagonLvlBuilder AbsoluteMadLad) // eh
    {
        myRef = AbsoluteMadLad; // fuuuu! When am I resetting this value?
        fall = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!begone && !fall || !collision.gameObject.CompareTag("Player")) return;

        if (fall) StartCoroutine("Fall", Random.Range(0.1f,0.7f));
        else if (begone) PillarBegone();

        if (myRef != null) myRef.FallPillar(10, 20); // on the highest lvl make the rest off the pillars fall on contact

        begone = false; // that way it won't accidently be called a second time!
    }

    public void CustumOnDisable()
    {
        if (voidSucker)
        {
            GameObject g = Instantiate(voidExplo, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        }
        gameObject.SetActive(false);
    }
}

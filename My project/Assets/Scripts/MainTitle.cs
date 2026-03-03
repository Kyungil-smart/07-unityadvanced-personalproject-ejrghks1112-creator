using System.Collections;
using UnityEngine;

public class MainTitle : MonoBehaviour
{
    private static readonly int Wait = Animator.StringToHash("Wait");
    private WaitForSeconds _winkDelay;
    Animator _titleAnim;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        StartCoroutine(TitleCoroutine());
    }

    void Init()
    {
        _titleAnim = GetComponentInChildren<Animator>();
        _winkDelay = new WaitForSeconds(10f);
    }

    IEnumerator TitleCoroutine()
    {
        while (true)
        {
            yield return _winkDelay;
            
            if (_titleAnim != null)
            {
                _titleAnim.SetTrigger("Wait");
            }
        }
    }
}

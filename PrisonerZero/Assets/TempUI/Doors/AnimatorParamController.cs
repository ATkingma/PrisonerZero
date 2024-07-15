using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParamController : MonoBehaviour
{
    [SerializeField]
    private int startingValue;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private string name;

    private void Start()
    {
        SetAnimValue(startingValue);
    }

    public void SetAnimValue(int _newValue)
    {
        anim.SetInteger(name, _newValue);   
    }

    public void SetAnimValueDelayed(int value)
    {
        StartCoroutine(DelayedValueSet(value));
    }

    IEnumerator DelayedValueSet(int value)
    {
        yield return new WaitForSeconds(1);
        SetAnimValue(value);
    }
}

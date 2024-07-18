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
    private string paramName;

    private void Start()
    {
        if (this.isActiveAndEnabled)
            SetAnimValue(startingValue);
    }

    public void SetAnimValue(int _newValue)
    {
        if (this.isActiveAndEnabled)
            anim.SetInteger(paramName, _newValue);   
    }

    public void SetAnimValueDelayed(int value)
    {
        if(this.isActiveAndEnabled)
            StartCoroutine(DelayedValueSet(value));
    }

    IEnumerator DelayedValueSet(int value)
    {
        yield return new WaitForSeconds(1);
        SetAnimValue(value);
    }
}

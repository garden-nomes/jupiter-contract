using System;
using System.Collections;
using UnityEngine;

public static class Helpers
{
    public static IEnumerator DelayedAction(Action action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        yield return new WaitForEndOfFrame();
        action.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public static class AnalyticsEventsController 
{
    public static void ActivateAnalytics()
    {
#if UNITY_EDITOR
        Debug.Log("event sended");
#elif UNITY_IOS || UNITY_ANDROID

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
#endif

    }

    public static void LogEvent(string key, string valueName, string value)
    {
#if UNITY_EDITOR
        Debug.Log("event sended - " + key);

#elif UNITY_IOS || UNITY_ANDROID
        FirebaseAnalytics.LogEvent(key, valueName, value);
        Debug.Log("Sended log event - " + key + " value name - " + valueName + " value - " + value);
#endif
    }

    public static void LogEvent(string key)
    {
#if UNITY_EDITOR
        Debug.Log("event sended - " + key);

#elif UNITY_IOS || UNITY_ANDROID
        FirebaseAnalytics.LogEvent(key);
        Debug.Log("Sended log event - " + key);
#endif
    }

    public static void LogPlayerLevel(int level)
    {
#if UNITY_EDITOR
        Debug.Log("event sended");

#elif UNITY_IOS || UNITY_ANDROID
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.ParameterLevel, "reached level", level);
        Debug.Log("Sended log event - level: " + level);

#endif
    }
}

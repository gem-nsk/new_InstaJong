using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

[System.Serializable]
public struct _notification
{
    public Color _Color;
    public string _Header;
    public string _Message;
}

public class Notifications : MonoBehaviour
{
    int identifier;
    public _notification[] _NotificationList;

    private void Start()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        CreateRandomNotification(System.DateTime.Now.AddHours(4));
        CreateRandomNotification(System.DateTime.Now.AddHours(8));
        CreateRandomNotification(System.DateTime.Now.AddDays(1));
        CreateRandomNotification(System.DateTime.Now.AddDays(3));
        CreateRandomNotification(System.DateTime.Now.AddDays(7));
    }

    public void CreateRandomNotification(System.DateTime time)
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "instajong",
            Name = "Default Instajong Channel",
            Importance = Importance.Default,
            Description = "Generic notifications"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        _notification _n = GetRandomNotification();

        var _NewNotification = new AndroidNotification()
        {
            Title = _n._Header,
            Text = _n._Message,
            Color = _n._Color,
            FireTime = time,
            SmallIcon = "icon_0"
        };

        identifier = AndroidNotificationCenter.SendNotification(_NewNotification, channel.Id);

        AndroidNotificationCenter.NotificationReceivedCallback receivedCallback = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n Title: " + data.Notification.Title;
            msg += "\n Body: " + data.Notification.Text;
            msg += "\n Channel: " + data.Channel;
        };

        AndroidNotificationCenter.OnNotificationReceived += ReceivedNotificationHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if(notificationIntentData != null)
        {
            Debug.Log("App was opened with notification!");
        }
    }

    private void ReceivedNotificationHandler(AndroidNotificationIntentData data)
    {
        throw new System.NotImplementedException();
    }


    _notification GetRandomNotification()
    {
        return _NotificationList[Random.Range( 0, _NotificationList.Length - 1)];
    }
}

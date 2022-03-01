using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject specialLevel;
    void Start()
    {
        if (PlayerPrefs.GetInt("lastDay", 1) != System.DateTime.Now.DayOfYear)
        {
            specialLevel.SetActive(true);
            PlayerPrefs.SetInt("dailylevel", (System.DateTime.Now.DayOfYear) % 30);
            CreateNotificationChannel();
            SendNotification();
        }
    }
    public void CreateNotificationChannel()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }
    public void SendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "New challenge playable";
        notification.Text = "Play the newest challenge and try to get 250 coins";
        System.DateTime notiTime = System.DateTime.Now.AddDays(2);
        notification.FireTime = notiTime.AddHours(-1);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}

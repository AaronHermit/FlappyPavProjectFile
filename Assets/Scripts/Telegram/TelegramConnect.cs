using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TelegramConnect : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void ShowAlert(string text);
    //[DllImport("__Internal")]
    //public static extern void Ready();
    //[DllImport("__Internal")]
    //public static extern void Close();
    //[DllImport("__Internal")]
    //public static extern void Expand();
    [DllImport("__Internal")]
    public static extern void HapticFeedback(string level);
    [DllImport("__Internal")]
    public static extern string GetUserData();
    [DllImport("__Internal")]
    public static extern int GetUserId();

    

}

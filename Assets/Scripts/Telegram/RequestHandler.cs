using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestHandler : MonoBehaviour
{

    [Serializable]
    public class UserData
    {
        public User user;
        public string chat_instance;
        public string chat_type;
        public string auth_date;
        public string hash;
    }

    [Serializable]
    public class User
    {
        public long id;
        public string first_name;
        public string last_name;
        public string language_code;
        public string username;
        public bool allows_write_to_pm;
    }



}

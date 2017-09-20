using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CouchBaseChatApp.DataModels
{
   public  class MessageModel
    {
        public string Id { get; set; }
        public string  Message { get; set; }
        //public DateTime Time { get; set; }

    }
}
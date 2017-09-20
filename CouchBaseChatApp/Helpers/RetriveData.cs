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
using CouchBaseChatApp.DataModels;

namespace CouchBaseChatApp.Helpers
{
   public  class RetriveData
    {
        public static List<MessageModel> MessageList { get; set; } = new List<MessageModel>();
        public RetriveData()
        {
            //if (MessageList==null)
            //{
            //    MessageList = new List<MessageModel>();
            //}
           
        }
    }
}
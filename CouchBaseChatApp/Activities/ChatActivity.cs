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
using CouchBaseChatApp.Database;
using CouchBaseChatApp.Adapters;
using CouchBaseChatApp.Helpers;
using Couchbase.Lite;
using Newtonsoft.Json;
using MongoDB.Bson;
using Android.Util;

namespace CouchBaseChatApp.Activities
{
    [Activity(Label = "ChatActivity", MainLauncher = true)]
    public class ChatActivity : Activity
    {
        private ListView mListView;
        private Button mButton;
        private EditText mEditText;
        DbStorage db;
        Couchbase.Lite.Database db2;

        List<MessageModel> messagesList { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            db2 = Manager.SharedInstance.GetDatabase("dbase");///test code
            messagesList = new List<MessageModel>();
            db = new DbStorage();
            FindViews();
            HandleEvents();
            messagesList.Clear();
            // messagesList= db.GetItems();
            GetItems();
            mListView.Adapter = new MessagesAdapter(this, messagesList);
            StartSync();
        }

        void FindViews()
        {
            mListView = FindViewById<ListView>(Resource.Id.messageList);
            mButton = FindViewById<Button>(Resource.Id.sendButton);
            mEditText = FindViewById<EditText>(Resource.Id.messageEditor);
        }

        void HandleEvents()
        {
            mButton.Click += SendMessageCliked;
        }

        private void SendMessageCliked(object sender, EventArgs e)
        {

            //db.SaveItem(new MessageModel { Id = Guid.NewGuid().ToString(), Message = mEditText.Text });
            saveItems(new MessageModel { Id = Guid.NewGuid().ToString(), Message = mEditText.Text });
            messagesList.Clear();
            GetItems();
            // mListView.Adapter = new MessagesAdapter(this, messagesList);
            mListView.SetSelection(mListView.Adapter.Count - 1);

        }

        ///////testing code///////
        void saveItems(MessageModel item)
        {
            try
            {
                Document doc;
                if (item.Id != null)
                {
                    doc = db2.CreateDocument();
                    var jsonData = JsonConvert.SerializeObject(item);
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
                    doc.PutProperties(values);
                    doc.Id = item.Id;
                    Toast.MakeText(Application.Context, "Item saved", ToastLength.Short).Show();
                    StartSync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Toast.MakeText(Application.Context, "failed to save item", ToastLength.Short);
            }
            finally
            {
                mButton.Text = "send" + messagesList.Count().ToString();
            }

        }

        // http://10.0.2.2:4985/_admin/db/sync_gateway

        void GetItems()
        {
            // var qry = db2.CreateAllDocumentsQuery();
            // var res = qry.Run();
            //// List<MessageModel> list = new List<MessageModel>();
            // foreach (var item in res)
            // {
            //     // var x = item.Document.ToJson();
            //     var jsondata = item.Document.UserProperties.ToJson();
            //     var datamodel = JsonConvert.DeserializeObject<MessageModel>(jsondata);
            //     messagesList.Add(datamodel);

            // }
            //// return messagesList;


            ///testcode

            try
            {
                var qry = db2.CreateAllDocumentsQuery();
                var res = qry.Run();
                foreach (var item in res)
                {
                    var x = item.Document.ToJson();
                    var prop = item.Document.UserProperties;
                    var id = item.Document.UserProperties["Id"].ToString();
                    var mess = item.Document.UserProperties["Message"].ToString();

                    // var z = JsonConvert.DeserializeObject<MessageModel>(y);

                    messagesList.Add(new MessageModel { Id = id, Message = mess });
                    Console.WriteLine(item.Document.UserProperties.ToJson());//.ToJson());//.ToJson<RootObject>());//.UserProperties);
                    mListView.Adapter = new MessagesAdapter(this, messagesList);
                }
                int count = messagesList.Count; ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                // Log.Error("inside GetItems() in mainactivity ",ex.StackTrace);
                
            }



        }
        //4985/_admin/db/sync_gateway
        private String mSyncGatewayUrl = "http://192.168.10.180:4984/dbase/";
        //"http://10.0.2.2:4984/sync_gateway/";

        void StartSync()
        {
            try
            {

                Uri _url = new Uri(mSyncGatewayUrl);
                Replication puller = Manager.SharedInstance.GetDatabase("dbase").CreatePullReplication(_url);
                Replication pusher = Manager.SharedInstance.GetDatabase("dbase").CreatePushReplication(_url);
                puller.Continuous = true;
                pusher.Continuous = true;
                puller.Changed += Puller_Changed;
                pusher.Changed += Pusher_Changed;
                puller.Start();
                pusher.Start();
                ReplicationStatus x= puller.Status;
                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

            }
        }
        int pushercount = 0;
        int pullercount = 0;
        private void Pusher_Changed(object sender, ReplicationChangeEventArgs e)
        {
           
            Toast.MakeText(this,"pusher count"+pushercount,ToastLength.Short).Show();
            messagesList.Clear();
            GetItems();
            pushercount++;
            mListView.SetSelection(mListView.Adapter.Count-1);
        }

        private void Puller_Changed(object sender, ReplicationChangeEventArgs e)
        {
          
            Toast.MakeText(this, "puller count"+ pullercount, ToastLength.Short).Show();
            messagesList.Clear();
            GetItems();
            mListView.SetSelection(mListView.Adapter.Count - 1);
        }
         
    }
}
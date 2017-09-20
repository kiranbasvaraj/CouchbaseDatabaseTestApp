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
using System.Threading.Tasks;
using Couchbase.Lite;
using CouchBaseChatApp.DataModels;
using Newtonsoft.Json;
using MongoDB.Bson;
using CouchBaseChatApp.Helpers;

namespace CouchBaseChatApp.Database
{
    public class DbStorage
    {
        Couchbase.Lite.Database db;
       
        public DbStorage()
        {
            db = Manager.SharedInstance.GetDatabase("testdb");
        }

        #region saveitem
        public void SaveItem(MessageModel item)
        {
            try
            {
                Document doc;
                if (item.Id != null)
                {
                    doc = db.CreateDocument();
                    var jsonData = JsonConvert.SerializeObject(item);
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
                    doc.PutProperties(values);
                    doc.Id = item.Id;
                    Toast.MakeText(Application.Context, "Item saved", ToastLength.Short).Show();

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
                Toast.MakeText(Application.Context, "failed to save item", ToastLength.Short);
            }

        }
        #endregion

        #region updateItem  
        public void UpdateItem()
        {

        }
        #endregion

        #region DeleteItem
        public void DeleteItem(string ItemId)
        {
           var doc= db.GetExistingDocument(ItemId);
            doc.Delete();
        }
        #endregion

        #region GetItems
        public List<MessageModel> GetItems()
        {
           var qry= db.CreateAllDocumentsQuery();
           var res= qry.Run();
            List<MessageModel> list = new List<MessageModel>();
            foreach (var item in res)
            {
               // var x = item.Document.ToJson();
                var jsondata = item.Document.UserProperties.ToJson();
                var datamodel = JsonConvert.DeserializeObject<MessageModel>(jsondata);
                list.Add(datamodel);

            }
            return list;
        } 
        #endregion

    }
}

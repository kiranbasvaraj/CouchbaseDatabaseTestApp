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

namespace CouchBaseChatApp.Adapters
{
    public class MessagesAdapter : BaseAdapter<MessageModel>
    {
        List<MessageModel> ListItems;
        Activity _context;
        public MessagesAdapter(Activity context, List<MessageModel> items) : base()
        {
            this.ListItems = items;
            this._context = context;
        }
        public override MessageModel this[int position]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int Count
        {
            get
            {
                if (this.ListItems != null)

                    return ListItems.Count;
                else
                    return 0;
                
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            try
            {
                var item = ListItems[position];
                if (convertView == null)
                {
                    convertView = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                }
                if (item!=null)
                {
                    convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Message;
                }
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
            }
            return convertView;
        }
    }
}
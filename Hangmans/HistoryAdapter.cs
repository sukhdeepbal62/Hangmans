using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Hangmans.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangmans
{
    public class HistoryAdapter : BaseAdapter<User>
    {
        private Activity context;
        private List<User> users;

        public HistoryAdapter(Activity context, List<User> users)
        {
            this.users = users;
            this.context = context;
        }

        public override int Count
        {
            get { return users.Count; }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override User this[int position]
        {
            get { return users[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.list_row, null, false);
            }

            TextView textUserName = row.FindViewById<TextView>(Resource.Id.textUserName);
            TextView textWon = row.FindViewById<TextView>(Resource.Id.textWon);
            TextView textLost = row.FindViewById<TextView>(Resource.Id.textLost);

            textUserName.Text = users[position].UserName;
            textWon.Text = " Won : " + users[position].TotalWon;
            textLost.Text = " Lost: " + users[position].TotalLost;

            return row;
        }
    }
}
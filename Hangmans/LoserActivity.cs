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
    [Activity(Label = "LoserActivity")]
    public class LoserActivity : Activity
    {
        ListView list;
        Button btnBack;
        DataConnection connection;
        HistoryAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_history);

            connection = new DataConnection(this);

            list = FindViewById<ListView>(Resource.Id.list);
            btnBack = FindViewById<Button>(Resource.Id.backmain);

            btnBack.Click += BtnBack_Click;

            List<User> users = connection.GetLosers();
            adapter = new HistoryAdapter(this, users);
            list.Adapter = adapter;
            Toast.MakeText(this, "Total: " + users.Count(), ToastLength.Long).Show();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}
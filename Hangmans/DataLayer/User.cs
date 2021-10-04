using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hangmans.DataLayer
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }

        [Unique]
        public string UserName { get; set; }

        public string Password { get; set; }

        public int TotalWon { get; set; }

        public int TotalLost { get; set; }
    }
}
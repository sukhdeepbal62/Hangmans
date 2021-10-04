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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private Button login, register;
        private EditText etname, etpassword;
        private DataConnection connection;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            connection = new DataConnection(this);

            etname = FindViewById<EditText>(Resource.Id.etName);
            etpassword = FindViewById<EditText>(Resource.Id.etPassword);
            login = FindViewById<Button>(Resource.Id.loginbutton);
            register = FindViewById<Button>(Resource.Id.registerbutton);

            login.Click += Login_Click;
            register.Click += Register_Click;
        }

        private void Register_Click(object sender, EventArgs e)
        {
            string name = etname.Text.Trim();
            string password = etpassword.Text;
            string message = "";
            if (name.Length == 0 || password.Length == 0)
            {
                message = "Please Fill All Boxes";
            }
            else
            {
                User user = new User();
                user.UserName = name;
                user.Password = password;
                if (connection.SaveUser(user))
                {
                    message = "User Details are saved";
                    Intent intent = new Intent(this, typeof(GamePlayActivity));
                    intent.PutExtra("username", name);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    message = "User Details are not Saved Due To " + connection.GetMessage();
                }
            }
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string name = etname.Text.Trim();
            string password = etpassword.Text;
            string message = "";
            if (name.Length == 0 || password.Length == 0)
            {
                message = "Please Fill All Boxes";
            }
            else
            {
                if (connection.CheckUser(name, password))
                {
                    message = "Welcome To Hangman Game";
                    Intent intent = new Intent(this, typeof(GamePlayActivity));
                    intent.PutExtra("username", name);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    message = "Invalid User Name and Password Given";
                }

            }
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }
    }
}
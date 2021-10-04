using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;

namespace Hangmans
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar.FullScreen", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button aboutButton = FindViewById<Button>(Resource.Id.aboutbutton);
            Button instructionButton = FindViewById<Button>(Resource.Id.instructionbutton);
            Button gameButton = FindViewById<Button>(Resource.Id.newgamebutton);
            Button winnerButton = FindViewById<Button>(Resource.Id.historybutton1);
            Button loserButton = FindViewById<Button>(Resource.Id.historybutton2);

            aboutButton.Click += AboutButton_Click;
            instructionButton.Click += InstructionButton_Click;
            gameButton.Click += GameButton_Click;
            winnerButton.Click += WinnerButton_Click;
            loserButton.Click += LoserButton_Click;
        }

        private void LoserButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoserActivity));
            StartActivity(intent);
        }

        private void WinnerButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(WinnerActivity));
            StartActivity(intent);
        }

        private void GameButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void InstructionButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(InstructionActivity));
            StartActivity(intent);
        }

        private void AboutButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AboutActivity));
            StartActivity(intent);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
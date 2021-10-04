using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Hangmans.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hangmans
{
    [Activity(Label = "GamePlayActivity")]
    public class GamePlayActivity : AppCompatActivity
    {
        private DataConnection connection;
        private static Dictionary<int, char> KEYBOARD;
        private string initial_status = "GIVE ME YOUR BEST GUESS";
        private string username;
              
        private string toast_text = "The game is not over yet. You can search for the word once the game is over.";
        
        private TextView statusBox, dashBox, guessLeftBox;
        private EvilHangman evilHangman;
        private Button buttonplay, buttonback;
        static GamePlayActivity()
        {
            KEYBOARD = new Dictionary<int, char>();
            KEYBOARD.Add(Resource.Id.button1, 'a');
            KEYBOARD.Add(Resource.Id.button2, 'b');
            KEYBOARD.Add(Resource.Id.button3, 'c');
            KEYBOARD.Add(Resource.Id.button4, 'd');
            KEYBOARD.Add(Resource.Id.button5, 'e');
            KEYBOARD.Add(Resource.Id.button6, 'f');
            KEYBOARD.Add(Resource.Id.button7, 'g');
            KEYBOARD.Add(Resource.Id.button8, 'h');
            KEYBOARD.Add(Resource.Id.button9, 'i');
            KEYBOARD.Add(Resource.Id.button10, 'j');
            KEYBOARD.Add(Resource.Id.button11, 'k');
            KEYBOARD.Add(Resource.Id.button12, 'l');
            KEYBOARD.Add(Resource.Id.button13, 'm');
            KEYBOARD.Add(Resource.Id.button14, 'n');
            KEYBOARD.Add(Resource.Id.button15, 'o');
            KEYBOARD.Add(Resource.Id.button16, 'p');
            KEYBOARD.Add(Resource.Id.button17, 'q');
            KEYBOARD.Add(Resource.Id.button18, 'r');
            KEYBOARD.Add(Resource.Id.button19, 's');
            KEYBOARD.Add(Resource.Id.button20, 't');
            KEYBOARD.Add(Resource.Id.button21, 'u');
            KEYBOARD.Add(Resource.Id.button22, 'v');
            KEYBOARD.Add(Resource.Id.button23, 'w');
            KEYBOARD.Add(Resource.Id.button24, 'x');
            KEYBOARD.Add(Resource.Id.button25, 'y');
            KEYBOARD.Add(Resource.Id.button26, 'z');
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_gameplay);

            username = Intent.GetStringExtra("username");

            connection = new DataConnection(this);

            evilHangman = new EvilHangman(connection.GetAllGameWord(), 5, 15);

            statusBox = FindViewById<TextView>(Resource.Id.statusbox);
            dashBox = FindViewById<TextView>(Resource.Id.dashbox);
            guessLeftBox = FindViewById<TextView>(Resource.Id.guessleftbox);
            buttonback = FindViewById<Button>(Resource.Id.back);
            buttonplay = FindViewById<Button>(Resource.Id.play_again);

            buttonback.Click += Buttonback_Click;
            buttonplay.Click += Buttonplay_Click;

            buttonplay.Visibility = ViewStates.Gone;
            buttonback.Visibility = ViewStates.Gone;

            statusBox.Text = initial_status;
            dashBox.Text = evilHangman.SecretWord;
            guessLeftBox.Text = "Guesses Left: " + evilHangman.GuessesLeft;

            foreach(int id in KEYBOARD.Keys)
            {
                Button button = FindViewById<Button>(id);
                button.Enabled = true;
                button.Text = KEYBOARD[id].ToString();
                button.SetBackgroundColor(Resources.GetColor(Resource.Color.Black));
                button.Click += Button_Click;

            }

        }

        private void Buttonplay_Click(object sender, EventArgs e)
        {
            statusBox.Text = initial_status;
            evilHangman.SecretWord = evilHangman.GenerateNewWord();
            dashBox.Text = evilHangman.SecretWord;
            evilHangman.GuessesLeft = 15;
            guessLeftBox.Text = "Guesses Left: " + evilHangman.GuessesLeft;

            foreach (int id in KEYBOARD.Keys)
            {
                Button button = FindViewById<Button>(id);
                button.Enabled = true;
                button.Text = KEYBOARD[id].ToString();
                button.SetBackgroundColor(Resources.GetColor(Resource.Color.Black));
            }
            buttonplay.Visibility = ViewStates.Gone;
            buttonback.Visibility = ViewStates.Gone;
        }

        private void Buttonback_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button button = sender as Button;
                if (KEYBOARD.Keys.Contains(button.Id))
                {
                    button.Text = "";
                    button.Enabled = false;
                    char guessedLetter = KEYBOARD[button.Id];
                    UpdateGame(guessedLetter);

                }
            }
            
        }

        public void UpdateGame(char guessedLetter)
        {
            bool result = evilHangman.GuessMade(guessedLetter);
            string status, secretWord = evilHangman.SecretWord;
            dashBox.Text = secretWord;
            guessLeftBox.Text = "Guesses Left: " + evilHangman.GuessesLeft;

            if (result)
            {
                status = "YOU ARE BANG ON RIGHT";
                statusBox.Text = status;
                statusBox.SetTextColor(Resources.GetColor(Resource.Color.Green));

                if (IsUserWon())
                {
                    status = "YOU WON SIR! TAKE A BOW!";
                    foreach (int id in KEYBOARD.Keys)
                    {
                        Button myButton = FindViewById<Button>(id);
                        myButton.Enabled = false;
                    }
                    statusBox.Text = status;
                    statusBox.SetTextColor(Resources.GetColor(Resource.Color.Green));
                    if(connection.UpdateProfile(username, true))
                    {
                        Toast.MakeText(this, "WIN " + connection.GetMessage(), ToastLength.Long).Show();
                    }
                    buttonplay.Visibility = ViewStates.Visible;
                    buttonback.Visibility = ViewStates.Visible;
                }
            }
            else
            {
                status = "YOU CAN'T BEAT ME BUDDY";
                 statusBox.Text = status;
                statusBox.SetTextColor(Resources.GetColor(Resource.Color.Red));

                if (evilHangman.GuessesLeft == 0)
                {
                    status = "GAME OVER";
                    foreach (int id in KEYBOARD.Keys )
                    {
                        Button myButton = FindViewById<Button>(id);
                        myButton.Enabled = false;
                    }
                    if(connection.UpdateProfile(username, false))
                    {
                        Toast.MakeText(this, "LOSE: " + connection.GetMessage(), ToastLength.Long).Show();
                    }
                    dashBox.Text = "The word was: " + evilHangman.PrepareAnswer();
                    buttonplay.Visibility = ViewStates.Visible;
                    buttonback.Visibility = ViewStates.Visible;
                }
                statusBox.Text=status;
            }
        }

        public bool IsUserWon()
        {
            if (evilHangman.GuessesLeft == 0) return false;
            string result = RemoveSpaces(evilHangman.SecretWord);
            if (evilHangman.IsRight(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string RemoveSpaces(string secretWord)
        {
            string result = "";
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (secretWord[i] != ' ')
                    result += secretWord[i];
            }
            return result;
        }
    
    }
}
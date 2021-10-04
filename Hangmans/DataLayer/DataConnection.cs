using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hangmans.DataLayer
{
    public class DataConnection
    {
        private SQLiteConnection conn;

        private string message;

        public string GetMessage()
        {
            return message;
        }

        public DataConnection(Context context)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            conn = new SQLiteConnection(Path.Combine(path, "project.db"));
            if (!CheckTable())
            {
                conn.CreateTable<User>();
                conn.CreateTable<GameWord>();
                try
                {
                    StreamReader br = new StreamReader(context.Assets.Open("dictionary.txt"));
                    string line;
                    int count = 0;
                    while ((line = br.ReadLine()) != null)
                    {
                        if (line.Length >= 5 )
                        {
                            count++;
                            GameWord word = new GameWord();
                            word.Word = line.Trim().ToLower();
                            SaveGameWord(word);
                        }
                        if(count == 500)
                        {
                            break;
                        }
                    }
                }
                catch (IOException ex)
                {

                }
            }

        }

        public List<string> GetAllGameWord()
        {
            List<string> wordStrings = new List<string>();
            List<GameWord> words = GetAllWords();
            foreach (GameWord word in words)
            {
                wordStrings.Add(word.Word);
            }
            return wordStrings;
        }


        public bool SaveGameWord(GameWord word)
        {
            try
            {
                conn.Insert(word);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool CheckUser(string name, string password)
        {
            List<User> users = conn.Query<User>("Select * from User");
            foreach (User user in users)
            {
                if (user.UserName.Equals(name) && user.Password.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateProfile(string name, bool winning)
        {
            try
            {
                var users = conn.Table<User>();
                var user = (from pro in users
                               where pro.UserName == name
                               select pro).Single();
                if (winning)
                {
                    user.TotalWon += 1;
                }
                else
                {
                    user.TotalLost += 1;
                }
                conn.Update(user);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool SaveUser(User user)
        {
            try
            {
                conn.Insert(user);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public List<GameWord> GetAllWords()
        {
            List<GameWord> words = conn.Query<GameWord>("Select * from GameWord");
            return words;
        }

        public List<User> GetLosers()
        {
            List<User> users = conn.Query<User>("Select * from User Order by TotalLost  Desc");
            return users;
        }

        public List<User> GetWinners()
        {
            List<User> users = conn.Query<User>("Select * from User Order by TotalWon Desc");
            return users;
        }

        private bool CheckTable()
        {
            try
            {
                conn.Get<GameWord>(1);
                conn.Get<User>(1);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
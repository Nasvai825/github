using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Fitnes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fitnes.user
{
    public partial class MainUser : TabbedPage
    {
        List<string> userData = new List<string>();
        bool upAbon = false;
        public MainUser(List<string> userData2)
        {
            InitializeComponent();

            nameAbon.Items.Add("Стандарт");
            nameAbon.Items.Add("Расширенный");
            nameAbon.Items.Add("VIP");
            nameAbon.Items.Add("VIP+");
            nameAbon.Items.Add("Премиум");
            nameAbon.Items.Add("Премиум+");

            imageAvatar.Source = ImageSource.FromResource("Fitnes.image.avatar.png");
            
            userData.Clear();
            userData.AddRange(userData2);
            Username.Text = userData[2];
            
            Update_Data();

        }

        protected override void OnAppearing()
        {
            string name = Preferences.Get("name", "не установлено");
            informLabel.Text = name;
            base.OnAppearing();
            informBox.IsVisible = false;
        }

        private void PicerButton_Clicked(object sender, EventArgs e)
        {
            int id = 0;
            DateTime dateTime = new DateTime();
            dateTime = DateTime.UtcNow;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `karta`(`idKarta`, `start`, `end`, `abonement_idAbonement`)" +
                " VALUES (default, @start, @end, @abon)", db.getConnection());
            command.Parameters.Add("@start", MySqlDbType.Date).Value = dateTime;
            command.Parameters.Add("@end", MySqlDbType.Date).Value = dateTime.AddMonths(1);
            command.Parameters.Add("@abon", MySqlDbType.Int32).Value = nameAbon.SelectedIndex + 1;
            command.ExecuteNonQuery();

            MySqlCommand command2 = new MySqlCommand("SELECT max(`idKarta`) FROM `karta` ", db.getConnection());
            MySqlDataReader reader = command2.ExecuteReader();

            if (reader.HasRows) 
            {
                while (reader.Read()) 
                {
                    id = Convert.ToInt32(reader[0]);
                }
            }
            reader.Close();
            MySqlCommand command3 = new MySqlCommand("UPDATE `klient` SET `karta_idKarta`=@id WHERE `idKlient`=@idUser2", db.getConnection());
            command3.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command3.Parameters.Add("@idUser2", MySqlDbType.Int32).Value = Convert.ToInt32(userData[0]);
            command3.ExecuteNonQuery();
            DisplayAlert("Успех", "Вы оформили абонемент на месяц", "Ок");
            
            userData[3] = (nameAbon.SelectedIndex + 1).ToString();
            upAbon = true;
            db.closeConnection();
            Update_Data();
        }

            private void Update_Clicked(object sender, EventArgs e)
        {
            informBox.IsVisible = false;
            string value = informBox.Text;
            informLabel.Text = value;
            Preferences.Set("name", value);
            informBox.IsVisible = true;

        }
        private void Update_Data()
        {
            phoneUser.Text = userData[1];
         
            if (userData[3] == "" && upAbon == false)
            {
                nameAbonement.Text = "У вас нет абонемента";
                dateStart.Text = "-";
                dateEnd.Text = "-";
                levelAbonement.Text = "-";
                priceAbonement.Text = "-";
                DisplayAlert("v", "dvvsdvdv", "advasd");
            }
            else
            {
                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT `start`,`end`,`nameAbonement`,`abonementLevel`,`abonementPrice` FROM `klient`,`karta`,`abonement` WHERE `karta_idKarta`=`idKarta` and `abonement_idAbonement`=`idAbonement` and `karta_idKarta`=@id", db.getConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(userData[3]);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // это строки?
                {
                    while (reader.Read()) // забираю данные из бд
                    {
                        dateStart.Text = reader[0].ToString();
                        dateEnd.Text = reader[1].ToString();
                        nameAbonement.Text = reader[2].ToString();
                        levelAbonement.Text = reader[3].ToString();
                        priceAbonement.Text = reader[4].ToString();
                    }
                }
                db.closeConnection();
               
            }
        }



    }
}
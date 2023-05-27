using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Fitnes;
using Xamarin.Forms;

namespace Fitnes
{
    public partial class AvtorizAdmin : ContentPage
    {
        public AvtorizAdmin()
        {
            InitializeComponent();
            image1.Source = ImageSource.FromResource("Fitnes.image.logo.png");
            back.Source = ImageSource.FromResource("Fitnes.image.arrow.png");
        }

        private async void button1_Clicked(object sender, EventArgs e)
        {
            string fioUser = FIO.Text;
            string phoneUser = Phone.Text;
            string passwordUser = Password.Text;
            string passwordUser2 = Password2.Text;

            if (fioUser == "" || phoneUser == "" || passwordUser == "" || passwordUser2 == "")
            {
                await DisplayAlert("Ошибка", "Заполнены не все поля", "OK");
            }
            else
            {
                if (isUserExist() == false)
                {
                   // DateTime dateTime = new DateTime();
                   // dateTime = DateTime.UtcNow;


                    if (passwordUser == passwordUser2)
                    {
                        DB db = new DB();
                        db.openConnection();
                        MySqlCommand command = new MySqlCommand("INSERT INTO `klient`(`idKlient`,`FIOKlient`, `phoneKlient`, `pass_klient`,`karta_idKarta`)" +
                            "values (default, @fio, @phone, @password, null)", db.getConnection());
                        command.Parameters.Add("@fio", MySqlDbType.VarChar).Value = fioUser;
                        command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phoneUser;
                       // command.Parameters.Add("@start", MySqlDbType.DateTime).Value = dateTime;
                       // command.Parameters.Add("@end", MySqlDbType.DateTime).Value = dateTime.AddMonths(1);
                        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = passwordUser;

                        if (command.ExecuteNonQuery() == 1)
                        {
                            await DisplayAlert("Отлично", "На другое окно", "OK");
                            db.closeConnection();
                            await Navigation.PopAsync();
                        }
                        else await DisplayAlert("Ошибка", "Не верные данные", "OK");
                        db.closeConnection();
                    }
                    else await DisplayAlert("Ошибка", "Не совпадают пароли", "OK");
                }
            }

        }

        public Boolean isUserExist()
        {
            string phoneUser = Phone.Text;


            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `klient` WHERE `phoneKlient`=@phone", db.getConnection());
            command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phoneUser;
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                DisplayAlert("Ошибка", "Пользователь с этим номером уже зарегистрирован", "OK");
                return true;
            }
            else return false;
        }

        public string GetHashMD5(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

using Fitnes.admin;
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
    public partial class RegUser : ContentPage
    {
        string post;
        public RegUser()
        {
            InitializeComponent();

            image1.Source = ImageSource.FromResource("Fitnes.image.logo.png");
            back.Source = ImageSource.FromResource("Fitnes.image.arrow.png");
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void vhod_Clicked(object sender, EventArgs e)
        {
            string phoneAdmin = nameInput1.Text;
            string passwordAdmin = nameInput2.Text;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `sotrudnic` WHERE `phoneSotrudnic`=@phoneAdmin AND `password`=@passwordAdmin", db.getConnection());
            command.Parameters.Add("@phoneAdmin", MySqlDbType.VarChar).Value = phoneAdmin;
            command.Parameters.Add("@passwordAdmin", MySqlDbType.VarChar).Value = passwordAdmin;

            MySqlDataReader reader = command.ExecuteReader();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    post = reader[5].ToString();
                }
                await Navigation.PushAsync(new MainAdmin(post));
            }
            else
            {
                await DisplayAlert("Ошибка!", "Не правильный логин или пароль", "OK");
            }
            db.closeConnection();
        }
    }
}

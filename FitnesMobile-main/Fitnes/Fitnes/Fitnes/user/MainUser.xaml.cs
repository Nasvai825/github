using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaxoparkMobile;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fitnes.user
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainUser : TabbedPage
    {
        List<string> userData = new List<string>();
        public MainUser(List<string> userData2)
        {
            InitializeComponent();

            imageAvatar.Source = ImageSource.FromResource("Fitnes.image.avatar.png");
            ////.Source = ImageSource.FromResource("Fitnes.image.logo.png");
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
            if (userData[3] == "")
            {
                nameAbonement.Text = "У вас нет абонемента";
                dateStart.Text = "-";
                dateEnd.Text = "-";
                levelAbonement.Text = "-";
                priceAbonement.Text = "-";
            }
            else
            {
                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT `start`,`end`,`nameAbonement`,`abonementLevel`,`abonementPrice` FROM `karta`,`abonement` WHERE `abonement_idAbonement`=`idAbonement` and `idKarta`=@id", db.getConnection());
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
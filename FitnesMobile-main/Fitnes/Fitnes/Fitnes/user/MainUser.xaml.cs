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
        string sql;
        int id = 0;
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
            LoadData();

            LoadNews();
        }

        private void LoadNews()
        {
            News.Children.Clear(); 

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `NameNews`,`TextNews` FROM `news` ", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Grid grid = new Grid
                    {
                        MinimumHeightRequest = 100,
                        BackgroundColor = Color.LightGoldenrodYellow,
                        Margin = new Thickness(15, 10, 15, 0),
                    };


                    Label label = new Label
                    {
                        FontSize = 20,
                        MinimumHeightRequest = 40,
                        Margin = new Thickness(25, 5, 15, 15),
                        Text = reader[0].ToString()
                    };
                    label.VerticalTextAlignment = TextAlignment.Center;
                    label.HorizontalTextAlignment = TextAlignment.Start;
                    label.VerticalOptions = LayoutOptions.Start;
                    label.FontFamily = "Bolt";
                    //BoxView box = new BoxView
                    //{
                    //    HeightRequest = 4,
                    //    BackgroundColor = Color.Black,
                    //    Margin = new Thickness(20, 30, 20, 30),
                    //};

                    Label label2 = new Label
                    {
                        FontSize = 16,
                        MinimumHeightRequest = 50,
                        Margin = new Thickness(15, 45, 15, 15),
                        Text = reader[1].ToString()
                    };
                    label2.VerticalTextAlignment = TextAlignment.Center;
                    label2.HorizontalTextAlignment = TextAlignment.Start;
                    label2.VerticalOptions = LayoutOptions.End;

                    grid.Children.Add(label);
                    //grid.Children.Add(box);
                    grid.Children.Add(label2);
                    News.Children.Add(grid);
                }
            }
        }

        private void Full_Schedule_User(object sender, EventArgs e)
        {

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
            userData[3] = (nameAbon.SelectedIndex + 1).ToString();
            upAbon = true;
            DisplayAlert("Успех", "Вы оформили абонемент на месяц", "Ок");
            
            
            
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
         
              
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `start`,`end`,`nameAbonement`,`abonementLevel`,`abonementPrice` FROM `klient`,`karta`,`abonement` WHERE  `abonement_idAbonement`=`idAbonement` and `karta_idKarta`=`idKarta` and `idKlient`=@idklient", db.getConnection());
                
            command.Parameters.Add("@idklient", MySqlDbType.Int32).Value = Convert.ToInt32(userData[0]);
                

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
            else
            {
                nameAbonement.Text = "У вас нет абонемента";
                dateStart.Text = "-";
                dateEnd.Text = "-";
                levelAbonement.Text = "-";
                priceAbonement.Text = "-";
            }
            reader.Close();
            db.closeConnection();
               
        }

        

        private void LoadData()
        {
            callView.Children.Clear();
            NameColumn.Children.Clear();
            int j = 0;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT idRaspisanie, Date, Time,nameUslugi FROM raspisanie,uslugi WHERE uslugi_idUslugi=idUslugi", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                Grid grid2 = new Grid
                {
                    HeightRequest = 45,
                    RowSpacing = -45,
                    ColumnSpacing = 4,
                    BackgroundColor = Color.Black,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    }
                };
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string textcall2 = reader.GetName(i).ToString();
                    Label label2 = new Label
                    {
                        FontSize = 12,
                        Text = textcall2,
                    };
                    label2.VerticalTextAlignment = TextAlignment.Center;
                    label2.HorizontalTextAlignment = TextAlignment.Center;
                    label2.TextColor = Color.Black;
                    label2.BackgroundColor = Color.White;
                    grid2.Children.Add(label2, i, 0);
                }
                BoxView box2 = new BoxView
                {
                    Margin = new Thickness(0, -7, 0, 0),
                    HeightRequest = 4,
                };
                box2.BackgroundColor = Color.Black;
                NameColumn.Children.Add(grid2);
                NameColumn.Children.Add(box2);

                while (reader.Read())
                {

                    Grid grid = new Grid
                    {
                        HeightRequest = 45,
                        RowSpacing = -45,
                        ColumnSpacing = 4,
                        BackgroundColor = Color.Black,
                        RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        }
                    };
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string textcall = reader[i].ToString();
                        if (textcall == "")
                        {
                            textcall = "Нет";
                        }
                        Label label1 = new Label
                        {
                            FontSize = 12,
                            Text = textcall,
                        };
                        label1.VerticalTextAlignment = TextAlignment.Center;
                        label1.HorizontalTextAlignment = TextAlignment.Center;
                        label1.TextColor = Color.Black;
                        label1.BackgroundColor = Color.LightGray;
                        grid.Children.Add(label1, i, j);
                    }
                    j++;
                    BoxView box = new BoxView
                    {
                        Margin = -5,
                        HeightRequest = 2
                    };
                    box.BackgroundColor = Color.Black;
                    callView.Children.Add(grid);
                    callView.Children.Add(box);
                }
            }
            db.closeConnection();
        }



    }
}
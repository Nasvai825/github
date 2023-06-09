﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitnes;
using Xamarin.Forms;

namespace Fitnes.admin
{
    public partial class MainAdmin : TabbedPage
    {
        string sql;
        string post;
            
       
        public MainAdmin(string post2)
        {
            InitializeComponent();

            post = post2;
            labelPost.Text = post;

            LoadDataInTableSchedule();
            LoadDataInTableClient();

            InsertDataInPicker();
            Picker_idUslugiInsert();
        }

        private void Picker_idUslugiInsert()
        {
            Picker_idUslugi.IsVisible = true;
            Picker_Date.IsVisible = true;
            Picker_Time.IsVisible = true;
            QuestionTime_Picker.IsVisible = true;
            QuestionDate_Picker.IsVisible = true;
            Picker_idUslugi.Items.Clear();


            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `idUslugi`,`nameUslugi` FROM `uslugi` ", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string usluga = reader[0].ToString() + "   " + reader[1].ToString();
                    Picker_idUslugi.Items.Add(usluga);
                }
            }
            db.closeConnection();
        }

        private void buttonVisits_Clicked(object sender, EventArgs e) 
        {
            int idVisiter = Convert.ToInt32(Picker_idKarta.SelectedItem);

            DateTime dateTime = new DateTime();
            dateTime = DateTime.Now;

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `visits`(`idVisits`, `dateTime`, `id_klient`) VALUES (default,@dateTime,@idVisiter)", db.getConnection());
            command.Parameters.Add("@dateTime", MySqlDbType.DateTime).Value = dateTime;
            command.Parameters.Add("@idVisiter", MySqlDbType.Int32).Value = idVisiter;
            command.ExecuteNonQuery();
            DisplayAlert("Ок", "Чето там заработало", "ок");
        }

        private void Insert_Schedule(object sender, EventArgs e)
        {
            
        }
        private void NewsAdd_Clicked(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `news`(`idNews`, `NameNews`, `TextNews`) VALUES (default,@nameNews,@textNews)", db.getConnection());
            command.Parameters.Add("@nameNews", MySqlDbType.VarChar).Value = nameInputNews.Text.ToString();
            command.Parameters.Add("@textNews", MySqlDbType.VarChar).Value = InputNewsText.Text.ToString();
            command.ExecuteNonQuery();
            DisplayAlert("Отлично", "Вы успешно добавили новость", "ок");
        }

        private void InsertDataInPicker()
        {
            Picker_idKarta.Items.Clear();

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `idKlient` FROM `klient` ", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Picker_idKarta.Items.Add(reader[0].ToString());
                }
            }
            db.closeConnection();
        }
        private void buttonInsertData_Clicked(object sender, EventArgs e)
        {
            buttonUpdateData.IsVisible = true;
           
            
            string idusl = "";
            string usluga = Picker_idUslugi.SelectedItem.ToString();
            for (int i = 0; i < 3; i++)
            {
                if (usluga[i].ToString() == " ")
                {
                    break;
                }
                else
                    idusl += usluga[i].ToString();
            }
            
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `raspisanie`(`idRaspisanie`, `Date`, `Time`,`uslugi_idUslugi`) VALUES (default, @Date, @Time, @iduslugi)  ", db.getConnection());
            command.Parameters.Add("@Time", MySqlDbType.Time).Value = Picker_Time.Time;
            command.Parameters.Add("@Date", MySqlDbType.Date).Value = Picker_Date.Date;
            command.Parameters.Add("@iduslugi", MySqlDbType.Int32).Value = Convert.ToInt32(idusl);
            //Specified cast is not valid.command.ExecuteNonQuery();
            command.ExecuteNonQuery();
            LoadDataInTableSchedule();
            DisplayAlert("Отлично", "Вы добавили новый ", "Ок");
        }
        private void buttonUpdateData_Clicked(object sender, EventArgs e)
        {
            int q = Convert.ToInt32(Picker_idRaspisanie.SelectedItem);

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE raspisanie SET Time=@Time, Date =@Date WHERE idRaspisanie=@idRasp", db.getConnection());
            command.Parameters.Add("@Time", MySqlDbType.Time).Value = Picker_Time.Time;
            command.Parameters.Add("@Date", MySqlDbType.Date).Value = Picker_Date.Date;
            command.Parameters.Add("@idRasp", MySqlDbType.Int32).Value = Convert.ToInt32(Picker_idRaspisanie.SelectedItem);
            //Specified cast is not valid.command.ExecuteNonQuery();
            command.ExecuteNonQuery();
            LoadDataInTableSchedule();
            DisplayAlert("Отлично", "Время изменено", "ок");
        }

        private void InsertDataInPickerSchedule(object sender, EventArgs e)
        {
            QuestionLable.IsVisible = false;
            Picker_idUslugi.IsVisible = false;
            СategoryLable.IsVisible = true;
            Picker_idRaspisanie.IsVisible = true;
            buttonUpdateData.IsVisible = true;
            Picker_Date.IsVisible = true;
            Picker_Time.IsVisible = true;
            QuestionTime_Picker.IsVisible = true;
            QuestionDate_Picker.IsVisible = true;

            Picker_idRaspisanie.Items.Clear();

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `idRaspisanie` FROM `raspisanie` ", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Picker_idRaspisanie.Items.Add(reader[0].ToString());
                }
            }
            db.closeConnection();
        }



        private void LoadDataInTableSchedule()
        {
           
            callView2.Children.Clear();
            NameColumn2.Children.Clear();

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
                    HeightRequest = 4,
                    Margin = new Thickness(0, -7, 0, 0),
                };
                box2.BackgroundColor = Color.Black;

                  
                    NameColumn2.Children.Add(grid2);
                    NameColumn2.Children.Add(box2);  

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
                        HeightRequest = 2,
                        
                    };
                    box.BackgroundColor = Color.Black;

                    callView2.Children.Add(grid);
                    callView2.Children.Add(box);
                }
            }
            db.closeConnection();
        }


        private void LoadDataInTableClient()
        {
            callView.Children.Clear();
            NameColumn.Children.Clear();

            int j = 0;

            DB db = new DB();
            db.openConnection();
            if (post == "Администратор")
            {
                sql = "SELECT karta.idKarta, klient.FIOKlient, abonement.NameAbonement FROM karta JOIN klient, abonement limit 30";

            }
            if (post == "Тренер")
            {
                sql = "SELECT `FIOKlient`,`idKarta`,`NameAbonement` FROM `klient`,`karta`,`abonement` WHERE `karta_idKarta`=`idKarta` and `abonement_idAbonement`=`idAbonement` and `abonement_idAbonement` > 0";
            }
            MySqlCommand command = new MySqlCommand(sql, db.getConnection());
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
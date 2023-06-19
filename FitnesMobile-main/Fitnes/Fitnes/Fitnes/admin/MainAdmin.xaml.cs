using MySqlConnector;
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
        string post = "";
        public MainAdmin(List<string> adminData)
        {
            InitializeComponent();

            Сategory.Items.Add("Массаж");
            Сategory.Items.Add("Солярий");
            Сategory.Items.Add("Персональная тренеровка");
            Сategory.Items.Add("Аэробика");
            Сategory.Items.Add("Кардио");
        }
        private void Visits_Ok(object sender, EventArgs e)
        {
            labelPost.Text = post;
            
                if (post == "Администратор")
                {
                    sql = "";
                    LoadData();

                }
                if (post == "Тренер")
                {
                    sql = "";
                    LoadData();

                }
                if (post == "Массажист")
                {
                    sql = "";
                    LoadData();

                }
                else
                {
                     DisplayAlert("Ошибка", "Поле не заполнено", "OK");
                }

        }
        private void buttonVisits_Clicked(object sender, EventArgs e) 
        {

        }
        private void Update_Schedule(object sender, EventArgs e)
        {
            string updateCategory = nameInput3.Text;
            СategoryLable.IsVisible = true;
            Сategory.IsVisible = true;
            

        }
        private void Insert_Schedule(object sender, EventArgs e)
        {
            
        }
    
        private void Full_Schedule(object sender, EventArgs e)
        {
            sql = " SELECT uslugi.nameUslugi, raspisanie.Date, raspisanie.Time FROM uslugi JOIN raspisanie ON `idRaspisanie` >= `uslugi_idUslugi`";
            LoadData();
        }

        private void LoadData()
        {
            callView.Children.Clear();
            NameColumn.Children.Clear();
            int j = 0;

            DB db = new DB();
            db.openConnection();
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
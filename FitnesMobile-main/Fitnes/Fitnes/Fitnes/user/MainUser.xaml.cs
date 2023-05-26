using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fitnes.user
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainUser : TabbedPage
    {
        public MainUser()
        {
            InitializeComponent();

            imageAvatar.Source = ImageSource.FromResource("Fitnes.image.avatar.png");
        }
    }
}
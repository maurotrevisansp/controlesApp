using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Model;
using Xamarin.Forms;

namespace ControleApp.Util
{
    public class Session
    {
        public static NavigationPage Navigation { get; set; }
        public static Usuario Usuario { get; set; }
        public static String UltOpMenu { get; set; }
        public static String UltOpMenu1 { get; set; }
        public static Boolean RetornaMenu { get; set; }
        public static String MinhaDelegada { get; set; }
    }
}

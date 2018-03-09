using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Model;
using SQLite;
using Xamarin.Forms;

namespace ControleApp.DAO
{
    internal class Contexto
    {
        internal SQLiteConnection database;
        internal Contexto()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();


            database.CreateTable<Usuario>();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Model;

namespace ControleApp.DAO
{
    public class UsuarioDAO
    {
        public static void InserirUsuario(Usuario u)
        {
            try
            {
                var context = new Contexto();
                if (context.database.Table<Usuario>().Any())
                {
                    context.database.DeleteAll<Usuario>();
                }
                    context.database.Insert(u);
                context.database.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Usuario Obter()
        {
            try
            {
                var context = new Contexto();
                return context.database.Table<Usuario>().FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void Remover()
        {
            try
            {
                var context = new Contexto();
                context.database.DeleteAll<Usuario>();
            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }
}

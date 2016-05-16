﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public class ValidaFuncoesUsuario
    {
        /// <summary>
        /// Classe que evita usuários não autorizados acessarem ações por meio de URL
        /// </summary>
        /// <param name="usuario">Usuário logado</param>
        /// <param name="NomeController">Controller que se deseja proteger</param>
        /// <param name="NomeAction">Action que se deseja proteger</param>
        /// <returns></returns>
        public bool PermiteAcesso(Usuario usuario, string NomeController, string NomeAction)
        {
            if(usuario.TipoUsuario == "VENDEDOR")
            {
                if(NomeController.ToUpper() == "USUARIO")
                {
                    // Vendedor não acessa ações do usuário
                    return false;
                }
            }

            if(usuario.TipoUsuario == "ADMINISTRADOR")
            {
                if(NomeController.ToUpper() == "USUARIO")
                {
                    // Administrador não acessa ações do usuário
                    return false;
                }
            }

            return true;
        }
    }
}
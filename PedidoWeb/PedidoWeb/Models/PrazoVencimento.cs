﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class PrazoVencimento
    {
        [Key]
        public int PrazoVencimentoID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        public int CodPrazoVencimento { get; set; }

        
        private string codEmpresa;

        public string Situacao { get; set; }


        // Properties

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }

        // Lazy Load
        public virtual Empresa Empresa { get; set; }
    }
}
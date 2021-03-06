﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Perc. Max. Desconto")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true, NullDisplayText = "Informar Valor")]
        public decimal PercDescontoMaximo { get; set; }

        [DisplayName("Un")]
        public string UnidadeMedida { get; set; }

        [DisplayName("Preço")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal PrecoVarejo { get; set; }

        [DisplayName("Preço Atacado")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal PrecoAtacado { get; set; }

        [DisplayName("Custo")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal PrecoCusto { get; set; }

        [DisplayName("Estoque")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Estoque { get; set; }

        [DisplayName("Situação")]
        public string Situacao { get; set; }

        [DisplayName("Código Cemapa")]
        public int CodProduto { get; set; }
        
        private string codEmpresa;

        public float? PercIPI { get; set; }

        // Properties

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }

        [DisplayName("Tributação")]
        public int? TributacaoID { get; set; }

        public virtual Tributacao Tributacao { get; set; }

        //Lazy Load
        public virtual Empresa Empresa { get; set; }
        [DisplayName("Núm Fabricante")]
        public string NumFabricante { get; set; }
    }
}
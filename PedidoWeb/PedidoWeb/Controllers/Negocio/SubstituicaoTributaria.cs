﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public class SubstituicaoTributaria
    {
        private PedidoWebContext db = new PedidoWebContext();
        private bool CalculaICMSParaDescontar(Tributacao trib)
        {
            var retorno = false;
            
            List<string> listaCsosn = new List<string> { "201", "202", "203", "900" };
            var st = string.Empty;
            if(!string.IsNullOrEmpty(trib.DescSituacaoTrib))
                st = trib.DescSituacaoTrib;
            if ((st.Length > 3 && st.Substring(2, 2) == "30")
                || listaCsosn.Contains(trib.DescCSOSN))
                retorno = true;
            
            return retorno;    
        }

        private bool TemST(Tributacao trib)
        {
            List<string> ListaCst = new List<string> {"010", "030", "060", "070"};
            List<string> ListaCsosn = new List<string> {"201", "202", "203", "900"};

            return (ListaCst.Contains(trib.DescSituacaoTrib) || ListaCsosn.Contains(trib.DescCSOSN));                
        }

        /// <summary>
        /// Escolhe a tributação a ser utilizada considerando operações interestaduais
        /// </summary>
        /// <param name="cadastro"></param>
        /// <param name="produto"></param>
        /// <param name="filial"></param>
        /// <returns>Tributacao</returns>
        public Tributacao EscolheTributacao(Cadastro cadastro, Produto produto, Filial filial, Operacao operacao)
        {
            Tributacao trib = produto.Tributacao;

            if (filial.CodEstado != cadastro.CodEstado && cadastro.Estado.TributacaoID != null && 
                    cadastro.Estado.TributacaoID > 0)
                trib = cadastro.Estado.Tributacao;

            if (operacao.Tributacao != null || operacao.TributacaoID != null  || operacao.TributacaoID > 0)
            {
                if (operacao.Tributacao != null)
                    trib = operacao.Tributacao;
                else
                    if (operacao.TributacaoID > 0)
                        trib = db.Tributacaos.First(t => t.TributacaoID == operacao.TributacaoID);
            }

            return trib;
        }

        public double CalculaSubstituicaoTributaria(Cadastro cadastro
            , Produto produto, double valUnitario, double valDesconto
            , double quantidade, Filial filial, Operacao operacao)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.Current.User.Identity.Name);

            var valorTotal = (valUnitario - valDesconto) * quantidade;
            double? percAliqSubst = 0;
            double? baseAliqSubst = 0;
            double? baseIcms = 0;
            double? valIcms = 0;
            double valIcmsReduzido = 0;
            double valST = 0;

            Tributacao tributacao = EscolheTributacao(cadastro, produto, filial, operacao);

            if (tributacao == null)
                return 0.00;

            if(TemST(tributacao))
            {
                percAliqSubst = cadastro.Estado.PercAliqSubst;
                baseAliqSubst = cadastro.Estado.PercBaseSubst;

                // Caso tenha ST por produto
                ProdutoSubstTrib prodSubst = null;
                try
                {
                    prodSubst = db.ProdutoSubstTribs.First(p => p.CodProduto == produto.CodProduto
                        && p.CodEstado == cadastro.CodEstado
                        && p.CodFilial == filial.CodFilial
                        && p.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa);
                }
                catch(Exception)
                {

                }

                if(prodSubst != null)
                {
                    percAliqSubst = prodSubst.PercAliquota;
                    baseAliqSubst = prodSubst.PercTributado;
                }

                // Sobrescreve com o valor calculado para a base da ST
                // A partir de daqui, baseAliqSubst contém o valor da base de cálculo da ST
                baseAliqSubst = (valorTotal * baseAliqSubst / 100);                

                if(CalculaICMSParaDescontar(tributacao))
                {
                    if (pedidoHelper.BuscaEmpresa().Nome == "ZAPOLI")
                        valIcmsReduzido = (valorTotal * 0.12);
                }

                baseIcms = valorTotal * tributacao.PercTributado / 100;
                valIcms = baseIcms * tributacao.PercAliquota / 100;

                valST = Convert.ToDouble(baseAliqSubst * percAliqSubst / 100) - Convert.ToDouble(valIcms) - valIcmsReduzido;

                if(cadastro.RegimeTributario != null && cadastro.RegimeTributario != 3)
                {
                    valST = valST - Convert.ToDouble(valST * cadastro.Estado.PercReducaoIcmsSubst / 100);
                }

                if (valST > 0)
                    return valST;
                else
                    return 0.00;
            }
            
            return 0.00;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PedidoWeb.Models;
using PagedList;

using PedidoWeb.Controllers.Negocio;

namespace PedidoWeb.Controllers
{
    public class CadastroController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Cadastro/
        [Authorize]
        public ViewResult Index(string sortOrder, string currentFilter, string search, int? page, string Cidade)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = sortOrder == "Nome" ? "Nome_desc" : "Nome";
            ViewBag.TipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;
            ViewBag.Cidade = string.IsNullOrEmpty(Cidade) ? null : Cidade;
            if (ViewBag.TipoUsuario == "ADMINISTRADOR")
            {
                ViewBag.UrlConfEmpresa = "/Empresa/Edit/" + pedidoHelper.UsuarioCorrente.CodEmpresa;
            }
            else
            {
                ViewBag.UrlConfEmpresa = null;
            }

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var cadastros = from s in db.Cadastroes.Where(c => c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa)
                          select s;
            if (ViewBag.TipoUsuario == "VENDEDOR" && pedidoHelper.UsuarioCorrente.CodEmpresa != "NUTRIVET")
                cadastros = cadastros.Where(c => c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID);
            if (!String.IsNullOrEmpty(search))
                cadastros = cadastros.Where(s => s.Nome.ToUpper().Contains(search.ToUpper()));
            if(!string.IsNullOrEmpty(Cidade))
                cadastros = cadastros.Where(s => s.DescCidade.ToUpper().Contains(Cidade.ToUpper()));
            switch (sortOrder)
            {
                case "Nome":
                    cadastros = cadastros.OrderBy(s => s.Nome);
                    break;
                case "Nome_desc":
                    cadastros = cadastros.OrderByDescending(s => s.Nome);
                    break;
                default:
                    cadastros = cadastros.OrderBy(s => s.Nome);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(cadastros.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Cadastro/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadastro cadastro = db.Cadastroes.First(c => c.CadastroID == id);
            if (cadastro == null)
            {
                return HttpNotFound();
            }
            return View(cadastro);
        }

        // Teste
        public JsonResult Teste(int? id)
        {
            if (id == null)
            {
                return Json(new JsonResult { Data = new { cadastro = string.Empty } }, JsonRequestBehavior.AllowGet);
            }
            Cadastro cadastro = db.Cadastroes.First(c => c.CadastroID == id);
            if (cadastro == null)
            {
                return Json(new JsonResult { Data = new { cadastro = string.Empty } }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonResult { Data = new { cadastro = cadastro } }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Cadastro/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(c => c.Descricao), "CidadeID", "Descricao");
        //    return View();
        //}

        // POST: /Cadastro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Create([Bind(Include="CadastroID,Nome,Fantasia,PercDescontoMaximo,CpfCnpj,Email,Situacao,VendedorID,Fone,IE,Endereco,Bairro,CidadeID,CEP")] Cadastro cadastro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        cadastro.Situacao = "ATIVO";
        //        cadastro.VendedorID = PedidoHelper.UsuarioCorrente.VendedorID;
        //        cadastro.CodEmpresa = PedidoHelper.UsuarioCorrente.CodEmpresa;
        //        cadastro.StatusSincronismo = "NOVO";
        //        db.Cadastroes.Add(cadastro);
        //        db.SaveChanges();
        //        return RedirectToAction("Index", "Pedido");
        //    }

        //    return RedirectToAction("Index", "Pedido");
        //}

        // GET: /Cadastro/Edit/5
        //[Authorize]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Cadastro cadastro = db.Cadastroes.Find(id);
        //    if (cadastro == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cadastro);
        //}

        // POST: /Cadastro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Edit([Bind(Include = "CadastroID,Nome,Fantasia,PercDescontoMaximo,CpfCnpj,Email,Situacao,VendedorID,Fone,IE,Endereco,Bairro,CidadeID,CEP")] Cadastro cadastro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cadastro).State = EntityState.Modified;
        //        cadastro.StatusSincronismo = "ALTERADO";
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cadastro);
        //}

        //// GET: /Cadastro/Delete/5
        //[Authorize]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Cadastro cadastro = db.Cadastroes.Find(id);
        //    if (cadastro == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cadastro);
        //}

        // POST: /Cadastro/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Cadastro cadastro = db.Cadastroes.Find(id);
        //    db.Cadastroes.Remove(cadastro);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

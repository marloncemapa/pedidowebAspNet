﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PedidoWeb.Models;

namespace PedidoWeb.Controllers
{
    public class PedidoController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Pedido/
        public ActionResult Index()
        {
            var pedidoes = db.Pedidoes.Include(p => p.Cadastro).Include(p => p.PrazoVencimento).Include(p => p.Transportador).Include(p => p.Vendedor);
            return View(pedidoes.ToList());
        }

        // GET: /Pedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: /Pedido/Create
        public ActionResult Create()
        {
            ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome");
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao");
            ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome");
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome");
            return View();
        }

        // POST: /Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PedidoID,Status,CadastroID,PrazoVencimentoID,Observacao,VendedorID,TipoFrete,TransportadorID,OrdemCompra,DataEmissao")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedidoes.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome", pedido.CadastroID);
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
            ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome", pedido.TransportadorID);
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
            return View(pedido);
        }

        // GET: /Pedido/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome", pedido.CadastroID);
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
            ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome", pedido.TransportadorID);
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
            return View(pedido);
        }

        // POST: /Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PedidoID,Status,CadastroID,PrazoVencimentoID,Observacao,VendedorID,TipoFrete,TransportadorID,OrdemCompra,DataEmissao")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome", pedido.CadastroID);
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
            ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome", pedido.TransportadorID);
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
            return View(pedido);
        }

        // GET: /Pedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: /Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidoes.Find(id);
            db.Pedidoes.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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

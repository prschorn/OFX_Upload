﻿using OFXParser;
using OFXParser.Entities;
using OFXUpload.Database;
using OFXUpload.Models.Interfaces;
using OFXUpload.Repositories.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OFXUpload.Controllers
{
  public class OFXController : Controller
  {
    private readonly IOFXHandler oFXHandler;
    private readonly IFinancialMovements financialMovementsHandler;
    private readonly IFinancialMovementRepository financialMovementsRepository;
    public OFXController(IOFXHandler ofx,
                         IFinancialMovements financialMovements,
                         IFinancialMovementRepository financialMovementRepository)
    {
      this.oFXHandler = ofx;
      this.financialMovementsHandler = financialMovements;
      this.financialMovementsRepository = financialMovementRepository;
    }
    // GET: OFX
    public async Task<ActionResult> Index()
    {
      ViewData["movements"] = await this.financialMovementsRepository.GetAllMovements();
      return View();
    }


    [HttpPost]
    public async Task<ActionResult> Index(HttpPostedFileBase file)
    {
      if (file == null)
        throw new Exception("Arquivo inválido, por favor, verifique o upload.");

      var extractedFile = this.oFXHandler.ExtractFile(file);

      var savedInformation = await this.financialMovementsHandler.SaveOFXInformation(extractedFile);

      if (savedInformation == "OK")
      {
        return this.RedirectToAction("Index");
      }

      return this.RedirectToAction("Index", "Erro ao salvar");

    }
  }
}
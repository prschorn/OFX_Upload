using OFXUpload.Models.Interfaces;
using OFXUpload.Repositories;
using OFXUpload.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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
      this.ViewData["movements"] = await this.financialMovementsRepository.GetAllMovements();
      return this.View();
    }


    [HttpPost]
    public async Task<ActionResult> Index(HttpPostedFileBase file)
    {
      if (file == null)
        throw new Exception("Arquivo inválido, por favor, verifique o upload.");


      //Call OFXparser to handle the file
      var extractedFile = this.oFXHandler.ExtractFile(file);

      //Handle the parsed file to save in the database and return the list of documents that couldn't be imported
      var savedInformation = await this.financialMovementsHandler.SaveOFXInformation(extractedFile);

      if (savedInformation.Count> 0)
      {
        this.ViewData["importedDocuments"] = savedInformation;
      }
      else
      {
        this.ViewData["importedDocuments"] = null;
      }

      return this.RedirectToAction("Index");

    }
  }
}
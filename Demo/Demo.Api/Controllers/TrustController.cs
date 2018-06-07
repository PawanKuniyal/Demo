using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Entities.CustomEntities;
using Helpa.Services.Interface;
using Helpa.Api.Utilities;

namespace Helpa.Api.Controllers
{
    //[Authorize(Roles = "HELPER")]
    public class TrustController : ApiController
    {
        private HelpaContext db = new HelpaContext();
        private IHelperServices services = null;
        private IFileServices fileServices = null;
        private IUtilityServices utilityServices = null;

        public TrustController()
        {
            services = new Services.HelperServices();
            fileServices = new Services.FileServices();
            utilityServices = new Services.UtiltiyServices();
        }  

        // GET: api/Trust
        public IQueryable<Document> GetDocuments()
        {
            return db.Documents;
        }

        // GET: api/Trust/5
        [ResponseType(typeof(Document))]
        public async Task<IHttpActionResult> GetDocument(int id)
        {
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/Trust/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.DocumentId)
            {
                return BadRequest();
            }

            db.Entry(document).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

          //POST: api/Trust
        [ResponseType(typeof(Helper))]
        public async Task<IHttpActionResult> PostDocument([FromBody]TrustDTO trustDTO)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!String.IsNullOrEmpty(trustDTO.SelfIntroduction))
                {
                    Helper helper = services.GetHelper(trustDTO.HelperId);
                    helper.Description = trustDTO.SelfIntroduction;

                    var result = await services.UpdateHelperAsync(helper);
                }

                string fileName = string.Empty;
                File file = null;

                if (ModelState.IsValid)
                {

                }
                if (trustDTO.Certificate.Count > 0)
                {
                    Document document = null;
                    foreach (var item in trustDTO.Certificate)
                    {
                        fileName = FileConverter.UploadFileToFileFolder(item.Certificate, "Documents");
                        file = new File
                        {
                            FileFolder = fileName,
                            RowStatus = "I",
                            CreatedBy = trustDTO.HelperId,
                            CreatedDate = DateTime.UtcNow
                        };
                        var result = await fileServices.AddFileAsync(file);
                        int fileId = result.FileId;
                        if (fileId > 0)
                        {
                            document = new Document
                            {
                                FileId = fileId,
                                HelperId = trustDTO.HelperId,
                                RowStatus = "I",
                                CreatedDate = DateTime.UtcNow
                            };
                            var resultDocument = fileServices.AddDocumentAsync(document);
                        }
                    }
                }

                if (trustDTO.Carousels.Count > 0)
                {
                    Entities.Carousel carousel = null;
                    foreach (var item in trustDTO.Carousels)
                    {
                        fileName = FileConverter.UploadFileToFileFolder(item.Images, "Carousels");
                        file = new File
                        {
                            FileFolder = fileName,
                            RowStatus = "I",
                            CreatedBy = trustDTO.HelperId,
                            CreatedDate = DateTime.UtcNow
                        };
                        var result = fileServices.AddFileAsync(file);
                        int fileId = result.Result.FileId;
                        if (fileId > 0)
                        {
                            carousel = new Entities.Carousel
                            {
                                ImageId = fileId,
                                HelperId = trustDTO.HelperId,
                                RowStatus = "I",
                                CreatedDate = DateTime.UtcNow
                            };
                            var resultCarousel = await fileServices.AddCarouselAsync(carousel);
                        }
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            //catch(Exception e)
            //{
            //    Console.Write(e.StackTrace);
            //    string error = "Invalid operation error.";
            //    return BadRequest(error);
            //}

            return Ok();
            }

        // DELETE: api/Trust/5
        [ResponseType(typeof(Document))]
        public async Task<IHttpActionResult> DeleteDocument(int id)
        {
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            db.Documents.Remove(document);
            await db.SaveChangesAsync();

            return Ok(document);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentExists(int id)
        {
            return db.Documents.Count(e => e.DocumentId == id) > 0;
        }
    }
}
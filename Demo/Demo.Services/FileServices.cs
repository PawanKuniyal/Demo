using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Services.Interface;
using Helpa.Services.Repository;
using Helpa.Entities;

namespace Helpa.Services
{
    public class FileServices : IFileServices
    {
        private IRepository<File> fileContext;
        private IRepository<Document> documentContext;
        private IRepository<Carousel> carouselContext;

        public FileServices()
        {
            fileContext = new Repository<File>();
            documentContext = new Repository<Document>();
            carouselContext = new Repository<Carousel>();
        }

        #region Files
        public File AddFile(File file)
        {
            var result = fileContext.Insert(file);
            return result;
        }

        public async Task<File> AddFileAsync(File file)
        {
            var result = await fileContext.InsertAsync(file);
            return result;
        }
        #endregion

        #region Documents
        public Document AddDocument(Document document)
        {
            var result = documentContext.Insert(document);
            return result;
        }

        public async Task<Document> AddDocumentAsync(Document document)
        {
            try
            {
                var result = await documentContext.InsertAsync(document);
                return result;
            }
            catch (Exception ex)
            {
              
                throw;
            }
          
         
        }
        #endregion

        #region Carousels
        public Carousel AddCarousel(Carousel carousel)
        {
            var result = carouselContext.Insert(carousel);
            return result;
        }

        public async Task<Carousel> AddCarouselAsync(Carousel carousel)
        {
            var result = await carouselContext.InsertAsync(carousel);
            return result;
        }
        #endregion
    }
}

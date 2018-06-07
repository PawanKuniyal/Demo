using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Entities;

namespace Helpa.Services.Interface
{
    public interface IFileServices
    {
        File AddFile(File file);
        Task<File> AddFileAsync(File file);
        Document AddDocument(Document document);
        Task<Document> AddDocumentAsync(Document document);
        Carousel AddCarousel(Carousel carousel);
        Task<Carousel> AddCarouselAsync(Carousel carousel);
    }
}

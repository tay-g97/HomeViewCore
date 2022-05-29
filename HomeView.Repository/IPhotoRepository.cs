using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HomeView.Models.Photo;

namespace HomeView.Repository
{
    public interface IPhotoRepository
    {
        public Task<Photo> GetAsync(int photoId);
        public Task<Photo> InsertAsync(PhotoCreate photoCreate, int userId, int propertyId, bool thumbnail);
        public Task<List<Photo>> GetAllByPropertyIdAsync(int propertyId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HomeView.Models.Profilephoto;

namespace HomeView.Repository
{
    public interface IProfilephotoRepository
    {
        public Task<Profilephoto> GetAsync(int photoId);
        public Task<Profilephoto> InsertAsync(ProfilephotoCreate profilephotoCreate, int userId);
        public Task<List<Profilephoto>> GetAllByIdAsync(int userId);
        public Task<int> DeleteAsync(int photoId);
    }
}

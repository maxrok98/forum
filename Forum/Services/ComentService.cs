using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Repositories;
using Forum.Services.Communication;

namespace Forum.Services
{
    public class ComentService : IComentService
    {
        private readonly IComentRepository _comentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ComentService(IComentRepository comentRepository, IUnitOfWork unitOfWork)
        {
            _comentRepository = comentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ComentResponse> AddAsync(Coment coment)
        {
            try
            {
                await _comentRepository.AddAsync(coment);
                await _unitOfWork.CompleteAsync();

                return new ComentResponse(coment);
            }
            catch(Exception ex)
            {
                return new ComentResponse($"An error occurred when saving the coment: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Coment>> GetAllAsync()
        {
            return await _comentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Coment>> GetAllFromPostAsync(string id)
        {
            return await _comentRepository.GetAllFromPostAsync(id);
        }

        public async Task<IEnumerable<Coment>> GetAllFromUserAsync(string id)
        {
            return await _comentRepository.GetAllFromUserAsync(id);
        }

        public async Task<Coment> GetAsync(string id)
        {
            return await _comentRepository.GetAsync(id);
        }

        public async Task<ComentResponse> RemoveAsync(string id)
        {
            var existingComent = await _comentRepository.GetAsync(id);

            if (existingComent == null)
                return new ComentResponse("Coment not found.");

            try
            {
                _comentRepository.Remove(existingComent);
                await _unitOfWork.CompleteAsync();

                return new ComentResponse(existingComent);
            }
            catch (Exception ex)
            {
                return new ComentResponse($"An error occurred when deleting the coment: {ex.Message}");
            }
        }

        public async Task<ComentResponse> UpdateAsync(string id, Coment coment)
        {
            var existingComent = await _comentRepository.GetAsync(id);

            if (existingComent == null)
                return new ComentResponse("Coment not found.");

            existingComent.Text = coment.Text ?? existingComent.Text;

            try
            {
                _comentRepository.Update(existingComent);
                await _unitOfWork.CompleteAsync();

                return new ComentResponse(existingComent);
            }
            catch (Exception ex)
            {
                return new ComentResponse($"An error occurred when updating the coment: {ex.Message}");
            }
        }
    }
}

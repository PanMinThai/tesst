using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Enums;
using TodoList_Project.Core.DAL.Repositories.Interfaces;
using TodoList_Project.Features.Categories.Models;
using TodoList_Project.Features.Tasks.Models;

namespace TodoList_Project.Features.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(DateTimePeriod period = DateTimePeriod.All)
        {
            var categories = await _categoryRepository.GetAllWithTaskStatsAsync(period);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
        public async Task UpdateCategoryAsync(CategoryDto model)
        {
            var categoryEntity = await _categoryRepository.GetByIdAsync(model.Id);
            if (categoryEntity == null)
                throw new Exception("Category not found");

            _mapper.Map(model, categoryEntity);
            await _categoryRepository.UpdateAsync(categoryEntity);
        }
    }
}
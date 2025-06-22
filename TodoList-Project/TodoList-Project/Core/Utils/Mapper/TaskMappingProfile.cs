using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Features.Categories.Models;
using TodoList_Project.Features.Tasks.Models;
using TaskStatus = TodoList_Project.Core.DAL.Enums.TaskStatus;

namespace TodoList_Project.Core.Utils.Mapper
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskEntity, TaskModel>()
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate));

            CreateMap<TaskModel, TaskEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
            CreateMap<CategoryEntity, CategoryDto>()
            .ForMember(dest => dest.TotalTasks,
                opt => opt.MapFrom(src => src.TaskCategories.Count))
            .ForMember(dest => dest.CompletedTasks,
                opt => opt.MapFrom(src => src.TaskCategories.Count(tc => tc.Task.Status == TaskStatus.Completed)));

        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList_Project.Core.DAL.Entities.SQL;
using TodoList_Project.Core.DAL.Entities.SQL.Auth;
using TodoList_Project.Features.Categories.Models;
using TodoList_Project.Features.Roles.Models;
using TodoList_Project.Features.Tasks.Models;
using TodoList_Project.Features.Users.Models;
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
            CreateMap<CategoryEntity, CategoryDto>();
            CreateMap<TaskCategoryEntity, TaskCategoryDto>();
            CreateMap<TaskEntity, TaskDto>();
            CreateMap<CategoryDto, CategoryEntity>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
            .ForMember(dest => dest.TaskCategories, opt => opt.Ignore());

            // Auth mappings
            CreateMap<UserEntity, UserModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
            CreateMap<RoleEntity, RoleModel>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission.Name)));
            CreateMap<PermissionEntity, PermissionModel>();
        }
    }
}

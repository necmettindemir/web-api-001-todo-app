using AutoMapper;
using TodoDB.DTOs;
using TodoDB.Helpers;
using TodoDB.Models;

namespace TodoWebApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            ////custom map
            //CreateMap<User, UserForListDTO>()
            //    .ForMember(dest => dest.PhotoUrl, opt =>
            //    {
            //        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            //    })
            //    .ForMember(dest => dest.Age, opt =>
            //    {
            //        opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            //    });

            ////to calculate field in destination
            //CreateMap<TodoForSaveDTO, TodoItem>()               
            //    .ForMember(dest => dest.UserId, opt =>
            //    {
            //        opt.ResolveUsing(d => 0);
            //    });

            CreateMap<User, UserForListDTO>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<TodoForSaveDTO, TodoItem>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<PageParamsForPost, PageParamsForReturn>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            

            //CreateMap<TodoForSaveDTO, TodoItem>().ForAllOtherMembers(opts => opts.Ignore());

            //CreateMap<TodoForSaveDTO, TodoItem>().ForSourceMember(x => x.TodoDateStr, y => y.Ignore());





        }

    }
}

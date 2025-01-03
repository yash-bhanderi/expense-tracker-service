using AutoMapper;
using ExpenseTracker.Dtos.Expense;
using ExpenseTracker.Models;

namespace ExpenseTracker.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Expense, ExpenseReadDto>();
            CreateMap<ExpenseCreateDto, Expense>();
            CreateMap<ExpenseUpdateDto, Expense>();
        }
    }
}
using AutoMapper;
using Chat.Data.Objects.Entities;
using Chat.Data.Objects.Helpers.DTO;

namespace Chat.Data.Objects.Helpers;
public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<MessageEntity, MessageDto>();
        CreateMap<MessageDto, MessageEntity>();
    }
}

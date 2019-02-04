using System;

namespace ITNews.DTO.UserDto
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DTO.UserDto.UserDto User { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Specialization { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}

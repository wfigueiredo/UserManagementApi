using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Dto;
using UserManagement.Domain.Enum;
using UserManagement.Domain.Model;
using UserManagement.Util;

namespace UserManagement.Translators
{
    public static class UserTranslator
    {
        public static UserDto ToAuthenticatedDto(this User user, string token)
        {
            if (user != null)
            {
                Enum.TryParse(user.Role.Code, true, out RoleType roleType);
                return new UserDto()
                {
                    username = user.Username,
                    role = roleType,
                    token = token
                };
            }

            return null;
        }

        public static UserDto ToSavedDto(this User user)
        {
            if (user != null)
            {
                Enum.TryParse(user.Role.Code, true, out RoleType roleType);
                return new UserDto()
                {
                    fullname = $"{user.Name} {user.LastName}",
                    username = user.Username,
                    role = roleType,
                };
            }

            return null;
        }

        public static UserDto ToDetailsDto(this User user)
        {
            if (user != null)
            {
                Enum.TryParse(user.Role.Code, true, out RoleType roleType);
                return new UserDto()
                {
                    fullname = $"{user.Name} {user.LastName}",
                    email = user.EmailAddress,
                    phone = user.PhoneNumber,
                    role = roleType,
                };
            }

            return null;
        }

        public static UserDto ToFullDetailsDto(this User user)
        {
            if (user != null)
            {
                Enum.TryParse(user.Role.Code, true, out RoleType roleType);
                return new UserDto()
                {
                    name = user.Name,
                    lastname = user.LastName,
                    username = user.Username,
                    email = user.EmailAddress,
                    phone = user.PhoneNumber,
                    role = roleType
                };
            }

            return null;
        }

        public static User ToDomain(this UserDto dto, Role role)
        {
            if (dto != null)
            {
                return new User()
                {
                    Name = dto.name,
                    LastName = dto.lastname,
                    Username = dto.username,
                    Secret = HashUtil.SHA1(dto.secret),
                    EmailAddress = dto.email,
                    PhoneNumber = dto.phone,
                    Role = role
                };
            }

            return null;
        }

        public static IList<UserDto> ToDetailsDto(this IEnumerable<User> users)
        {
            return users?.Select(ToDetailsDto).ToList();
        }
    }
}

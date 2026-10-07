using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }

    [JsonPropertyName("lastModifiedBy")]
    public string? LastModifiedBy { get; set; }

    public static UserDTO ConvertFrom(User user)
    {
        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            LastModified = user.LastModified,
            ModifiedBy = user.ModifiedBy,
            RoleId = user.RoleId,
            LastModifiedBy = user.LastModifiedBy
        };
    }

    public static User ConvertTo(UserDTO userDTO)
    {
        return new User
        {
            UserId = userDTO.UserId,
            Username = userDTO.Username,
            Email = userDTO.Email,
            CreatedAt = userDTO.CreatedAt,
            IsActive = userDTO.IsActive,
            LastModified = userDTO.LastModified,
            ModifiedBy = userDTO.ModifiedBy,
            RoleId = userDTO.RoleId,
            LastModifiedBy = userDTO.LastModifiedBy
        };
    }
}
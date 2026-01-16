namespace Domain.Entities;

public class DiscordUser
{
    // The "Snowflake" ID (e.g., "43290482309482")
    public string Id { get; set; }

    // The unique username (e.g., "courr")
    public string Username { get; set; }

    // The display name (e.g., "Courr The Great")
    // Note: This field might be null if they haven't set a specific global name
    public string? Global_name { get; set; }

    // The hash string for their profile picture
    public string? Avatar { get; set; }

    // Optional: The discriminator (e.g., "4422") - mostly deprecated by Discord but still exists
    public string? Discriminator { get; set; }

    // Optional: Their email (only if you requested the "email" scope)
    public string? Email { get; set; }
}
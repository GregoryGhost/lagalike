namespace Lagalike.Telegram.Shared
{
    /// <summary>
    /// An inputed command from a user. 
    /// </summary>
    /// <param name="UserLabel">Displaying user label in a demo commands menu.</param>
    /// <param name="CommandValue">What's going to do when the command was selected.</param>
    public record CommandInfo(string UserLabel, string CommandValue);
}
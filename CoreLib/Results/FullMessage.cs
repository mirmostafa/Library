using Library.Exceptions;

namespace Library.Results;

public record struct FullMessage(in string Messege, in string? Instruction = null, in string? Title = null, in string? Details = null)
{
    public static explicit operator string?(FullMessage? fullMessage) => fullMessage?.Messege;
    public static implicit operator FullMessage?(string? message) => message is null ? null : new(message);

    public override string ToString() 
        => this.Messege;
}
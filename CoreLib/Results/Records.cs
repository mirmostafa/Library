using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Results;

public record struct FullMessage(string Messege, string? Instruction, string? Title, string? Details);

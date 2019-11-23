using _01_Composite.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Composite
{
    public interface IGiftOperations
    {
        void Add(GiftBase gift);
        void Remove(GiftBase gift);
    }
}

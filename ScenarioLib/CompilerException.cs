﻿using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace ScenarioLib
{
    internal class CompilerException : Exception
    {
        private Diagnostic err;

        public CompilerException(Diagnostic err)
            : base(err.GetMessage())
        {

        }
    }
}
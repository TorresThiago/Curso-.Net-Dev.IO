using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevIO.App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormatDocumento(this RazorPage page, int tipoPessoa, string documento)
        {
            return tipoPessoa == 1
                ? Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00")
                : Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string RemovePontuacaoDocumento(string documento)
        {
            return Regex.Replace(documento, "[0-9]+", "gi");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDBXEditor.Storage;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor.Reader.FileTypes
{
    public class WDBC : DBHeader
    {
        public WDBC() : base() { }

        private static Dictionary<string, int> ColumnSizes = new Dictionary<string, int>
        {
            { "int", 4 },
            { "string", 4 }, // string is actually a uint32 offset in the string table
            { "float", 4 },
        };

        public WDBC(Table definition)
            : base()
        {
            var LocalizationCount = (definition.Build <= (int)ExpansionFinalBuild.Classic ? 9 : 17); //Pre TBC had 9 locales
            var numLocalizedFields = definition.Fields.Count(x => x.Type == "loc");
            //Special case for localized strings, all locales are present (subtract 1 for non-localized column in definition)
            FieldCount = (uint)(definition.Fields.Count + (numLocalizedFields * (LocalizationCount  -1)));

            RecordSize = (uint)definition.Fields.Sum(x => x.Type != "loc" ? ColumnSizes[x.Type] : (ColumnSizes["string"] * LocalizationCount));
        }

        public override void ReadHeader(ref BinaryReader dbReader, string signature)
        {
            base.ReadHeader(ref dbReader, signature);
        }
    }
}

using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;

namespace BuildingRecordsApp.Services;

internal sealed record ExportWorksheet(
    string Name,
    IReadOnlyList<IReadOnlyList<object?>> Rows,
    int FrozenRows = 1,
    IReadOnlyList<(int StartColumn, int EndColumn, string Title, int Style)>? Groups = null);

internal static class RegisterExcelWorkbookWriter
{
    private const string SpreadsheetContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";

    public static byte[] Write(IReadOnlyList<ExportWorksheet> worksheets)
    {
        using var output = new MemoryStream();
        using (var archive = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true))
        {
            WriteEntry(archive, "[Content_Types].xml", ContentTypes(worksheets.Count));
            WriteEntry(archive, "_rels/.rels", RootRelationships());
            WriteEntry(archive, "docProps/core.xml", CoreProperties());
            WriteEntry(archive, "docProps/app.xml", AppProperties(worksheets));
            WriteEntry(archive, "xl/workbook.xml", Workbook(worksheets));
            WriteEntry(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationships(worksheets.Count));
            WriteEntry(archive, "xl/styles.xml", Styles());
            for (var index = 0; index < worksheets.Count; index++)
                WriteEntry(archive, $"xl/worksheets/sheet{index + 1}.xml", Worksheet(worksheets[index]));
        }

        return output.ToArray();
    }

    private static void WriteEntry(ZipArchive archive, string path, string content)
    {
        var entry = archive.CreateEntry(path, CompressionLevel.Fastest);
        using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
        writer.Write(content);
    }

    private static string ContentTypes(int count)
    {
        var sheets = string.Concat(Enumerable.Range(1, count).Select(index =>
            $"<Override PartName=\"/xl/worksheets/sheet{index}.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>"));
        return $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/xl/workbook.xml" ContentType="{SpreadsheetContentType}"/><Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/><Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/><Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>{sheets}</Types>""";
    }

    private static string RootRelationships() => """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/><Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/></Relationships>""";

    private static string CoreProperties()
    {
        var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        return $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><dc:title>Building register export</dc:title><dc:creator>BuildingRecordsApp</dc:creator><cp:lastModifiedBy>BuildingRecordsApp</cp:lastModifiedBy><dcterms:created xsi:type="dcterms:W3CDTF">{now}</dcterms:created><dcterms:modified xsi:type="dcterms:W3CDTF">{now}</dcterms:modified></cp:coreProperties>""";
    }

    private static string AppProperties(IReadOnlyList<ExportWorksheet> worksheets)
    {
        var titles = string.Concat(worksheets.Select(sheet => $"<vt:lpstr>{Xml(sheet.Name)}</vt:lpstr>"));
        return $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes"><Application>BuildingRecordsApp</Application><TitlesOfParts><vt:vector size="{worksheets.Count}" baseType="lpstr">{titles}</vt:vector></TitlesOfParts></Properties>""";
    }

    private static string Workbook(IReadOnlyList<ExportWorksheet> worksheets)
    {
        var sheets = string.Concat(worksheets.Select((sheet, index) =>
            $"<sheet name=\"{Xml(sheet.Name)}\" sheetId=\"{index + 1}\" r:id=\"rId{index + 1}\"/>"));
        return $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><bookViews><workbookView/></bookViews><sheets>{sheets}</sheets></workbook>""";
    }

    private static string WorkbookRelationships(int count)
    {
        var sheets = string.Concat(Enumerable.Range(1, count).Select(index =>
            $"<Relationship Id=\"rId{index}\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet{index}.xml\"/>"));
        return $"""<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">{sheets}<Relationship Id="rId{count + 1}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/></Relationships>""";
    }

    private static string Styles() => """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"><numFmts count="1"><numFmt numFmtId="164" formatCode="yyyy-mm-dd"/></numFmts><fonts count="3"><font><sz val="11"/><name val="Calibri"/></font><font><b/><color rgb="FFFFFFFF"/><sz val="11"/><name val="Calibri"/></font><font><b/><color rgb="FF000000"/><sz val="12"/><name val="Calibri"/></font></fonts><fills count="11"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill><fill><patternFill patternType="solid"><fgColor rgb="FF222222"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFF2F2F2"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFD9EAF7"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFFCE4D6"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFE2F0D9"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFF4D7F4"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFE4CDF6"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFD0F0ED"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFFFF2CC"/><bgColor indexed="64"/></patternFill></fill></fills><borders count="2"><border><left/><right/><top/><bottom/><diagonal/></border><border><left style="thin"><color rgb="FFB7B7B7"/></left><right style="thin"><color rgb="FFB7B7B7"/></right><top style="thin"><color rgb="FFB7B7B7"/></top><bottom style="thin"><color rgb="FFB7B7B7"/></bottom><diagonal/></border></borders><cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs><cellXfs count="13"><xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0"/><xf numFmtId="0" fontId="1" fillId="2" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center" wrapText="1"/></xf><xf numFmtId="0" fontId="0" fillId="3" borderId="1" xfId="0"/><xf numFmtId="164" fontId="0" fillId="0" borderId="1" xfId="0" applyNumberFormat="1"/><xf numFmtId="0" fontId="2" fillId="4" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="5" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="6" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="7" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="8" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="9" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyAlignment="1"><alignment vertical="top" wrapText="1"/></xf><xf numFmtId="0" fontId="2" fillId="3" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="10" borderId="1" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf></cellXfs><cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles><dxfs count="1"><dxf><fill><patternFill patternType="solid"><fgColor rgb="FFFFC7CE"/><bgColor indexed="64"/></patternFill></fill><font><color rgb="FF9C0006"/></font></dxf></dxfs></styleSheet>""";

    private static string Worksheet(ExportWorksheet worksheet)
    {
        var groups = worksheet.Groups ?? [];
        var groupOffset = groups.Count > 0 ? 1 : 0;
        var columnCount = worksheet.Rows.Count == 0 ? 1 : worksheet.Rows.Max(row => row.Count);
        var builder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
        builder.Append("<sheetViews><sheetView workbookViewId=\"0\"><pane ySplit=\"")
            .Append(worksheet.FrozenRows + groupOffset)
            .Append("\" topLeftCell=\"A").Append(worksheet.FrozenRows + groupOffset + 1)
            .Append("\" activePane=\"bottomLeft\" state=\"frozen\"/></sheetView></sheetViews>");
        builder.Append("<cols>");
        for (var column = 1; column <= columnCount; column++)
            builder.Append($"<col min=\"{column}\" max=\"{column}\" width=\"{ColumnWidth(worksheet, column - 1):0.##}\" customWidth=\"1\"/>");
        builder.Append("</cols><sheetData>");

        if (groups.Count > 0)
        {
            builder.Append("<row r=\"1\" ht=\"24\" customHeight=\"1\">");
            foreach (var group in groups)
                builder.Append(Cell(1, group.StartColumn, group.Title, group.Style));
            builder.Append("</row>");
        }

        for (var rowIndex = 0; rowIndex < worksheet.Rows.Count; rowIndex++)
        {
            var excelRow = rowIndex + 1 + groupOffset;
            var row = worksheet.Rows[rowIndex];
            builder.Append($"<row r=\"{excelRow}\"{(rowIndex == 0 ? " ht=\"34\" customHeight=\"1\"" : string.Empty)}>");
            for (var columnIndex = 0; columnIndex < row.Count; columnIndex++)
            {
                var style = rowIndex == 0 ? 1 : rowIndex % 2 == 0 ? 2 : 10;
                if (row[columnIndex] is DateTime) style = 3;
                builder.Append(Cell(excelRow, columnIndex + 1, row[columnIndex], style));
            }
            builder.Append("</row>");
        }
        builder.Append("</sheetData>");

        if (worksheet.Rows.Count > 0)
        {
            var headerRow = 1 + groupOffset;
            builder.Append($"<autoFilter ref=\"A{headerRow}:{ColumnName(columnCount)}{Math.Max(headerRow, worksheet.Rows.Count + groupOffset)}\"/>");
        }

        if (groups.Count > 0)
        {
            builder.Append($"<mergeCells count=\"{groups.Count}\">");
            foreach (var group in groups)
                builder.Append($"<mergeCell ref=\"{ColumnName(group.StartColumn)}1:{ColumnName(group.EndColumn)}1\"/>");
            builder.Append("</mergeCells>");
        }

        if (worksheet.Rows.Count > 0)
        {
            var headerRow = 1 + groupOffset;
            AppendRulesSignedFormatting(builder, worksheet, headerRow + 1, worksheet.Rows.Count + groupOffset);
        }
        builder.Append("</worksheet>");
        return builder.ToString();
    }

    private static void AppendRulesSignedFormatting(StringBuilder builder, ExportWorksheet worksheet, int firstDataRow, int lastDataRow)
    {
        var column = worksheet.Name switch
        {
            "MAIN" => 22,
            "LEASES" => 8,
            _ => 0
        };
        if (column == 0 || lastDataRow < firstDataRow) return;
        var range = $"{ColumnName(column)}{firstDataRow}:{ColumnName(column)}{lastDataRow}";
        builder.Append($"<conditionalFormatting sqref=\"{range}\"><cfRule type=\"cellIs\" dxfId=\"0\" priority=\"1\" operator=\"equal\"><formula>\"No\"</formula></cfRule></conditionalFormatting>");
    }

    private static double ColumnWidth(ExportWorksheet worksheet, int column)
    {
        var max = worksheet.Rows.Select(row => column < row.Count ? Convert.ToString(row[column], CultureInfo.InvariantCulture)?.Length ?? 0 : 0).DefaultIfEmpty(0).Max();
        return Math.Clamp(max + 2, 10, 32);
    }

    private static string Cell(int row, int column, object? value, int style)
    {
        var reference = $"{ColumnName(column)}{row}";
        if (value is null) return $"<c r=\"{reference}\" s=\"{style}\"/>";
        if (value is bool boolean) return $"<c r=\"{reference}\" s=\"{style}\" t=\"inlineStr\"><is><t>{(boolean ? "Yes" : "No")}</t></is></c>";
        if (value is DateTime date) return $"<c r=\"{reference}\" s=\"{style}\"><v>{date.ToOADate().ToString(CultureInfo.InvariantCulture)}</v></c>";
        if (value is byte or short or int or long or float or double or decimal)
            return $"<c r=\"{reference}\" s=\"{style}\"><v>{Convert.ToString(value, CultureInfo.InvariantCulture)}</v></c>";
        return $"<c r=\"{reference}\" s=\"{style}\" t=\"inlineStr\"><is><t xml:space=\"preserve\">{Xml(Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty)}</t></is></c>";
    }

    private static string ColumnName(int column)
    {
        var result = string.Empty;
        while (column > 0)
        {
            column--;
            result = (char)('A' + column % 26) + result;
            column /= 26;
        }
        return result;
    }

    private static string Xml(string value) => SecurityElement.Escape(value) ?? string.Empty;
}

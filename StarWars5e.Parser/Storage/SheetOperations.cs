using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StarWars5e.Parser.Storage
{
    public class SheetOperations
    {
        private readonly SheetsService _sheetsService;
        private readonly string _featureSheetId;

        public SheetOperations(IServiceProvider serviceProvider)
        {
            _sheetsService = serviceProvider.GetService<SheetsService>();
            _featureSheetId = serviceProvider.GetService<IConfiguration>()?["FeaturesSheetId"] ?? "";
        }

        public async Task<UpdateValuesResponse> UpdateFeatureSheetAsync(IList<IList<object>> data)
        {
            var valuesToWrite = data;
            var indexOfHighestColumn = 2;

            var existingValues = await _sheetsService.Spreadsheets.Values.Get(_featureSheetId, "2:3001").ExecuteAsync();
            var badRows = new List<int>();

            if (existingValues.Values?.Any() == true)
            {
                valuesToWrite = new List<IList<object>>();
                var allData = existingValues.Values.Concat(data);

                var groupedExisting = allData.GroupBy(r => new {RowKey = r.ElementAtOrDefault(0), Level = r.ElementAtOrDefault(1)?.ToString()?.Trim() ?? ""}).ToList();
                foreach (var group in groupedExisting)
                {
                    var entry = new List<object> {group.Key.RowKey, group.Key.Level};
                    foreach (var child in group)
                    {
                        entry.AddRange(child.Skip(2));

                        if (child.Count > indexOfHighestColumn)
                        {
                            indexOfHighestColumn = child.Count - 1;
                        }
                    }

                    valuesToWrite.Add(entry);
                }

                var existsInSheetButNotInData = existingValues.Values.Where(e => !data.Any(d => d.Contains(e.ElementAtOrDefault(0)))).ToList();
                foreach (var badValue in existsInSheetButNotInData)
                {
                    var findValue = valuesToWrite.FirstOrDefault(v =>
                        v.ElementAtOrDefault(0) == badValue.ElementAtOrDefault(0));
                    if (findValue != null)
                    {
                        var index = valuesToWrite.IndexOf(findValue);
                        badRows.Add(index + 2); //compensate for 0-indexing and header row
                    }
                }
            }

            var update =
                _sheetsService.Spreadsheets.Values.Update(
                    new ValueRange
                    {
                        Values = valuesToWrite,
                        MajorDimension = "ROWS"
                    }, _featureSheetId, "2:3001");
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

            var response = await update.ExecuteAsync();

            if (badRows.Any())
            {
                await MarkRedRows(badRows);
            }

            await UpdateRangeWithParsedStyleAsync( new GridRange
            {
                StartColumnIndex = 0,
                EndColumnIndex = indexOfHighestColumn,
                StartRowIndex = 1,
                EndRowIndex = valuesToWrite.Count + 1
            });

            return response;
        }

        private async Task MarkRedRows(List<int> rows)
        {
            await _sheetsService.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
            {
                Requests = rows.Select(r => new Request
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Rows = new List<RowData>
                        {
                            new()
                            {
                                Values = new List<CellData>
                                {
                                    new ()
                                    {
                                        UserEnteredFormat = new CellFormat
                                        {
                                            BackgroundColor = new Color
                                            {
                                                Red = 1
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Fields = "userEnteredFormat(backgroundColor)",
                        Range = new GridRange
                        {
                            StartRowIndex = r-1,
                            EndRowIndex = r
                        }
                    }
                }).ToList()
                //Requests = new List<Request>
                //{
                //    new()
                //    {
                //        UpdateCells = new UpdateCellsRequest
                //        {
                //            Rows = new List<RowData>
                //            {
                //                new()
                //                {
                //                    Values = new List<CellData>
                //                    {
                //                        new ()
                //                        {
                //                            UserEnteredFormat = new CellFormat
                //                            {
                //                                BackgroundColor = new Color
                //                                {
                //                                    Red = 1
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            },
                //            Fields = "userEnteredFormat(backgroundColor)",
                //            Range = new GridRange
                //            {
                //                StartRowIndex = 1,
                //                EndRowIndex = 2
                //            }
                //        }
                //    }
                //}
            }, _featureSheetId).ExecuteAsync();
        }

        private async Task UpdateRangeWithParsedStyleAsync(GridRange range)
        {
            await _sheetsService.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request>
                {
                    //new()
                    //{
                    //    UpdateBorders = new UpdateBordersRequest
                    //    {
                    //        Bottom = new Border
                    //        {
                    //            Style = "DASHED",
                    //            Color = new Color
                    //            {
                    //                Blue = 1
                    //            },
                    //            Width = 2
                    //        },
                    //        Right = new Border
                    //        {
                    //            Style = "DASHED",
                    //            Color = new Color
                    //            {
                    //                Blue = 1
                    //            },
                    //            Width = 4
                    //        },
                    //        Left = new Border
                    //        {
                    //            Style = "DASHED",
                    //            Color = new Color
                    //            {
                    //                Blue = 1
                    //            },
                    //            Width = 2
                    //        },
                    //        Range = range
                    //    }
                    //},
                    new ()
                    {
                        AutoResizeDimensions = new AutoResizeDimensionsRequest
                        {
                            Dimensions = new DimensionRange
                            {
                                Dimension = "COLUMNS",
                                StartIndex = 0,
                                EndIndex = range.EndColumnIndex
                            }
                        }
                    },
                    new ()
                    {
                        SortRange = new SortRangeRequest
                        {
                            Range = range,
                            SortSpecs = new List<SortSpec>
                            {
                                new()
                                {
                                    SortOrder = "ASCENDING",
                                    DimensionIndex = 0
                                }
                            }
                        }
                    }
                }
            }, _featureSheetId).ExecuteAsync();
        }
    }
}

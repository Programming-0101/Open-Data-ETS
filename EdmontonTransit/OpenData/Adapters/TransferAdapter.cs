using EdmontonTransit.Common;
using EdmontonTransit.OpenData.ApiModels;
using System;

namespace EdmontonTransit.OpenData.Adapters
{
    public class TransferAdapter : AbstractTextFileAdapter<Transfer>
    {
        public TransferAdapter(FileFormat fileFormat, string filePath, bool skipFirstLine) : base(fileFormat, filePath, skipFirstLine)
        {
        }

        protected override Transfer ParseRow(string row)
        {
            /*
             * from_stop_id,to_stop_id,transfer_type,min_transfer_time
             * 1001,1002,1,
             */
            string[] csvData = row.Split(',');
            return new Transfer()
            {
                FromStopId = int.Parse(csvData[0]),
                ToStopId = int.Parse(csvData[1]),
                TransferType = int.Parse(csvData[2]),
                MinTransferTime = csvData[3]
            };
        }
    }
}

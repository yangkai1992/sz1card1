using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sz1card1.Common.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtRegroupItems : EscherRecord
	{
		public MsofbtRegroupItems(EscherRecord record) : base(record) { }

		public MsofbtRegroupItems()
		{
			this.Type = EscherRecordType.MsofbtRegroupItems;
		}

	}
}

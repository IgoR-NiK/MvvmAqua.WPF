using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAqua.Validation
{
	internal interface IValidity
	{
		bool IsValid { get; }
		bool Validate();
	}
}
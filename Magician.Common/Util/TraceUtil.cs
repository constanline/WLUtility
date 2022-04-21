using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Magician.Common.Util
{
    public static class TraceUtil
	{
		public static string GetMethodName(int iCallIdx)
		{
			string result;
			try
			{
				var stackTrace = new StackTrace();
				var frame = stackTrace.GetFrame(iCallIdx);
				var method = frame.GetMethod();
				if (method == null)
				{
					result = string.Empty;
				}
				else if (method.Name.IndexOf("Can't Get MethodBase") >= 0)
				{
					result = string.Empty;
				}
				else
				{
					string str = string.Empty;
					if (method.DeclaringType != null)
					{
						str = method.DeclaringType.ToString() + ".";
					}
					result = str + method.Name;
				}
			}
			catch
			{
				result = "(Can't Get MethodBase)";
			}
			return result;
		}
		public static string GetTraceMethodName(int iTargetCallIdx)
		{
			iTargetCallIdx += 2;
			if (iTargetCallIdx < 2)
			{
				return GetMethodName(iTargetCallIdx);
			}
			string text = string.Empty;
			for (int i = 2; i <= iTargetCallIdx; i++)
			{
				text = GetMethodName(i) + "()." + text;
			}
			return text;
		}
	}
}

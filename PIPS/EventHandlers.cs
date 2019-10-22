using System;
using System.Xml;

namespace PIPS
{
	public delegate void BooleanHandler(bool state);
	public delegate void IntHandler(int val);
	public delegate void StringHandler(string cmd);
	public delegate void TimeSpanHandler(TimeSpan time);
	public delegate void NullHandler();
	public delegate void ByteHandler(byte val);
	public delegate void CharHandler(char val);
	public delegate void DoubleHandler(double val);
	public delegate void LongHandler(long val);
	public delegate void DateTimeHandler(DateTime dt);
	public delegate void XmlDocumentHandler(XmlDocument xml);
	public delegate void XmlNodeHandler(XmlNode node);
}

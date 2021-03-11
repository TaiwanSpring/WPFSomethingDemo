using System;
namespace WPFDragToReorder.Enum
{
	[Flags]
	enum DirectionEnum : byte
	{
		None = 0x00,

		Left = 0x01,
		Top = 0x02,
		Right = 0x04,
		Bottom = 0x08,

		LeftTop = Left | Top,
		TopRight = Top | Right,
		RightBottom = Right | Bottom,
		BottomLeft = Bottom | Left,

		TopLine = LeftTop | TopRight,
		RightLine = TopRight | RightBottom,
		BottomLine = RightBottom | BottomLeft,
		LeftLine = BottomLeft | LeftTop
	}
}

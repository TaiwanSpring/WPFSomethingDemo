using WPFDragToReorder.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDragToReorder.Model
{
	class LineAdorner : Adorner
	{
		Pen pen;
		Point leftTop;
		Point topRight;
		Point rightBottom;
		Point bottomLeft;

		DirectionEnum direction;
		public DropPositionEnum DropPosition { get; private set; }

		readonly AdornerLayer adornedLayer;
		public LineAdorner(UIElement adornedElement) : base(adornedElement)
		{
			this.adornedLayer = AdornerLayer.GetAdornerLayer(AdornedElement);

			if (this.adornedLayer != null)
			{
				this.adornedLayer.Add(this);
			}

			this.pen = new Pen(Brushes.Red, 1);

			this.pen.Freeze();

			IsHitTestVisible = false;
		}

		public void UpdateBrush(Brush brush, double thickness)
		{
			this.pen = new Pen(brush, thickness);

			this.pen.Freeze();
		}
		public void ClearTarget()
		{
			this.direction = DirectionEnum.None;
			InvalidateVisual();
		}
		public void UpdateTarget(MouseDevice mouseDevice, FrameworkElement target, Orientation orientation)
		{
			Point itemsControlTestPoint = mouseDevice.GetPosition(AdornedElement);
			Point targetTestPoint = mouseDevice.GetPosition(target);
			Vector offset = itemsControlTestPoint - targetTestPoint;
			double width = target.ActualWidth;
			double height = target.ActualHeight;

			this.leftTop = new Point(offset.X, offset.Y);
			this.topRight = new Point(offset.X + width, offset.Y);
			this.rightBottom = new Point(offset.X + width, offset.Y + height);
			this.bottomLeft = new Point(offset.X, offset.Y + height);

			double centerWidth = width / 2d;
			double centerHight = height / 2d;

			this.direction = DirectionEnum.None;
			switch (orientation)
			{
				case Orientation.Horizontal:
				{
					this.direction |= DirectionEnum.Top;
					this.direction |= DirectionEnum.Bottom;
					if (targetTestPoint.X <= centerWidth)
					{
						this.direction |= DirectionEnum.Left;
					}
					else
					{
						this.direction |= DirectionEnum.Right;
					}
				}
				break;
				default:
				case Orientation.Vertical:
				{
					this.direction |= DirectionEnum.Right;
					this.direction |= DirectionEnum.Left;
					if (targetTestPoint.Y <= centerHight)
					{
						this.direction |= DirectionEnum.Top;
					}
					else
					{
						this.direction |= DirectionEnum.Bottom;
					}
				}
				break;
			}

			if (( this.direction & DirectionEnum.TopLine ) == DirectionEnum.TopLine)
			{
				DropPosition = DropPositionEnum.Before;
			}

			if (( this.direction & DirectionEnum.RightLine ) == DirectionEnum.RightLine)
			{
				DropPosition = DropPositionEnum.After;
			}

			if (( this.direction & DirectionEnum.BottomLine ) == DirectionEnum.BottomLine)
			{
				DropPosition = DropPositionEnum.After;
			}

			if (( this.direction & DirectionEnum.LeftLine ) == DirectionEnum.LeftLine)
			{
				DropPosition = DropPositionEnum.Before;
			}

			InvalidateVisual();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			if (( this.direction & DirectionEnum.TopLine ) == DirectionEnum.TopLine)
			{
				drawingContext.DrawLine(this.pen, this.leftTop, this.topRight);
			}

			if (( this.direction & DirectionEnum.RightLine ) == DirectionEnum.RightLine)
			{
				drawingContext.DrawLine(this.pen, this.topRight, this.rightBottom);
			}

			if (( this.direction & DirectionEnum.BottomLine ) == DirectionEnum.BottomLine)
			{
				drawingContext.DrawLine(this.pen, this.rightBottom, this.bottomLeft);
			}

			if (( this.direction & DirectionEnum.LeftLine ) == DirectionEnum.LeftLine)
			{
				drawingContext.DrawLine(this.pen, this.bottomLeft, this.leftTop);
			}
		}

		public void Detath()
		{
			if (this.adornedLayer != null)
			{
				this.adornedLayer.Remove(this);
			}
		}
	}
}

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPFDragToReorder.Model
{
	class DragAdorner : Adorner
	{
		static Pen borderPen = new Pen(Brushes.DarkGray, 1) { DashStyle = new DashStyle(new double[] { 1d, 5d }, 0d) };
		readonly FrameworkElement frameworkElement;
		readonly Point offset;
		Point location;
		readonly VisualBrush visualBrush;

		static DragAdorner()
		{
			if (borderPen.CanFreeze)
			{
				borderPen.Freeze();
			}
		}
		readonly AdornerLayer adornedLayer;
		public DragAdorner(UIElement adornedElement, FrameworkElement frameworkElement, Point offset) : base(adornedElement)
		{
			this.adornedLayer = AdornerLayer.GetAdornerLayer(AdornedElement);

			if (this.adornedLayer != null)
			{
				this.adornedLayer.Add(this);
			}

			this.frameworkElement = frameworkElement;

			this.offset = offset;

			this.visualBrush = new VisualBrush(frameworkElement);

			this.visualBrush.Opacity = 0.7d;

			IsHitTestVisible = false;
		}

		public void UpdatePoint(Point location)
		{
			this.location = location;
			InvalidateVisual();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			var p = location;

			p.Offset(offset.X, offset.Y);

			drawingContext.DrawRectangle(visualBrush, borderPen, new Rect(p, this.frameworkElement.RenderSize));
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

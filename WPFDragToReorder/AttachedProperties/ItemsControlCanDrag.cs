using WPFDragToReorder.DataStruct;
using WPFDragToReorder.Enum;
using WPFDragToReorder.Interface;
using WPFDragToReorder.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDragToReorder.AttachedProperties
{
	public static partial class ItemsControlSet
	{
		static DragInfo dragInfo;
		static Point mouseDownPoint;
		static DragAdorner dragAdorner;
		static LineAdorner lineAdorner;
		const double dragBeginedOffset = 1d;
		public static bool GetCanDrag(DependencyObject obj)
		{
			return (bool)obj.GetValue(CanDragProperty);
		}

		public static void SetCanDrag(DependencyObject obj, bool value)
		{
			obj.SetValue(CanDragProperty, value);
		}

		// Using a DependencyProperty as the backing store for CanDrag.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CanDragProperty =
			DependencyProperty.RegisterAttached("CanDrag", typeof(bool), typeof(ItemsControl), new PropertyMetadata(false, OnCanDragValueChanged));

		private static void OnCanDragValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ItemsControl itemsControl)
			{
				if ((bool)e.NewValue)
				{
					itemsControl.PreviewMouseDown += ItemsControl_PreviewMouseDown;
					itemsControl.PreviewMouseMove += ItemsControl_PreviewMouseMove;
					itemsControl.PreviewMouseUp += ItemsControl_PreviewMouseUp;
					itemsControl.Unloaded += ItemsControl_Unloaded;
				}
				else
				{
					ReleaseEvent(itemsControl);
				}
			}
		}

		private static void ItemsControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (sender is ItemsControl itemsControl)
			{
				if (Window.GetWindow(itemsControl) == null)
				{
					ReleaseEvent(itemsControl);
				}
			}
		}

		private static void ItemsControl_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is ItemsControl itemsControl && e.Source is DependencyObject dependencyObject)
			{
				if (GetItem(itemsControl.ItemContainerGenerator, e.OriginalSource, out DependencyObject itemContainer, out object item))
				{
					dragInfo = new DragInfo()
					{
						ItemsControl = sender as ItemsControl,
						ItemContainer = itemContainer,
						Item = item,
						OriginIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer)
					};

					mouseDownPoint = e.GetPosition(itemsControl);

					SetDragStatus(itemsControl, DragStatusEnum.Begining);
				}
			}
		}

		private static void ItemsControl_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (sender is ItemsControl itemsControl)
			{
				DragStatusEnum dragStatus = GetDragStatus(itemsControl);

				switch (dragStatus)
				{
					case DragStatusEnum.None:
						break;
					case DragStatusEnum.Begining:
					{
						if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
						{
							Vector vector = e.GetPosition(itemsControl) - mouseDownPoint;
							if (vector.X > dragBeginedOffset || vector.Y > dragBeginedOffset)
							{
								SetDragStatus(itemsControl, DragStatusEnum.Draging);

								itemsControl.CaptureMouse();

								Window window = Window.GetWindow(itemsControl);

								if (window != null && window.Content is UIElement element
									&& dragInfo.ItemContainer is FrameworkElement frameworkElement)
								{
									if (dragAdorner != null)
									{
										dragAdorner.Detath();
									}
									Point offset = e.GetPosition(frameworkElement);
									dragAdorner = new DragAdorner(element, frameworkElement, new Point() { X = -offset.X, Y = -offset.Y });
								}
							}
						}
					}
					break;
					case DragStatusEnum.Draging:
					{
						if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
						{

							if (dragAdorner != null)
							{
								Window window = Window.GetWindow(itemsControl);
								if (window != null)
								{
									dragAdorner.UpdatePoint(e.GetPosition(window));
								}
							}

							if (GetItem(itemsControl.ItemContainerGenerator, itemsControl.InputHitTest(e.GetPosition(itemsControl)), out DependencyObject itemContainer, out object item))
							{
								if (/*itemContainer != dragInfo.ItemContainer && */itemContainer is FrameworkElement frameworkElement)
								{
									if (lineAdorner == null)
									{
										lineAdorner = new LineAdorner(itemsControl);
										lineAdorner.UpdateBrush(GetDropTargetLineBrush(itemsControl), GetDropTargetLineThickness(itemsControl));
									}

									lineAdorner.UpdateTarget(e.MouseDevice, frameworkElement, GetItemPanelOrientation(itemsControl));
								}
							}
							else
							{
								if (lineAdorner != null)
								{
									lineAdorner.ClearTarget();
								}
							}
						}
					}
					break;
					default:
						break;
				}

			}
		}

		private static void ItemsControl_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (sender is ItemsControl itemsControl)
			{
				if (GetDragStatus(itemsControl) == DragStatusEnum.Draging)
				{
					IDropHandler dargHandler = GetDragHandler(itemsControl);
					if (dargHandler != null)
					{
						if (GetItem(itemsControl.ItemContainerGenerator, itemsControl.InputHitTest(e.GetPosition(itemsControl)), out DependencyObject itemContainer, out object item))
						{
							if (itemContainer != dragInfo.ItemContainer)
							{
								dragInfo.TargetIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer);

								dargHandler.OnDrag(dragInfo);
								//if (lineAdorner == null)
								//{
								//	dargHandler.OnDrag(dragInfo);
								//}
								//else
								//{
								//	if (dragInfo.OriginIndex - dragInfo.TargetIndex == 1)// 
								//	{
								//		if (lineAdorner.DropPosition != DropPositionEnum.After)
								//		{
								//			dargHandler.OnDrag(dragInfo);
								//		}
								//	}
								//	else if (dragInfo.OriginIndex - dragInfo.TargetIndex == -1)
								//	{
								//		if (lineAdorner.DropPosition != DropPositionEnum.Before)
								//		{
								//			dargHandler.OnDrag(dragInfo);
								//		}
								//	}
								//	else
								//	{
								//		dargHandler.OnDrag(dragInfo);
								//	}
								//}								
							}
						}
					}

					if (dragAdorner != null)
					{
						dragAdorner.Detath();

						dragAdorner = null;
					}
					if (lineAdorner != null)
					{
						lineAdorner.Detath();

						lineAdorner = null;
					}

					itemsControl.ReleaseMouseCapture();
				}

				dragInfo = default(DragInfo);

				SetDragStatus(itemsControl, DragStatusEnum.None);
			}
		}

		private static bool GetItem(ItemContainerGenerator itemContainerGenerator, object originalSource, out DependencyObject itemContainer, out object item)
		{
			itemContainer = null;
			item = null;
			if (originalSource is DependencyObject dependencyObject)
			{
				item = itemContainerGenerator.ItemFromContainer(dependencyObject);

				if (item == null || item == DependencyProperty.UnsetValue)
				{
					return GetItem(itemContainerGenerator, VisualTreeHelper.GetParent(dependencyObject), out itemContainer, out item);
				}
				else
				{
					itemContainer = dependencyObject;
					return true;
				}
			}
			return false;
		}
		private static void ReleaseEvent(ItemsControl itemsControl)
		{
			itemsControl.PreviewMouseDown -= ItemsControl_PreviewMouseDown;
			itemsControl.PreviewMouseMove -= ItemsControl_PreviewMouseMove;
			itemsControl.PreviewMouseUp -= ItemsControl_PreviewMouseUp;
			itemsControl.Unloaded -= ItemsControl_Unloaded;
		}
	}
}

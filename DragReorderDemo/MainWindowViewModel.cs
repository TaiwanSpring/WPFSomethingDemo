using LightMvvm.ViewModel;
using WPFDragToReorder.Enum;
using WPFDragToReorder.Interface;

namespace ListItemDrag
{
	class MainWindowViewModel : ViewModelBase
	{
		private DragStatusEnum dragStatus;

		public DragStatusEnum DragStatus
		{
			get { return this.dragStatus; }
			set { SetProperty(ref this.dragStatus, value); }
		}


		private IDropHandler dropHandler = new TestDropHandler();

		public IDropHandler DragHandler
		{
			get { return this.dropHandler; }
			set { SetProperty(ref this.dropHandler, value); }
		}

	}
}

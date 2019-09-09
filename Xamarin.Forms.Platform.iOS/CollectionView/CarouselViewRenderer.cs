﻿using System.ComponentModel;

namespace Xamarin.Forms.Platform.iOS
{
	public class CarouselViewRenderer : ItemsViewRenderer
	{
		CarouselView CarouselView => (CarouselView)Element;

		CarouselViewController CarouselViewController => (CarouselViewController)ItemsViewController;

		public CarouselViewRenderer()
		{
			CollectionView.VerifyCollectionViewFlagEnabled(nameof(CarouselViewRenderer));
		}

		protected override ItemsViewController CreateController(ItemsView newElement, ItemsViewLayout layout)
		{
			return new CarouselViewController(newElement as CarouselView, layout);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs changedProperty)
		{
			base.OnElementPropertyChanged(sender, changedProperty);

			if (changedProperty.Is(CarouselView.PeekAreaInsetsProperty))
			{
				(CarouselViewController.Layout as CarouselViewLayout).UpdateConstraints(Frame.Size);
				CarouselViewController.Layout.InvalidateLayout();
			}
			else if (changedProperty.Is(CarouselView.IsSwipeEnabledProperty))
				UpdateIsSwipeEnabled();
			else if (changedProperty.Is(CarouselView.IsBounceEnabledProperty))
				UpdateIsBounceEnabled();
		}

		protected override ItemsViewLayout SelectLayout(IItemsLayout layoutSpecification, ItemSizingStrategy itemSizingStrategy)
		{
			if (layoutSpecification is ListItemsLayout listItemsLayout)
			{
				return new CarouselViewLayout(listItemsLayout, itemSizingStrategy, CarouselView);
			}

			// Fall back to horizontal carousel
			return new CarouselViewLayout(new ListItemsLayout(ItemsLayoutOrientation.Horizontal), itemSizingStrategy, CarouselView);
		}

		protected override void SetUpNewElement(ItemsView newElement)
		{
			base.SetUpNewElement(newElement);
			InitializeSnapBehavior();
			UpdateIsSwipeEnabled();
			UpdateIsBounceEnabled();
		}

		protected override void TearDownOldElement(ItemsView oldElement)
		{
			CarouselViewController?.TearDown();
			base.TearDownOldElement(oldElement);
		}

		void InitializeSnapBehavior()
		{
			if (CarouselView == null || !(CarouselView.ItemsLayout is ItemsLayout itemsLayout))
				return;

			if (itemsLayout.SnapPointsType == SnapPointsType.None)
				itemsLayout.SnapPointsType = SnapPointsType.Mandatory;
		}

		void UpdateIsSwipeEnabled()
		{
			if (CarouselView == null)
				return;

			CarouselViewController.CollectionView.ScrollEnabled = CarouselView.IsSwipeEnabled;
		}

		void UpdateIsBounceEnabled()
		{
			if (CarouselView == null)
				return;

			CarouselViewController.CollectionView.Bounces = CarouselView.IsBounceEnabled;
		}
	}
}

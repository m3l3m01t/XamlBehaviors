﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Xaml.Interactions.Media
{
	using Interactivity;

	/// <summary>
	/// An action that will change the state of the specified <seealso cref="UI.Xaml.Media.Animation.Storyboard"/> when executed.
	/// </summary>
	public sealed class ControlStoryboardAction : DependencyObject, IAction
	{
		/// <summary>
		/// Identifies the <seealso cref="ControlStoryboardOption"/> dependency property.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly DependencyProperty ControlStoryboardOptionProperty = DependencyProperty.Register(
			"ControlStoryboardOption",
			typeof(ControlStoryboardOption),
			typeof(ControlStoryboardAction),
			new PropertyMetadata(ControlStoryboardOption.Play));

		/// <summary>
		/// Identifies the <seealso cref="Storyboard"/> dependency property.
		/// </summary>
		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(
			"Storyboard",
			typeof(Storyboard),
			typeof(ControlStoryboardAction),
			new PropertyMetadata(null, new PropertyChangedCallback(ControlStoryboardAction.OnStoryboardChanged)));

		private bool _isPaused;

		/// <summary>
		/// Gets or sets the action to execute on the <see cref="UI.Xaml.Media.Animation.Storyboard"/>. This is a dependency property.
		/// </summary>
		public ControlStoryboardOption ControlStoryboardOption
		{
			get
			{
				return (ControlStoryboardOption)this.GetValue(ControlStoryboardAction.ControlStoryboardOptionProperty);
			}
			set
			{
				this.SetValue(ControlStoryboardAction.ControlStoryboardOptionProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the targeted <see cref="UI.Xaml.Media.Animation.Storyboard"/>. This is a dependency property.
		/// </summary>
		public Storyboard Storyboard
		{
			get
			{
				return (Storyboard)this.GetValue(ControlStoryboardAction.StoryboardProperty);
			}
			set
			{
				this.SetValue(ControlStoryboardAction.StoryboardProperty, value);
			}
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
		/// <param name="parameter">The value of this parameter is determined by the caller.</param>
		/// <returns>True if the specified operation is invoked successfully; else false.</returns>
		public object Execute(object sender, object parameter)
		{
			if (this.Storyboard == null)
			{
				return false;
			}

			switch (this.ControlStoryboardOption)
			{
				case ControlStoryboardOption.Play:
					this.Storyboard.Begin();
					break;

				case ControlStoryboardOption.Stop:
					this.Storyboard.Stop();
					break;

				case ControlStoryboardOption.TogglePlayPause:
					{
						ClockState currentState = this.Storyboard.GetCurrentState();

						if (currentState == ClockState.Stopped)
						{
							this._isPaused = false;
							this.Storyboard.Begin();
						}
						else if (this._isPaused)
						{
							this._isPaused = false;
							this.Storyboard.Resume();
						}
						else
						{
							this._isPaused = true;
							this.Storyboard.Pause();
						}
					}

					break;

				case ControlStoryboardOption.Pause:
					this.Storyboard.Pause();
					break;

				case ControlStoryboardOption.Resume:
					this.Storyboard.Resume();
					break;

				case ControlStoryboardOption.SkipToFill:
					this.Storyboard.SkipToFill();
					break;

				default:
					return false;
			}

			return true;
		}

		private static void OnStoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			ControlStoryboardAction action = sender as ControlStoryboardAction;
			if (action != null)
			{
				action._isPaused = false;
			}
		}
	}
}

using MochaCore.Navigation;
using System;
using System.Windows;

namespace MochaWPF.Navigation
{
    /// <summary>
    /// Provides a typical implementation of <see cref="INavigationModule"/> for WPF apps.
    /// </summary>
    public class NavigationModule : INavigationModule
    {
        protected FrameworkElement _view;
        protected INavigatable _dataContext;

        /// <inheritdoc/>
        public virtual object View => _view;

        /// <inheritdoc/>
        public virtual INavigatable DataContext => _dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationModule"/> class.
        /// <para>
        /// Use this constructor if provided *view* object has already assigned adequate 
        /// DataContext in XAML. Throws if DataContext is not assigned to provided view or its not *INavigatable*.
        /// </para>
        /// </summary>
        /// <param name="view">Visual representation for this module. A <see langword="UserControl"/> is a good choice here.</param>
        /// <exception cref="InvalidOperationException"/>
        public NavigationModule(FrameworkElement view)
        {
            _view = view;

            if (view.DataContext is not null && view.DataContext is INavigatable navigatable)
            {
                _dataContext = navigatable;
            }
            else
            {
                throw new InvalidOperationException("Data context was not assigned for provided *view* or wasn't *INavigatable*");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationModule"/> class.
        /// </summary>
        /// <param name="view">Visual representation for this module. A <see langword="UserControl"/> is a good choice here.</param>
        /// <param name="dataContext">
        /// An <see cref="INavigatable"/> which will be bounded to <see cref="View"/> object by DataBinding mechanism.
        /// </param>
        public NavigationModule(FrameworkElement view, INavigatable dataContext)
        {
            _view = view;
            _dataContext = dataContext;
        }

        /// <inheritdoc/>
        public virtual void CleanUp()
        {
            _view.DataContext = null;
        }

        /// <inheritdoc/>
        public virtual void SetDataContext(INavigatable dataContext)
        {
            _view.DataContext = dataContext;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        public override bool Equals(object? obj)
        {
            if (obj is INavigationModule comparing)
            {
                bool viewEqual = comparing.View?.GetType() == this.View?.GetType();
                bool navigatableEqual = comparing.DataContext?.GetType() == this.DataContext.GetType();

                return viewEqual && navigatableEqual;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_view, _dataContext, View, DataContext);
        }
    }
}

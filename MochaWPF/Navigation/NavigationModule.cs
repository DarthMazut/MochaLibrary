using Mocha.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Returns a reference to technology-specific view object.
        /// </summary>
        public virtual object View => _view;

        /// <summary>
        /// An <see cref="INavigatable"/> object bounded to <see cref="View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        public virtual INavigatable DataContext => _dataContext;

        /// <summary>
        /// Returns a new instance of the <see cref="NavigationModule"/> class.
        /// </summary>
        /// <param name="view">Visual representation for this module. A *UserControl* is good choice here.</param>
        /// <param name="dataContext"></param>
        public NavigationModule(FrameworkElement view, INavigatable dataContext)
        {
            _view = view;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Invoked when this module is no longer in use and can be garbage collected.
        /// This method is called internally by <see cref="NavigationService"/>.
        /// </summary>
        public virtual void CleanUp()
        {
            _view.DataContext = null;
        }

        /// <summary>
        /// Sets a *DataContext* for technology-specific <see cref="View"/> object.
        /// </summary>
        /// <param name="dataContext">Object to be bound with <see cref="View"/> object by *DataBinding* mechanism.</param>
        public virtual void SetDataContext(INavigatable dataContext)
        {
            _view.DataContext = dataContext;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        public override bool Equals(object obj)
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
            var hashCode = 536724180;
            hashCode = hashCode * -1521134295 + EqualityComparer<FrameworkElement>.Default.GetHashCode(_view);
            hashCode = hashCode * -1521134295 + EqualityComparer<INavigatable>.Default.GetHashCode(_dataContext);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(View);
            hashCode = hashCode * -1521134295 + EqualityComparer<INavigatable>.Default.GetHashCode(DataContext);
            return hashCode;
        }
    }
}

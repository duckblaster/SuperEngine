using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace SuperEngine.Misc {
	public abstract class NotifyPropertyChanged : INotifyPropertyChanged {
		private static readonly Dictionary<string, List<string>> PropertyDependecyMap = new Dictionary<string, List<string>>();

		protected static void PropertyDependsOn(string property, string depends) {
			if (property == depends) {
				return;
			}

			var dependentProperties = PropertyDependecyMap[depends];

			if (dependentProperties == null) {
				dependentProperties = new List<string>();
				PropertyDependecyMap[depends] = dependentProperties;
			}

			if (!dependentProperties.Contains(property)) {
				dependentProperties.Add(property);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
			T oldValue = field;
			if (Equals(oldValue, value)) {
				return;
			}
			field = value;
			OnPropertyChanged(oldValue, value, propertyName);
		}

		protected delegate void OnSetPropertyAction<in T>(T oldValue, T newValue);
		protected virtual void SetProperty<T>(ref T field, T value, OnSetPropertyAction<T> onSetAction, [CallerMemberName] string propertyName = null) {
			T oldValue = field;
			if(Equals(oldValue, value)) {
				return;
			}
			field = value;
			onSetAction(oldValue, value);
			OnPropertyChanged(oldValue, value, propertyName);
		}


		protected virtual void OnPropertyChanged(object oldValue, object newValue, [CallerMemberName] string propertyName = null) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler == null) {
				return;
			}
			handler(this, new PropertyChangedEventArgs(propertyName));

			if (propertyName == null) {
				return;
			}
			List<string> handlerDependentProperties = PropertyDependecyMap[propertyName];

			if (handlerDependentProperties == null) {
				return;
			}

			foreach (string handlerDependentProperty in handlerDependentProperties) {
				handler(this, new PropertyChangedEventArgs(handlerDependentProperty));
			}
		}
	}
}



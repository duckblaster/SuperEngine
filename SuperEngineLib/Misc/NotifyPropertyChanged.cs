using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;

namespace SuperEngine.Misc {
	public class DetailedPropertyChangedEventArgs : PropertyChangedEventArgs{
		object oldValue;
		object newValue;
		Type type;
		string originalPropertyName;

		public virtual object OldValue {
			get {
				return oldValue;
			}
		}
	
		public virtual object NewValue {
			get {
				return newValue;
			}
		}

		public Type Type {
			get {
				return type;
			}
		}

		public string OriginalPropertyName {
			get {
				return originalPropertyName;
			}
		}
		

		public DetailedPropertyChangedEventArgs(string propertyName, string originalPropertyName = null) : base(propertyName) {
			this.originalPropertyName = originalPropertyName;
		}

		public DetailedPropertyChangedEventArgs(object oldValue, object newValue, Type type, string propertyName, string originalPropertyName = null) : base(propertyName) {
			this.oldValue = oldValue;
			this.newValue = newValue;
			this.type = type;
			this.originalPropertyName = originalPropertyName;
		}
		
	}
	public class DetailedPropertyChangedEventArgs<T> : DetailedPropertyChangedEventArgs{

		public new T OldValue {
			get {
				return (T)base.OldValue;
			}
		}

		public new T NewValue {
			get {
				return (T)base.NewValue;
			}
		}

		/*public static explicit operator DetailedPropertyChangedEventArgs<T>(DetailedPropertyChangedEventArgs args){
			if(typeof(T).IsAssignableFrom(args.Type)) {
				return new DetailedPropertyChangedEventArgs<T>(args.OldValue, args.NewValue, args.PropertyName, args.OriginalPropertyName);
			}
		}*/

		public DetailedPropertyChangedEventArgs(T oldValue, T newValue, string propertyName, string originalPropertyName = null) : base(oldValue, newValue, typeof(T), propertyName, originalPropertyName) {
		}
		/*public DetailedPropertyChangedEventArgs(DetailedPropertyChangedEventArgs args) : base(args.OldValue, args.NewValue, args.PropertyName, typeof(T), args.OriginalPropertyName) {
		}*/

	}
	public abstract class NotifyPropertyChanged : INotifyPropertyChanged {
		static readonly Dictionary<Type, Dictionary<string, List<string>>> PropertyDependecyMap = new Dictionary<Type, Dictionary<string, List<string>>>();
		static readonly Dictionary<Type, Dictionary<string, List<string>>> PropertySubDependecyMap = new Dictionary<Type, Dictionary<string, List<string>>>();

		Dictionary<string, List<string>> localPropertyDependecyMap;
		Dictionary<string, List<string>> localPropertySubDependecyMap;
		Dictionary<NotifyPropertyChanged, PropertyChangedEventHandler> subPropertyEventHandlers = new Dictionary<NotifyPropertyChanged, PropertyChangedEventHandler>();

		static Dictionary<Type, bool> initializedTypes = new Dictionary<Type, bool>();
		bool initialized;
		Type myType;

		static Dictionary<string, List<string>> PropertyDependecyMapForType(Type t) {
			Dictionary<string, List<string>> result;
			if(!PropertyDependecyMap.TryGetValue(t, out result)) {
				result = new Dictionary<string, List<string>>();
				PropertyDependecyMap[t] = result;
			}
			return result;
		}

		static Dictionary<string, List<string>> PropertySubDependecyMapForType(Type t) {
			Dictionary<string, List<string>> result;
			if(!PropertySubDependecyMap.TryGetValue(t, out result)) {
				result = new Dictionary<string, List<string>>();
				PropertySubDependecyMap[t] = result;
			}
			return result;
		}

		protected static void PropertyDependsOn<T>(string property, string depends) {
			Type t = typeof(T);
			if(initializedTypes.ContainsKey(t) && initializedTypes[t]) {
				throw new InvalidOperationException(string.Format("NotifyPropertyChanged.PropertyDependsOn must be called from a static constructor. Called by {0}", t));
			}
			if (property == depends) {
				return;
			}
			Dictionary<string, List<string>> propertyDependecyMap = PropertyDependecyMapForType(t);
			List<string> dependentProperties;

			if (!propertyDependecyMap.TryGetValue(depends, out dependentProperties)) {
				dependentProperties = new List<string>();
				propertyDependecyMap[depends] = dependentProperties;
			}

			if (!dependentProperties.Contains(property)) {
				dependentProperties.Add(property);
			}
		}

		protected static void PropertySubDependsOn<T>(string property, string depends) {
			Type t = typeof(T);
			if(initializedTypes.ContainsKey(t) && initializedTypes[t]) {
				throw new InvalidOperationException(string.Format("NotifyPropertyChanged.PropertySubDependsOn must be called from a static constructor. Called by {0}", t));
			}
			if (property == depends) {
				return;
			}
			Dictionary<string, List<string>> propertySubDependecyMap = PropertySubDependecyMapForType(t);
			List<string> dependentSubProperties;

			if (!propertySubDependecyMap.TryGetValue(depends, out dependentSubProperties)) {
				dependentSubProperties = new List<string>();
				propertySubDependecyMap[depends] = dependentSubProperties;
			}

			if (!dependentSubProperties.Contains(property)) {
				dependentSubProperties.Add(property);
			}
		}
		void Initialize(){
			myType = GetType();
			Initialize(myType);
			localPropertyDependecyMap = PropertyDependecyMapForType(myType);
			localPropertySubDependecyMap = PropertySubDependecyMapForType(myType);
			initialized = true;
		}
		static void Initialize(Type t) {
			if(initializedTypes.ContainsKey(t) && initializedTypes[t]) {
				return;
			}
			Type baseType = t.BaseType;
			if(baseType == typeof(NotifyPropertyChanged))
				return;
			Initialize(baseType);

			Dictionary<string, List<string>> propertyDependecyMap = PropertyDependecyMapForType(t);
			Dictionary<string, List<string>> basePropertyDependecyMap = PropertyDependecyMapForType(baseType);
			foreach(var itemKey in basePropertyDependecyMap.Keys) {
				List<string> itemList;
				List<string> itemBaseList;

				if (!propertyDependecyMap.TryGetValue(itemKey, out itemList)) {
					itemList = new List<string>();
					propertyDependecyMap[itemKey] = itemList;
				}

				if (!basePropertyDependecyMap.TryGetValue(itemKey, out itemBaseList)) {
					itemBaseList = new List<string>();
					basePropertyDependecyMap[itemKey] = itemBaseList;
				}

				foreach(var item in itemBaseList) {
					if(!itemList.Contains(item)) {
						itemList.Add(item);
					}
				}
			}
			initializedTypes[t] = true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected delegate void OnSetPropertyAction<in T>(T oldValue, T newValue);
		protected delegate void OnBeforeSetPropertyAction<T>(T oldValue, ref T newValue);

		protected virtual void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
			if (Equals(field, value)) {
				return;
			}
			T oldValue = field;
			field = value;
			OnPropertyChanged<T>(oldValue, value, propertyName);
		}

		protected virtual void SetProperty<T>(ref T field, T value, OnSetPropertyAction<T> onSetAction, [CallerMemberName] string propertyName = null) {
			if(Equals(field, value)) {
				return;
			}
			T oldValue = field;
			field = value;
			onSetAction(oldValue, value);
			OnPropertyChanged<T>(oldValue, value, propertyName);
		}

		protected virtual void SetProperty<T>(ref T field, T value, OnBeforeSetPropertyAction<T> onBeforeSetAction, OnSetPropertyAction<T> onSetAction, [CallerMemberName] string propertyName = null) {
			if(Equals(field, value)) {
				return;
			}
			T oldValue = field;
			onBeforeSetAction(oldValue, ref value);
			if(oldValue.Equals(value)){
				return;
			}
			field = value;
			onSetAction(oldValue, value);
			OnPropertyChanged<T>(oldValue, value, propertyName);
		}
		
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null, string originalPropertyName = null) {
			if(!initialized){
				Initialize();
			}
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler == null) {
				return;
			}
			handler(this, new DetailedPropertyChangedEventArgs(propertyName));

			if (propertyName == null) {
				return;
			}

			List<string> handlerDependentProperties;

			if (!localPropertyDependecyMap.TryGetValue(propertyName, out handlerDependentProperties)) {
				return;
			}

			foreach (string handlerDependentProperty in handlerDependentProperties) {
				OnPropertyChanged(handlerDependentProperty, propertyName);
			}
		}

		protected virtual void OnPropertyChanged(object oldValue, object newValue, Type type, [CallerMemberName] string propertyName = null, string originalPropertyName = null) {
			if(!initialized){
				Initialize();
			}
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler == null) {
				return;
			}
			handler(this, new DetailedPropertyChangedEventArgs(oldValue, newValue, type, propertyName));

			HandleDependentPropertyChanges(oldValue, newValue, type, propertyName, originalPropertyName);
		}
		protected virtual void OnPropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] string propertyName = null, string originalPropertyName = null) {
			if(!initialized){
				Initialize();
			}
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler == null) {
				return;
			}
			handler(this, new DetailedPropertyChangedEventArgs<T>(oldValue, newValue, propertyName));

			HandleDependentPropertyChanges(oldValue, newValue, typeof(T), propertyName, originalPropertyName);
		}

		static PropertyChangedEventHandler GetSubPropertyHandler(NotifyPropertyChanged self, string propertyName, string subPropertyName) {
			return (object sender, PropertyChangedEventArgs e) =>  {
				var args = e as DetailedPropertyChangedEventArgs;
				if(args != null) {
					if(e.PropertyName == subPropertyName) {
						self.OnPropertyChanged(propertyName + "." + e.PropertyName, e.PropertyName);
					}
				}
				else
					if(args.PropertyName == subPropertyName) {
						self.OnPropertyChanged(args.OldValue, args.NewValue, args.Type, propertyName + "." + args.PropertyName, args.PropertyName);
					}
			};
		}

		protected virtual void HandleDependentPropertyChanges(object oldValue, object newValue, Type type, string propertyName = null, string originalPropertyName = null) {
			if (propertyName == null) {
				return;
			}

			List<string> handlerDependentProperties;

			if (!localPropertyDependecyMap.TryGetValue(propertyName, out handlerDependentProperties)) {
				return;
			}

			foreach (string handlerDependentProperty in handlerDependentProperties) {
				OnPropertyChanged(oldValue, newValue, type, handlerDependentProperty, propertyName);
			}

			List<string> handlerSubDependentProperties;

			if (!localPropertySubDependecyMap.TryGetValue(propertyName, out handlerSubDependentProperties)) {
				return;
			}

			foreach (string handlerSubDependentProperty in handlerSubDependentProperties) {
				PropertyChangedEventHandler handler;
				var oldNotify = oldValue as NotifyPropertyChanged;
				if(oldNotify != null) {
					if(subPropertyEventHandlers.TryGetValue(oldNotify, out handler)) {
						oldNotify.PropertyChanged -= handler;
						subPropertyEventHandlers.Remove(oldNotify);
					}
				}
				var newNotify = newValue as NotifyPropertyChanged;
				if(newNotify != null) {
					handler = GetSubPropertyHandler(this, propertyName, handlerSubDependentProperty);
					newNotify.PropertyChanged += handler;
					subPropertyEventHandlers[newNotify] = handler;
				}
			}
		}
	}
}



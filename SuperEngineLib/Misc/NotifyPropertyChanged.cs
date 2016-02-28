using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace SuperEngineLib.Misc {
    public class DetailedPropertyChangedEventArgs : PropertyChangedEventArgs {
        readonly object oldValue;
        readonly object newValue;
        readonly Type type;
        readonly string originalPropertyName;

        public virtual object OldValue {
            get {
                return oldValue;
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


        public DetailedPropertyChangedEventArgs(string propertyName, string originalPropertyName = null)
            : base(propertyName) {
            this.originalPropertyName = originalPropertyName;
        }

        public DetailedPropertyChangedEventArgs(object oldValue, Type type, string propertyName, string originalPropertyName = null)
            : base(propertyName) {
            this.oldValue = oldValue;
            this.type = type;
            this.originalPropertyName = originalPropertyName;
        }

    }

    public class DetailedPropertyChangedEventArgs<T> : PropertyChangedEventArgs {
        readonly T oldValue;
        readonly Type type;
        readonly string originalPropertyName;

        public virtual T OldValue {
            get {
                return oldValue;
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


        public DetailedPropertyChangedEventArgs(string propertyName, string originalPropertyName = null)
            : base(propertyName) {
            this.originalPropertyName = originalPropertyName;
        }

        public DetailedPropertyChangedEventArgs(T oldValue, string propertyName, string originalPropertyName = null)
            : base(propertyName) {
            this.oldValue = oldValue;
            type = typeof(T);
            this.originalPropertyName = originalPropertyName;
        }

        public static explicit operator DetailedPropertyChangedEventArgs<T>(DetailedPropertyChangedEventArgs args) {
            return new DetailedPropertyChangedEventArgs<T>((T)args.OldValue, args.PropertyName, args.OriginalPropertyName);
        }

        public static implicit operator DetailedPropertyChangedEventArgs(DetailedPropertyChangedEventArgs<T> args) {
            return new DetailedPropertyChangedEventArgs(args.OldValue, args.Type, args.PropertyName, args.OriginalPropertyName);
        }

        public DetailedPropertyChangedEventArgs(DetailedPropertyChangedEventArgs args)
            : base(args.PropertyName) {
            if (!typeof(T).IsAssignableFrom(args.Type)) {
                throw new InvalidCastException();
            }
            oldValue = (T)args.OldValue;
            type = typeof(T);
            originalPropertyName = args.OriginalPropertyName;
        }

    }

    public abstract class NotifyPropertyChanged : INotifyPropertyChanged {
        static readonly Dictionary<Type, Dictionary<string, List<string>>> PropertyDependecyMap = new Dictionary<Type, Dictionary<string, List<string>>>();
        static readonly Dictionary<Type, Dictionary<string, List<string>>> PropertySubDependecyMap = new Dictionary<Type, Dictionary<string, List<string>>>();

        Dictionary<string, List<string>> localPropertyDependecyMap;
        Dictionary<string, List<string>> localPropertySubDependecyMap;
        readonly Dictionary<NotifyPropertyChanged, PropertyChangedEventHandler> subPropertyEventHandlers = new Dictionary<NotifyPropertyChanged, PropertyChangedEventHandler>();

        static readonly Dictionary<Type, bool> initializedTypes = new Dictionary<Type, bool>();
        bool initialized;
        Type myType;

        static Dictionary<string, List<string>> PropertyDependecyMapForType(Type t) {
            Dictionary<string, List<string>> result;
            if (!PropertyDependecyMap.TryGetValue(t, out result)) {
                result = new Dictionary<string, List<string>>();
                PropertyDependecyMap[t] = result;
            }
            return result;
        }

        static Dictionary<string, List<string>> PropertySubDependecyMapForType(Type t) {
            Dictionary<string, List<string>> result;
            if (!PropertySubDependecyMap.TryGetValue(t, out result)) {
                result = new Dictionary<string, List<string>>();
                PropertySubDependecyMap[t] = result;
            }
            return result;
        }

        protected static void PropertyDependsOn<T>(string property, string depends) {
            var t = typeof(T);
            if (initializedTypes.ContainsKey(t) && initializedTypes[t]) {
                throw new InvalidOperationException($"NotifyPropertyChanged.PropertyDependsOn must be called from a static constructor. Called by type {t}");
            }
            if (property == depends) {
                return;
            }
            var propertyDependecyMap = PropertyDependecyMapForType(t);
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
            var t = typeof(T);
            if (initializedTypes.ContainsKey(t) && initializedTypes[t]) {
                throw new InvalidOperationException($"NotifyPropertyChanged.PropertySubDependsOn must be called from a static constructor. Called by type {t}");
            }
            if (property == depends) {
                return;
            }
            var propertySubDependecyMap = PropertySubDependecyMapForType(t);
            List<string> dependentSubProperties;

            if (!propertySubDependecyMap.TryGetValue(depends, out dependentSubProperties)) {
                dependentSubProperties = new List<string>();
                propertySubDependecyMap[depends] = dependentSubProperties;
            }

            if (!dependentSubProperties.Contains(property)) {
                dependentSubProperties.Add(property);
            }
        }

        protected void Initialize() {
            if(initialized) {
                return;
            }
            myType = GetType();
            Initialize(myType);
            localPropertyDependecyMap = PropertyDependecyMapForType(myType);
            localPropertySubDependecyMap = PropertySubDependecyMapForType(myType);
            initialized = true;
        }

        static void Initialize(Type t) {
            if (initializedTypes.ContainsKey(t) && initializedTypes[t]) {
                return;
            }
            var baseType = t.BaseType;
            if (baseType == typeof(NotifyPropertyChanged)) {
                initializedTypes[t] = true;
                return;
            }
            Initialize(baseType);

            var propertyDependecyMap = PropertyDependecyMapForType(t);
            var basePropertyDependecyMap = PropertyDependecyMapForType(baseType);
            foreach (KeyValuePair<string, List<string>> property in basePropertyDependecyMap) {
                List<string> itemList;

                if (!propertyDependecyMap.TryGetValue(property.Key, out itemList)) {
                    itemList = new List<string>();
                    propertyDependecyMap[property.Key] = itemList;
                }

                foreach (string dependency in property.Value) {
                    if (!itemList.Contains(dependency)) {
                        itemList.Add(dependency);
                    }
                }
            }
            initializedTypes[t] = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (!initialized) {
                Initialize();
            }
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new DetailedPropertyChangedEventArgs(propertyName));
            }

            if (propertyName == null) {
                return;
            }

            List<string> handlerDependentProperties;

            if (!localPropertyDependecyMap.TryGetValue(propertyName, out handlerDependentProperties)) {
                return;
            }

            foreach (string handlerDependentProperty in handlerDependentProperties) {
                OnPropertyChanged(handlerDependentProperty);
            }
        }

        protected virtual void OnPropertyChanged(object oldValue, Type type, [CallerMemberName] string propertyName = null, string originalPropertyName = null) {
            if (!initialized) {
                Initialize();
            }
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new DetailedPropertyChangedEventArgs(oldValue, type, propertyName));
            }

            HandleDependentPropertyChanges(oldValue, propertyName, originalPropertyName);
        }

        protected virtual void OnPropertyChanged<T>(T oldValue, [CallerMemberName] string propertyName = null, string originalPropertyName = null) {
            if (!initialized) {
                Initialize();
            }
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new DetailedPropertyChangedEventArgs<T>(oldValue, propertyName));
            }

            HandleDependentPropertyChanges(oldValue, propertyName, originalPropertyName);
        }

        static PropertyChangedEventHandler GetSubPropertyHandler(NotifyPropertyChanged self, string propertyName, List<string> subPropertyNames) {
            return (sender, e) => {
                var args = e as DetailedPropertyChangedEventArgs;
                if (subPropertyNames.Contains(e.PropertyName)) {
                    if (args != null) {
                        self.OnPropertyChanged(args.OldValue, args.Type, propertyName + "." + args.PropertyName, args.PropertyName);
                    } else {
                        self.OnPropertyChanged(propertyName + "." + e.PropertyName);
                    }
                }
            };
        }

        protected virtual void HandleDependentPropertyChanges(object oldvalue, string propertyName, string originalPropertyName = null) {
            List<string> handlerDependentProperties;

            if (!localPropertyDependecyMap.TryGetValue(propertyName, out handlerDependentProperties)) {
                return;
            }

            foreach (string handlerDependentProperty in handlerDependentProperties) {
                OnPropertyChanged(handlerDependentProperty, propertyName, originalPropertyName);
            }

            List<string> handlerSubDependentProperties;

            if (!localPropertySubDependecyMap.TryGetValue(propertyName, out handlerSubDependentProperties)) {
                return;
            }

            var oldNotify = oldvalue as NotifyPropertyChanged;
            var newNotify = GetPropertyValueByName(propertyName);

            PropertyChangedEventHandler handler;
            if (oldNotify != null) {
                if (subPropertyEventHandlers.TryGetValue(oldNotify, out handler)) {
                    oldNotify.PropertyChanged -= handler;
                    subPropertyEventHandlers.Remove(oldNotify);
                }
            }
            if (newNotify != null) {
                handler = GetSubPropertyHandler(this, propertyName, handlerSubDependentProperties);
                newNotify.PropertyChanged += handler;
                subPropertyEventHandlers[newNotify] = handler;
            }
        }

        private static Dictionary<string, Func<NotifyPropertyChanged>> propertyGetterCache = new Dictionary<string, Func<NotifyPropertyChanged>>();

        private NotifyPropertyChanged GetPropertyValueByName(string propertyName)
        {
            Func<NotifyPropertyChanged> propertyGetter;
            if (!propertyGetterCache.TryGetValue(propertyName, out propertyGetter))
            {
                propertyGetter = (Func<NotifyPropertyChanged>)GetType().GetProperty(propertyName).GetGetMethod().CreateDelegate(typeof(Func<NotifyPropertyChanged>));
            }
            return propertyGetter();
        }
    }
}



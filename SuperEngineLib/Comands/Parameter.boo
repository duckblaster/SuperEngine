namespace SuperEngine.Comands

import System
import Boo.Lang
import Boo.Lang.Useful
import Boo.Lang.Extensions

class Parameter:
"""Description of Parameter"""
	[Property(Name, Observable: true)]
	_name as string
	[Property(Locked)]
	_locked as bool = false
	[Property(ValueType)]
	_valueType as Type
	[Property(Value, _valueType.IsAssignableFrom(value.GetType()) and (not ((value isa ParameterGroup) and _locked)))]
	_value as object
	
	
	public def constructor():
		pass

